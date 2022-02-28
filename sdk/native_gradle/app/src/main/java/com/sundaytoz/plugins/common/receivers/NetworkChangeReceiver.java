package com.sundaytoz.plugins.common.receivers;

import com.sundaytoz.plugins.common.SundaytozAndroid;
import com.sundaytoz.plugins.common.enums.ENetworkType;
import com.sundaytoz.plugins.common.utils.NetworkUtil;
import com.sundaytoz.plugins.common.utils.STZLog;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class NetworkChangeReceiver extends BroadcastReceiver
{
    @Override
    public void onReceive(final Context inContext, final Intent inIntent)
    {
        if(!("android.intent.action.BOOT_COMPLETED").equals(inIntent.getAction()))
        {
            ENetworkType type = NetworkUtil.getStatus(inContext);
            
            STZLog.i("Changed network status: " + type.getTypeName());
            
            if(SundaytozAndroid.instance != null)
            {
            	SundaytozAndroid.instance.changedNetworkStatus(type);
            }
       }
    }
}
