namespace Authentication.RefreshToken.Concerns.Common
{
    public class SuccessResponse<T>
    {
        public T Data { get; set; } = default!;
        public bool IsSuccess  => true;
        public string Message { get; set; } =null!;
    }
}
