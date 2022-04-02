using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMT.Access.Identity;
using SMT.ViewModel.Dto.UserDto;
using SMT.ViewModel.Exceptions;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

namespace SMT.Security
{
    public class UserService : IUserService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
                            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<UserResponse> Create(UserCreate userCreate, string role)
        {
            var user = await _userManager.FindByNameAsync(userCreate.Username);

            if (user != null)
                throw new ConflictException($"{userCreate.Username} already registered");

            var applicationUser = new ApplicationUser()
            {
                UserName = userCreate.Username,
                Telegram = userCreate.Telegram
            };

            var result = await _userManager.CreateAsync(applicationUser, userCreate.Password);
            var errors = new StringBuilder();
            result.Errors.ToList().ForEach(e => errors.Append(e.Description));

            if (!result.Succeeded)
                throw new InvalidOperationException(errors.ToString());

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(applicationUser, role);

            return (UserResponse)applicationUser;
        }

        public async Task<UserResponse> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new NotFoundException($"{username} user not found");

            var userResponse = new UserResponse()
            {
                Username = username
            };

            return userResponse;
        }

        public async Task<UserResponse> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new InvalidOperationException($"username or password is incorrect");

            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Secret"));
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            claims.Add(new Claim("id", user.Id));
            claims.Add(new Claim("username", user.UserName));

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddSeconds(10),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

            var userResponse = new UserResponse()
            {
                Username = username,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return userResponse;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
