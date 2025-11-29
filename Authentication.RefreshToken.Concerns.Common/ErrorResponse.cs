using System.Text.Json.Serialization;

namespace Authentication.RefreshToken.Concerns.Common
{
    public class ErrorResponse : ApiResponse<object>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<BaseError>? Errors { get; set; }

        public ErrorResponse(string message, IEnumerable<BaseError>? errors = null)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
        }
    }
}
