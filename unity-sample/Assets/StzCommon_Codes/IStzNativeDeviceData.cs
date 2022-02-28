namespace StzNativeExtention
{
    public interface IDeviceData
    {
        string  DeviceName       { get; } 
        string  Carrier          { get; } 
        string  SimState         { get; } 
        string  LocalCountry     { get; } 
        string  NetworkCountry   { get; } 
        string  Country          { get; } 
        string  OsVersion        { get; } 
        string  AppParams        { get; } 
        string  Timezone         { get; } 
        string  InstallReferrer   { get; } 
        string  Timeoffset       { get; } 
        string  VersionName      { get; } 
        string  UUID             { get; } 
        int     FreeSpaceMb      { get; } 
        int     ApiLevel         { get; } 

        EInAppUpdate InAppUpdateStatus { get; }
    }           
}
