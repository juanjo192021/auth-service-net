using Authentication.RefreshToken.Application.Dto.User;

namespace Authentication.RefreshToken.Application.Dto.Auth
{
    public class AuthDto
    {
        public TokenInfoDto Tokens { get; set; }
        public UserDto User { get; set; }
    }
}
