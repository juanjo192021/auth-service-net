using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register
{
    public sealed record class RegisterCommand : IRequest<ApiResponse<TokenInfoDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
