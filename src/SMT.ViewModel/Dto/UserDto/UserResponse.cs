using SMT.Access.Identity;

namespace SMT.ViewModel.Dto.UserDto
{
    public class UserResponse
    {
        public string Username { get; set; }

        public string Token { get; set; }

        public static explicit operator UserResponse(ApplicationUser user)
        {
            return new UserResponse
            {
                Username = user.UserName
            };
        }
    }
}
