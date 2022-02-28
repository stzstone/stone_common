package com.sundaytoz.plugins.common.utils;

import com.sundaytoz.plugins.common.enums.ENetworkType;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;

public class NetworkUtil
{
    /**
     * 현재 네트워크 상태 반환 
     * @param inContext
     * @return
     */
    public static ENetworkType getStatus(Context inContext)
    {
        ConnectivityManager cm = (ConnectivityManager) inContext.getSystemService(Context.CONNECTIVITY_SERVICE);

        NetworkInfo currentState = cm.getActiveNetworkInfo();
        ENetworkType networkType = ENetworkType.NOT_CONNECTED;
        
        if(currentState != null)
        {
        	final int type = currentState.getType();
        	
        	switch(type)
        	{
	        	case ConnectivityManager.TYPE_WIFI:
	        	{
	        		networkType = ENetworkType.WIFI;
	        		break;
	        	}
	        	
	        	case ConnectivityManager.TYPE_MOBILE:
	        	{
	        		networkType = ENetworkType.MOBILE;
	        		break;
	        	}
        	}
        } 
        
        return networkType;
    }
}
