using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Queries.GetUser
{
    public sealed record class GetUserQuery : IRequest<ApiResponse<UserDto>>
    {
        public int Id { get; set; }
    }
}
