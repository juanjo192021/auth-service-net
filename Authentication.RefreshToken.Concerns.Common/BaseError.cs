namespace Authentication.RefreshToken.Concerns.Common
{
    public class BaseError
    {
        public string? PropertyMessage { get; set; }
        public List<string>? ErrorMessage { get; set; }
    }
}
