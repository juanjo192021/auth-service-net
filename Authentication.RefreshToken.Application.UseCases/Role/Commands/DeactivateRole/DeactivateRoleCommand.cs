using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.DeactivateRole
{
    public sealed record class DeactivateRoleCommand : IRequest<SuccessResponse<bool>>
    {
        public int Id { get; set; }
    }
}
