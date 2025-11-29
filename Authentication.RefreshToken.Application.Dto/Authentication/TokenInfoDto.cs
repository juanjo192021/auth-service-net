using System.Text.Json.Serialization;

namespace Authentication.RefreshToken.Application.Dto.Authentication
{
    public sealed record class TokenInfoDto
    {
        public string AccessToken { get; set; } = null!;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? RefreshToken { get; set; }
    }
}
