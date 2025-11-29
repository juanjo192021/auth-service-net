namespace Authentication.RefreshToken.Application.Dto.Account
{
    public class AccountDto
    {
        //public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
