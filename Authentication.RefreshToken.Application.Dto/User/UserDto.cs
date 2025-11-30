using Authentication.RefreshToken.Application.Dto.Common;
using Authentication.RefreshToken.Application.Dto.Role;

namespace Authentication.RefreshToken.Application.Dto.User
{
    public record class UserDto : UserBaseDto
    {
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public IEnumerable<RoleSummaryDto> Roles { get; set; } =null!;
        public AuditDto Audit { get; set; } = null!;
    }
}
