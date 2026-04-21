using BMS.Core.Models;


namespace BMS.Core.Interfaces
{
    public interface IUserService
    {
        AppUser ValidateUser(string username, string password);
    }
}
