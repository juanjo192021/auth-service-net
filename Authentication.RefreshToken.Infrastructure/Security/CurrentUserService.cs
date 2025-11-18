using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Domain.Constants;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.RefreshToken.Infrastructure.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User =>
            _httpContextAccessor.HttpContext?.User;
        public int? UserId =>
            int.TryParse(User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var id)
            ? id
            : (int?)null;

        public List<string> Roles => 
            User?.FindAll(ClaimTypes.Role)?.Select(r => r.Value).ToList()
            ?? new List<string>();

        public string? GetClaim(string claimType) =>
            User?.FindFirst(claimType)?.Value;
    }
}
