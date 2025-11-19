namespace Authentication.RefreshToken.Application.UseCases.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
