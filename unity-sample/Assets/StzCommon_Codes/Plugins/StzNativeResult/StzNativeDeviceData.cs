using StzNativeExtention;
using System;
using UnityEngine;

[Serializable]
public sealed class StzNativeDeviceData : StzNativeResult_Base, IDeviceData
{
    [Serializable]
    private class Data
    {
        public string   device_name          = string.Empty;
        public string   carrier              = string.Empty;
        public string   sim_state            = string.Empty;
        public string   local_country        = string.Empty;
        public string   network_country      = string.Empty;
        public string   country              = string.Empty;
        public string   os_version           = string.Empty;
        public string   app_params           = string.Empty;
        public string   timezone             = string.Empty;
        public string   install_referrer     = string.Empty;
        public string   timeoffset           = string.Empty;
        public string   version_name         = string.Empty;
        public string   uuid                 = string.Empty;
        public int      free_space_mb        = 0;
        public int      api_level            = 0;

        public bool     update_available     = false;
        public bool     update_flexible      = false;
        public bool     update_immediate     = false;
    }
    
    //----------------------------------------------------------------
    // variables
    //----------------------------------------------------------------
    [SerializeField] private Data data = null;

    //----------------------------------------------------------------
    // get, set
    //----------------------------------------------------------------
    public string          DeviceName        { get { return StzNativeResultStringUtil.ParseDataString(data?.device_name); } }
    public string          Carrier           { get { return StzNativeResultStringUtil.ParseDataString(data?.carrier); } }
    public string          SimState          { get { return StzNativeResultStringUtil.ParseDataString(data?.sim_state); } }
    public string          LocalCountry      { get { return StzNativeResultStringUtil.ParseDataString(data?.local_country); } }
    public string          NetworkCountry    { get { return StzNativeResultStringUtil.ParseDataString(data?.network_country); } }
    public string          Country           { get { return StzNativeResultStringUtil.ParseDataString(data?.country); } }
    public string          OsVersion         { get { return StzNativeResultStringUtil.ParseDataString(data?.os_version); } }
    public string          AppParams         { get { return StzNativeResultStringUtil.ParseDataString(data?.app_params); } }
    public string          Timezone          { get { return StzNativeResultStringUtil.ParseDataString(data?.timezone); } }
    public string          InstallReferrer   { get { return StzNativeResultStringUtil.ParseDataString(data?.install_referrer); } }
    public string          Timeoffset        { get { return StzNativeResultStringUtil.ParseDataString(data?.timeoffset); } }
    public string          VersionName       { get { return StzNativeResultStringUtil.ParseDataString(data?.version_name); } }
    public string          UUID              { get { return StzNativeResultStringUtil.ParseDataString(data?.uuid); } }
    public int             FreeSpaceMb       { get { return data?.free_space_mb ?? 0; } }
    public int             ApiLevel          { get { return data?.api_level ?? 0; } }

    public EInAppUpdate InAppUpdateStatus
    {
        get
        {
            if (isInAppUpdateDownloaded)
                return EInAppUpdate.Downloaded;
            
            return data?.update_available == true ? EInAppUpdate.Available : EInAppUpdate.NotAvailable;
        }
    }

    private bool isInAppUpdateDownloaded;
    
    //----------------------------------------------------------------
    // functions
    //----------------------------------------------------------------
    public void SetFreeSpaceMb(int size)
    {
        if (null == data)
            return;

        data.free_space_mb = size;
    }

    public void InAppUpdateDownloaded()
    {
        if (null == data)
            return;

        isInAppUpdateDownloaded = true;
    }

    public void SetUUID(string uuid)
    {
        if (null == data)
            return;
        
        data.uuid = uuid;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, {nameof(data)}: {data}, {nameof(isInAppUpdateDownloaded)}: {isInAppUpdateDownloaded}, {nameof(DeviceName)}: {DeviceName}, {nameof(Carrier)}: {Carrier}, {nameof(SimState)}: {SimState}, {nameof(LocalCountry)}: {LocalCountry}, {nameof(NetworkCountry)}: {NetworkCountry}, {nameof(Country)}: {Country}, {nameof(OsVersion)}: {OsVersion}, {nameof(AppParams)}: {AppParams}, {nameof(Timezone)}: {Timezone}, {nameof(InstallReferrer)}: {InstallReferrer}, {nameof(Timeoffset)}: {Timeoffset}, {nameof(VersionName)}: {VersionName}, {nameof(UUID)}: {UUID}, {nameof(FreeSpaceMb)}: {FreeSpaceMb}, {nameof(ApiLevel)}: {ApiLevel}, {nameof(InAppUpdateStatus)}: {InAppUpdateStatus}";
    }
}