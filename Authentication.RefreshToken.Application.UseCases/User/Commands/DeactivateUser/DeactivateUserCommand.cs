using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.DeactivateUser
{
    public sealed record class DeactivateUserCommand : IRequest<ApiResponse<object>>
    {
        public int Id { get; set; }
    }
}
