namespace Authentication.RefreshToken.Concerns.Common
{
    public enum TokenStatus
    {
        Valid,
        Expired,
        InvalidFormat,
        InvalidSignature,
        Corrupt
    }
}
