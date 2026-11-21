using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SMT.Access.Identity;
using SMT.Security;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.Documents;
using SMT.ViewModel.Dto.DocumentDto;
using SMT.ViewModel.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    /// <summary>
    /// Staff document folders. Every engineer has one folder:
    ///  - the owner can upload, create folders, rename and delete inside it;
    ///  - other engineers can only browse and download;
    ///  - admins can do everything in every folder and manage the staff accounts.
    /// Files live on disk under Documents:RootFolder/{username}/... - back that folder up.
    /// </summary>
    [Authorize]
    public class DocumentController : BaseController
    {
        private const long DefaultMaxFileSizeMb = 500;

        /// <summary>Identity claim holding a folder's display name ("1 Метрология").</summary>
        public const string TitleClaim = "documents:title";

        private readonly IDocumentStorage _storage;
        private readonly IUserService _userService;
        private readonly long _maxFileBytes;

        public DocumentController(IDocumentStorage storage, IUserService userService, IConfiguration configuration)
        {
            _storage = storage;
            _userService = userService;
            var maxMb = configuration.GetValue<long?>("Documents:MaxFileSizeMb") ?? DefaultMaxFileSizeMb;
            _maxFileBytes = maxMb * 1024 * 1024;
        }

        /********************  Folders & files  ********************/

        /// <summary>Everyone who has a folder, for the folder picker.</summary>
        [HttpGet("owners")]
        public async Task<IActionResult> GetOwners()
        {
            if (!CanView)
                return Denied("You don't have access to staff documents");

            return Ok(await LoadOwnersAsync());
        }

        [HttpGet("list")]
        public async Task<IActionResult> List(string owner, string path)
        {
            if (!CanView)
                return Denied("You don't have access to staff documents");
            await EnsureOwnerAsync(owner);

            var listing = _storage.List(owner, path);
            listing.CanEdit = CanEdit(owner);
            return Ok(listing);
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download(string owner, string path)
        {
            if (!CanView)
                return Denied("You don't have access to staff documents");
            await EnsureOwnerAsync(owner);

            var stream = _storage.OpenRead(owner, path, out var fileName);
            // Always an attachment: never let an uploaded .html/.svg render in the app's origin.
            return File(stream, "application/octet-stream", fileName);
        }

        /// <summary>Multipart upload of one file (form field "file") into {path}.</summary>
        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Upload([FromQuery] string owner, [FromQuery] string path, [FromQuery] bool overwrite = false)
        {
            if (!CanEdit(owner))
                return Denied("You can only upload to your own folder");
            await EnsureOwnerAsync(owner);

            // Reject oversize uploads from the header, before the body is read.
            var limit = _maxFileBytes + 1024 * 1024; // + multipart overhead
            if (Request.ContentLength > limit)
                return TooLarge();
            var sizeFeature = HttpContext.Features.Get<IHttpMaxRequestBodySizeFeature>();
            if (sizeFeature != null && !sizeFeature.IsReadOnly)
                sizeFeature.MaxRequestBodySize = limit;

            var form = await Request.ReadFormAsync(HttpContext.RequestAborted);
            var file = form.Files.GetFile("file");
            if (file == null)
                throw new InvalidOperationException("No file was sent");
            if (file.Length > _maxFileBytes)
                return TooLarge();

            using var content = file.OpenReadStream();
            var entry = await _storage.SaveAsync(owner, path, file.FileName, content, overwrite);
            return Ok(entry);
        }

        [HttpPost("folder")]
        public async Task<IActionResult> CreateFolder([FromBody] DocumentCreateFolder request)
        {
            if (!CanEdit(request.Owner))
                return Denied("You can only create folders in your own folder");
            await EnsureOwnerAsync(request.Owner);

            return Ok(_storage.CreateFolder(request.Owner, request.Path, request.Name));
        }

        [HttpPost("rename")]
        public async Task<IActionResult> Rename([FromBody] DocumentRename request)
        {
            if (!CanEdit(request.Owner))
                return Denied("You can only rename in your own folder");
            await EnsureOwnerAsync(request.Owner);

            return Ok(_storage.Rename(request.Owner, request.Path, request.NewName));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string owner, string path)
        {
            if (!CanEdit(owner))
                return Denied("You can only delete in your own folder");
            await EnsureOwnerAsync(owner);

            _storage.Delete(owner, path);
            return NoContent();
        }

        /********************  Staff accounts (admin)  ********************/

        [HttpGet("staff")]
        public async Task<IActionResult> GetStaff()
        {
            if (!IsAdmin)
                return Denied("Only an admin can manage staff accounts");

            return Ok(await LoadOwnersAsync());
        }

        [HttpPost("staff")]
        public async Task<IActionResult> CreateStaff([FromBody] DocumentStaffCreate request)
        {
            if (!IsAdmin)
                return Denied("Only an admin can manage staff accounts");

            var username = (request.Username ?? "").Trim();
            // The username is also the folder name on disk.
            _storage.ValidateName(username);

            if (await _userService.IsInRoleAsync(username, ApplicationUserRoles.Engineer))
                throw new ConflictException($"{username} already has a folder");

            bool existingAccount;
            try
            {
                await _userService.GetUserByUsername(username);
                existingAccount = true;
            }
            catch (NotFoundException)
            {
                existingAccount = false;
            }

            if (existingAccount)
            {
                // e.g. the admin giving themselves a folder: keep the login and its password as they are.
                await _userService.AddToRoleAsync(username, ApplicationUserRoles.Engineer);
            }
            else
            {
                if (string.IsNullOrEmpty(request.Password))
                    throw new InvalidOperationException("Password is required");
                await _userService.Create(new UserCreate { Username = username, Password = request.Password }, ApplicationUserRoles.Engineer);
            }

            if (request.IsAdmin)
                await _userService.AddToRoleAsync(username, ApplicationUserRoles.Admin);
            if (!string.IsNullOrWhiteSpace(request.Title))
                await _userService.SetClaimValueAsync(username, TitleClaim, ValidateTitle(request.Title));

            return Ok(new
            {
                username,
                title = string.IsNullOrWhiteSpace(request.Title) ? username : request.Title.Trim(),
                isAdmin = request.IsAdmin || await _userService.IsInRoleAsync(username, ApplicationUserRoles.Admin),
                existingAccount,
            });
        }

        /// <summary>Changes the folder's display name (the login and the folder on disk stay the same).</summary>
        [HttpPut("staff/title")]
        public async Task<IActionResult> SetTitle([FromQuery] string username, [FromBody] DocumentTitleUpdate request)
        {
            if (!IsAdmin)
                return Denied("Only an admin can manage staff accounts");
            await EnsureOwnerAsync(username);

            var title = string.IsNullOrWhiteSpace(request?.Title) ? null : ValidateTitle(request.Title);
            await _userService.SetClaimValueAsync(username, TitleClaim, title);
            return NoContent();
        }

        private static string ValidateTitle(string title)
        {
            title = title.Trim();
            if (title.Length > 100)
                throw new InvalidOperationException("Folder name is too long (100 characters max)");
            return title;
        }

        [HttpPut("staff/password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string username, [FromBody] DocumentPasswordReset request)
        {
            if (!IsAdmin)
                return Denied("Only an admin can manage staff accounts");
            await EnsureOwnerAsync(username);

            await _userService.ResetPasswordAsync(username, request.Password);
            return NoContent();
        }

        /// <summary>Removes the login. The person's files stay on disk (and stay visible to admins on the server).</summary>
        [HttpDelete("staff")]
        public async Task<IActionResult> DeleteStaff([FromQuery] string username)
        {
            if (!IsAdmin)
                return Denied("Only an admin can manage staff accounts");
            if (string.Equals(username, CurrentUsername, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("You cannot delete your own account");
            await EnsureOwnerAsync(username);

            await _userService.DeleteByUsernameAsync(username);
            return NoContent();
        }

        /********************  Helpers  ********************/

        private string CurrentUsername => User.FindFirst("username")?.Value ?? User.Identity?.Name;

        // Role names have been created both as "admin" and "Admin" over time; compare case-insensitively.
        private bool HasRole(string role) =>
            User.Claims.Any(c => (c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role") &&
                                 string.Equals(c.Value, role, StringComparison.OrdinalIgnoreCase));

        private bool IsAdmin => HasRole(ApplicationUserRoles.Admin);

        private bool CanView => IsAdmin || HasRole(ApplicationUserRoles.Engineer);

        private bool CanEdit(string owner) =>
            IsAdmin ||
            (HasRole(ApplicationUserRoles.Engineer) &&
             string.Equals(owner, CurrentUsername, StringComparison.OrdinalIgnoreCase));

        private async Task EnsureOwnerAsync(string owner)
        {
            if (string.IsNullOrWhiteSpace(owner) || !await _userService.IsInRoleAsync(owner, ApplicationUserRoles.Engineer))
                throw new NotFoundException($"No staff folder for \"{owner}\"");
        }

        private async Task<DocumentFolderOwner[]> LoadOwnersAsync()
        {
            var engineers = await _userService.GetUsernamesInRoleAsync(ApplicationUserRoles.Engineer);
            var admins = (await _userService.GetUsernamesInRoleAsync(ApplicationUserRoles.Admin))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var owners = new List<DocumentFolderOwner>();
            foreach (var username in engineers)
            {
                var title = await _userService.GetClaimValueAsync(username, TitleClaim);
                owners.Add(new DocumentFolderOwner
                {
                    Username = username,
                    Title = string.IsNullOrWhiteSpace(title) ? username : title,
                    IsAdmin = admins.Contains(username),
                });
            }
            return SortByStep(owners).ToArray();
        }

        /// <summary>
        /// Folder titles start with the process step ("1 Метрология", "10 Менеджер СМК"): order by
        /// that number (1, 2 ... 10, 11 - not 1, 10, 11, 2), untitled/unnumbered folders last.
        /// </summary>
        public static IEnumerable<DocumentFolderOwner> SortByStep(IEnumerable<DocumentFolderOwner> owners)
        {
            static int Step(string title)
            {
                var digits = new string((title ?? "").TakeWhile(char.IsDigit).ToArray());
                return digits.Length > 0 && digits.Length < 9 ? int.Parse(digits) : int.MaxValue;
            }

            return owners
                .OrderBy(o => Step(o.Title))
                .ThenBy(o => o.Title, StringComparer.CurrentCultureIgnoreCase);
        }

        private IActionResult Denied(string message) =>
            StatusCode(StatusCodes.Status403Forbidden, new { code = StatusCodes.Status403Forbidden, message });

        private IActionResult TooLarge() =>
            StatusCode(StatusCodes.Status413PayloadTooLarge, new
            {
                code = StatusCodes.Status413PayloadTooLarge,
                message = $"File is larger than the {_maxFileBytes / 1024 / 1024} MB limit",
                maxMb = _maxFileBytes / 1024 / 1024,
            });
    }
}
