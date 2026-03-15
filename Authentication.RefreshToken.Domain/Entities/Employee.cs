using Authentication.RefreshToken.Domain.Common;

namespace Authentication.RefreshToken.Domain.Entities
{
    public class Employee: BaseAuditableEntity
    {
        public int Id { get; set; }
        public string? DocumentType { get; set; }
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Mobile { get; set; }
        public string? Gender { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? Department { get; set; }
        public string? JobTitle { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? UserId { get; set; }
        public int? ManagerId { get; set; }
        public virtual User? User { get; set; }
        public virtual Employee? Manager { get; set; }
    }
}
