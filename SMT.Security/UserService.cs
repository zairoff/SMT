using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SMT.Common.Dto.UserDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using System;
using System.Threading.Tasks;

namespace SMT.Security
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IOptions<AppSettings> appSettings, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<UserResponse> Create(UserCreate userCreate)
        {
            var user = await _userManager.FindByNameAsync(userCreate.Username);

            if (user != null)
                throw new ConflictException();

            user = _mapper.Map<UserCreate, User>(userCreate);

            var result = await _userManager.CreateAsync(user, userCreate.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException();

            if (!await _roleManager.RoleExistsAsync(userCreate.Role))
                await _roleManager.CreateAsync(new IdentityRole(userCreate.Role));

            await _userManager.AddToRoleAsync(user, userCreate.Role);

            return _mapper.Map<User, UserResponse>(user);
        }

        public async Task<string> Authenticate(string username, string password)
        {
            //var user = await _repository.Get()
            //                            .Where(u => u.Email == username && u.PasswordHash == password)
            //                            .FirstOrDefaultAsync();

            //if (user == null)
            //    throw new NotFoundException();

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, username),
            //        new Claim(ClaimTypes.Role, user.Role.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddMinutes(50),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);    
            //return tokenHandler.WriteToken(token);
            return null;
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
