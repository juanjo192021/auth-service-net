using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Concerns.Common;
using Authentication.RefreshToken.Domain.Entities;
using Authentication.RefreshToken.Infrastructure.Security.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.RefreshToken.Infrastructure.Security
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        private readonly TokenValidationParameters _defaultValidationParameters;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            _defaultValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

        }

        // Genera un token JWT para el usuario
        public string GenerateToken(int id, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            // Agregar roles
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role,role));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_settings.TokenExpiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                NotBefore = DateTime.UtcNow,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GetJwtId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var jtiClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            return jtiClaim?.Value ?? string.Empty;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            // Configura los parámetros de validación para ignorar la expiración
            var parameters = _defaultValidationParameters.Clone();
            parameters.ValidateLifetime = false;

            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, parameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwt ||
                    !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public int? GetUserIdFromExpiredToken(string token)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
                return null;

            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value 
                ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal.FindFirst("sub")?.Value;

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }

        public TokenStatus ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, _defaultValidationParameters, out SecurityToken validatedToken);
                // Token válido
                return TokenStatus.Valid;
            }
            catch (SecurityTokenExpiredException)
            {
                return TokenStatus.Expired;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return TokenStatus.InvalidSignature;
            }
            catch (ArgumentException)
            {
                return TokenStatus.InvalidFormat;
            }
            catch (Exception)
            {
                return TokenStatus.Corrupt;
            }
        }
    }
}
