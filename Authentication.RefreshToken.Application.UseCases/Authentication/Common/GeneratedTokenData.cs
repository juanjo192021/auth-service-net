namespace Authentication.RefreshToken.Application.UseCases.Authentication.Common
{
    internal record GeneratedTokenData(
        string Token,
        string RefreshToken,
        string JwtId,
        string RefreshTokenHash
    );
}
