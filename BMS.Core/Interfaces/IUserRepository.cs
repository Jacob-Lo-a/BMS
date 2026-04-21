using BMS.Core.Models;


namespace BMS.Core.Interfaces
{
    public interface IUserRepository
    {
        AppUser GetUserByName(string username);
    }
}
