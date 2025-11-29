using System.Text.Json.Serialization;

namespace Authentication.RefreshToken.Concerns.Common
{
    public class ApiResponse<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Type => Succeeded ? "success" : "error";
        public ApiResponse() { }

        public ApiResponse(T data, string message)
        {
            Succeeded = true;
            Message = message ?? string.Empty;
            Data = data;
        }
    }
}
