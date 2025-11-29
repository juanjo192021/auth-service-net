using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser
{
    public sealed record class CreateUserCommand : IRequest<ApiResponse<UserSummaryDto>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsBlocked { get; set; }
        public List<int>? Roles { get; set; }
    }
}
