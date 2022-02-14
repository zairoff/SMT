using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SMT.Access.Identity;
using SMT.ViewModel.Dto.UserDto;
using SMT.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<UserResponse> Create(UserCreate userCreate)
        {
            var user = await _userManager.FindByNameAsync(userCreate.Username);

            if (user != null)
                throw new ConflictException();

            var result = await _userManager.CreateAsync(user, userCreate.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException();

            if (!await _roleManager.RoleExistsAsync(userCreate.Role))
                await _roleManager.CreateAsync(new IdentityRole(userCreate.Role));

            await _userManager.AddToRoleAsync(user, userCreate.Role);

            return (UserResponse)user;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                throw new NotFoundException();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("AppSettings:Secret"));
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<UserResponse> Update(int id, UserUpdate userUpdate)
        {
            //var user = await _repository.Get()
            //                            .Where(u => u.Id == id)
            //                            .FirstOrDefaultAsync();

            //if (user == null)
            //    throw new NotFoundException();

            //user.Password = userUpdate.Password;
            //user.Role = userUpdate.Role;
            //user.Username = userUpdate.Username;
            return null;
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
