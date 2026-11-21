using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SMT.Api.Controllers;
using SMT.Security;
using SMT.Services.Documents;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.DocumentDto;
using SMT.ViewModel.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Documents
{
    public class DocumentControllerPermissionTests : IDisposable
    {
        private readonly string _root;
        private readonly DocumentStorage _storage;
        private readonly FakeUserService _users = new FakeUserService();

        public DocumentControllerPermissionTests()
        {
            _root = Path.Combine(Path.GetTempPath(), "smt-docs-ctl-" + Guid.NewGuid().ToString("N"));
            _storage = new DocumentStorage(new DocumentStorageOptions { RootFolder = _root });
            _users.Add("ali", "Engineer");
            _users.Add("bob", "Engineer");
            _users.Add("boss", "admin");
            _users.Add("line1", "user");
        }

        public void Dispose()
        {
            if (Directory.Exists(_root))
                Directory.Delete(_root, recursive: true);
        }

        // Builds a controller as if `username` logged in with the given token roles.
        private DocumentController As(string username, params string[] roles)
        {
            var claims = new List<Claim> { new Claim("username", username) };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var controller = new DocumentController(_storage, _users, new ConfigurationBuilder().Build());
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test")) },
            };
            return controller;
        }

        private static int Status(IActionResult result) =>
            result is ObjectResult o ? o.StatusCode ?? 200 : result is StatusCodeResult s ? s.StatusCode : 200;

        [Fact]
        public async Task Engineer_CanChangeOwnFolder()
        {
            var ali = As("ali", "Engineer");

            Assert.Equal(200, Status(await ali.CreateFolder(new DocumentCreateFolder { Owner = "ali", Path = "", Name = "BOM" })));
            Assert.Equal(200, Status(await ali.Rename(new DocumentRename { Owner = "ali", Path = "BOM", NewName = "BOMs" })));
            Assert.Equal(204, Status(await ali.Delete("ali", "BOMs")));
        }

        [Fact]
        public async Task Engineer_CannotChangeSomeoneElsesFolder_ButCanView()
        {
            _storage.CreateFolder("bob", "", "keep");
            var ali = As("ali", "Engineer");

            Assert.Equal(403, Status(await ali.CreateFolder(new DocumentCreateFolder { Owner = "bob", Path = "", Name = "x" })));
            Assert.Equal(403, Status(await ali.Rename(new DocumentRename { Owner = "bob", Path = "keep", NewName = "y" })));
            Assert.Equal(403, Status(await ali.Delete("bob", "keep")));
            Assert.Equal(403, Status(await ali.Upload("bob", "", false)));

            var list = Assert.IsType<OkObjectResult>(await ali.List("bob", ""));
            var listing = Assert.IsType<DocumentListing>(list.Value);
            Assert.False(listing.CanEdit);
            Assert.Single(listing.Entries);
        }

        [Fact]
        public async Task Admin_CanChangeAnyFolder()
        {
            var boss = As("boss", "Admin"); // role claim case differs from the stored "admin"

            Assert.Equal(200, Status(await boss.CreateFolder(new DocumentCreateFolder { Owner = "bob", Path = "", Name = "fixed" })));
            var listing = (DocumentListing)((OkObjectResult)await boss.List("bob", "")).Value;
            Assert.True(listing.CanEdit);
        }

        [Fact]
        public async Task AccountsWithoutTheRole_SeeNothing()
        {
            var line = As("line1", "user");

            Assert.Equal(403, Status(await line.GetOwners()));
            Assert.Equal(403, Status(await line.List("ali", "")));
            Assert.Equal(403, Status(await line.Download("ali", "x.txt")));
            Assert.Equal(403, Status(await line.GetStaff()));
        }

        [Fact]
        public async Task Engineer_CannotManageStaff()
        {
            var ali = As("ali", "Engineer");

            Assert.Equal(403, Status(await ali.GetStaff()));
            Assert.Equal(403, Status(await ali.CreateStaff(new DocumentStaffCreate { Username = "eve", Password = "x" })));
            Assert.Equal(403, Status(await ali.ResetPassword("bob", new DocumentPasswordReset { Password = "x" })));
            Assert.Equal(403, Status(await ali.DeleteStaff("bob")));
        }

        [Fact]
        public async Task FoldersOnlyExistForEngineers()
        {
            var boss = As("boss", "admin");

            // "line1" is a real login but has no folder; admins must not create one by accident.
            await Assert.ThrowsAsync<NotFoundException>(() =>
                boss.CreateFolder(new DocumentCreateFolder { Owner = "line1", Path = "", Name = "x" }));
            await Assert.ThrowsAsync<NotFoundException>(() => boss.List("nobody", ""));
        }

        [Fact]
        public async Task Admin_AddsNewAndExistingAccounts()
        {
            var boss = As("boss", "admin");

            Assert.Equal(200, Status(await boss.CreateStaff(new DocumentStaffCreate { Username = "eve", Password = "Pass-123" })));
            Assert.True(await _users.IsInRoleAsync("eve", "Engineer"));
            Assert.Equal("Pass-123", _users.Passwords["eve"]);

            // The admin's own login gets a folder without touching its password.
            Assert.Equal(200, Status(await boss.CreateStaff(new DocumentStaffCreate { Username = "boss" })));
            Assert.True(await _users.IsInRoleAsync("boss", "Engineer"));
            Assert.False(_users.Passwords.ContainsKey("boss"));

            await Assert.ThrowsAsync<ConflictException>(() => boss.CreateStaff(new DocumentStaffCreate { Username = "eve", Password = "x" }));
            await Assert.ThrowsAsync<InvalidOperationException>(() => boss.CreateStaff(new DocumentStaffCreate { Username = "../x", Password = "x" }));
            await Assert.ThrowsAsync<InvalidOperationException>(() => boss.DeleteStaff("boss"));
        }

        [Fact]
        public async Task Folders_ShowTitles_InProcessStepOrder()
        {
            var boss = As("boss", "admin");
            foreach (var (login, title) in new[]
            {
                ("Smk", "10 Менеджер СМК"), ("Metrologiya", "1 Метрология"), ("Znaniya", "11 Внутрифирменные знание"),
                ("Kto", "2 КТО"), ("Sklad", "8 Склад"),
            })
                await boss.CreateStaff(new DocumentStaffCreate { Username = login, Password = "x", Title = title });

            var owners = (DocumentFolderOwner[])((OkObjectResult)await As("ali", "Engineer").GetOwners()).Value;

            // Numbered first by step (1, 2, 8, 10, 11 - not 1, 10, 11, 2), untitled logins after.
            Assert.Equal(
                new[] { "1 Метрология", "2 КТО", "8 Склад", "10 Менеджер СМК", "11 Внутрифирменные знание", "ali", "bob" },
                owners.Select(o => o.Title).ToArray());
            Assert.Equal("Metrologiya", owners[0].Username);
        }

        [Fact]
        public async Task OnlyAdmins_CanRenameFolderTitles()
        {
            Assert.Equal(403, Status(await As("ali", "Engineer").SetTitle("ali", new DocumentTitleUpdate { Title = "1 Мой" })));

            Assert.Equal(204, Status(await As("boss", "admin").SetTitle("ali", new DocumentTitleUpdate { Title = " 3 Отдел Закупок " })));
            Assert.Equal("3 Отдел Закупок", await _users.GetClaimValueAsync("ali", DocumentController.TitleClaim));

            // Clearing the title falls back to the login.
            await As("boss", "admin").SetTitle("ali", new DocumentTitleUpdate { Title = "" });
            var owners = (DocumentFolderOwner[])((OkObjectResult)await As("ali", "Engineer").GetOwners()).Value;
            Assert.Contains(owners, o => o.Username == "ali" && o.Title == "ali");
        }

        private class FakeUserService : IUserService
        {
            private readonly Dictionary<string, HashSet<string>> _roles = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

            public Dictionary<string, string> Passwords { get; } = new Dictionary<string, string>();

            public void Add(string username, string role) =>
                _roles[username] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { role };

            public Task<UserResponse> Create(UserCreate userCreate, string role)
            {
                if (_roles.ContainsKey(userCreate.Username))
                    throw new ConflictException("exists");
                Add(userCreate.Username, role);
                Passwords[userCreate.Username] = userCreate.Password;
                return Task.FromResult(new UserResponse { Username = userCreate.Username });
            }

            public Task<UserResponse> GetUserByUsername(string username) =>
                _roles.ContainsKey(username)
                    ? Task.FromResult(new UserResponse { Username = username })
                    : throw new NotFoundException("no user");

            public Task<IList<string>> GetUsernamesInRoleAsync(string role) =>
                Task.FromResult<IList<string>>(_roles.Where(kv => kv.Value.Contains(role)).Select(kv => kv.Key).ToList());

            public Task<bool> IsInRoleAsync(string username, string role) =>
                Task.FromResult(_roles.TryGetValue(username, out var r) && r.Contains(role));

            public Task AddToRoleAsync(string username, string role)
            {
                _roles[username].Add(role);
                return Task.CompletedTask;
            }

            public Task ResetPasswordAsync(string username, string newPassword)
            {
                Passwords[username] = newPassword;
                return Task.CompletedTask;
            }

            public Task DeleteByUsernameAsync(string username)
            {
                _roles.Remove(username);
                return Task.CompletedTask;
            }

            public Dictionary<(string, string), string> Claims { get; } = new Dictionary<(string, string), string>();

            public Task<string> GetClaimValueAsync(string username, string claimType) =>
                Task.FromResult(Claims.TryGetValue((username.ToLowerInvariant(), claimType), out var v) ? v : null);

            public Task SetClaimValueAsync(string username, string claimType, string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    Claims.Remove((username.ToLowerInvariant(), claimType));
                else
                    Claims[(username.ToLowerInvariant(), claimType)] = value.Trim();
                return Task.CompletedTask;
            }

            public Task<bool> AnyUserInRoleAsync(string role) => Task.FromResult(_roles.Values.Any(r => r.Contains(role)));
            public Task<UserResponse> Authenticate(string username, string password) => throw new NotImplementedException();
            public Task Delete(int id) => throw new NotImplementedException();
        }
    }
}
