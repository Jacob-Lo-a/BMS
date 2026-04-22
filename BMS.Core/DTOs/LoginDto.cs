using BMS.Core.DTOs;

namespace BMS.API.DTOs
{
    public class LoginDto : BaseResponse
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
