using BMS.Core.Models;
using BMS.Core.Interfaces;

namespace BMS.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CloudBookStoreDbContext _context;

        public UserRepository(CloudBookStoreDbContext context)
        {
            _context = context;
        }

        public AppUser GetUserByName(string username)
        {
            return _context.AppUsers.FirstOrDefault(n => n.Username == username);
        }
    }
}
