using System.Text.Json.Serialization;

namespace Authentication.RefreshToken.Concerns.Common
{
    public class ErrorResponse
    {
        public bool IsSuccess => false;
        public string Message { get; set; } = null!;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<BaseError>? Errors { get; set; }
    }
}
