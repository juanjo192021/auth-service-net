namespace Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        List<string> Roles { get; }
        string? GetClaim(string claimType);
    }
}
