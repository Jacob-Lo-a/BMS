using BMS.Core.Models;
using BMS.Core.Interfaces;

namespace BMS.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public AppUser ValidateUser(string username, string password)
        {
            var user = _userRepository.GetUserByName(username);

            if (user == null || user.PasswordHash != password)
            {
                return null;
            }

            return user;
        }
    }
}
