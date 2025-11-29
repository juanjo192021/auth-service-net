using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Queries.GetAllUser
{
    public sealed record class GetAllUserQuery : IRequest<PagedResponse<IEnumerable<UserDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; } = string.Empty;
    }
}
