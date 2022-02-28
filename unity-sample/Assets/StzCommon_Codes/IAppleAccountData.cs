namespace StzNativeExtention
{
    public interface IAppleAccountData
    {
        string StatusDetailMessage   { get; }
        string Id                    { get; }
        string Email                 { get; }
        string UserFullName          { get; }

        EAppleAccountStatus Status { get; }
    }
}
