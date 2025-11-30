using Authentication.RefreshToken.Application.Dto.User;

namespace Authentication.RefreshToken.Application.Dto.Common
{
    public sealed record class AuditDto
    {
        public DateTime CreatedAt { get; set; }
        public UserSummaryDto? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserSummaryDto? UpdatedBy { get; set; }
        public DateTime? DeactivatedAt { get; set; }
        public UserSummaryDto? DeactivatedBy { get; set; }

    }
}
