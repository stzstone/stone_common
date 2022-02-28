package com.sundaytoz.plugins.common.utils;

import android.util.Log;

public class STZLog
{
	private static String TAG = "stzcommon.plugin";
	
	public static Boolean enabled = false;
	
	/**
	 * 디버깅용 
	 * @param inMessage
	 */
	public static void d(String inMessage)
	{
		if(enabled)
		{
			Log.d(TAG, inMessage);
		}
	}
	
	/**
	 * 정보 
	 * @param inMessage
	 */
	public static void i(String inMessage)
	{
		Log.i(TAG, inMessage);
	}
	
	/**
	 * 오류 
	 * @param inMessage
	 */
	public static void e(String inMessage)
	{
		Log.e(TAG, inMessage);
	}
}
