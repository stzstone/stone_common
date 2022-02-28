package com.sundaytoz.plugins.common.utils;

import java.util.HashMap;
import java.util.Map;

import com.sundaytoz.plugins.common.SundaytozAndroid;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

public class StorageUtil
{
	
	private static final String PREF_KEY = "__STZ_COMMON__";
	private static final String PREF_KEY_NOTI = "__STZ_COMMON_NOTI__";

	public static void saveMainActivityName(Context context,String mainActivityName)
	{
 		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
	   	SharedPreferences.Editor ed = myPrefs.edit();
	   	ed.putString(SundaytozAndroid.Action.Param.main_activity_name,mainActivityName);
	   	ed.apply();
	}
	 
	public static String getMainActivityName(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		return myPrefs.getString(SundaytozAndroid.Action.Param.main_activity_name, "Empty");
	}
	
	public static String getInstallReferer(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		
		if( myPrefs.contains ( SundaytozAndroid.Action.Param.install_referrer) ){
			return myPrefs.getString(SundaytozAndroid.Action.Param.install_referrer, "Empty");
		}else{
			return "Empty";
		}
	}

	public static void saveInstallReferer(Context context,String referrer)
	{
		Log.d("STZCommon", "saveInstallReferer() referrer: " + referrer);
		if(referrer==null || referrer.isEmpty() ){
			return;
		}
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		SharedPreferences.Editor ed = myPrefs.edit();
		ed.putString(SundaytozAndroid.Action.Param.install_referrer, referrer );
		ed.apply();
	}
	
	public static void clearInstallReferer(Context context)
	{
		clearData(context,SundaytozAndroid.Action.Param.install_referrer);
	}

	public static void saveNotificationInfo(Context context,
			String inTitleColor,
			String inMsgColor,
			int inTitleSize,
			int inMsgSize
			)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
	   	SharedPreferences.Editor ed = myPrefs.edit();
	   	ed.putString("title_color", 	inTitleColor);
	   	ed.putString("msg_color", 		inMsgColor);
	   	ed.putString("titleSize", 		String.valueOf(inTitleSize));
	   	ed.putString("msgSize", 		String.valueOf(inMsgSize));		
	   	ed.apply();		
		
	}
	
	public static Map<String,String> getNotificationInfo(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		
		Map<String,String> result = new HashMap<String, String>(); 
		result.put("title_color", 	myPrefs.getString("title_color", 	""));
		result.put("msg_color", 	myPrefs.getString("msg_color", 		""));
		result.put("titleSize", 	myPrefs.getString("titleSize", 		""));
		result.put("msgSize", 		myPrefs.getString("msgSize", 		""));			
		
		return result;
	}

	public static void saveIntentData(Context context, String intentData)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		SharedPreferences.Editor ed = myPrefs.edit();
		ed.putString("scheme_data", intentData);
		ed.apply();
	}

	
	public static String getIntentData(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);

		return myPrefs.getString("scheme_data","Empty");
	}

	public static void saveDeeplinkData(Context context, String deeplinkData)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		SharedPreferences.Editor ed = myPrefs.edit();
		ed.putString("deeplink_data", deeplinkData);
		ed.apply();
	}

	
	public static String getDeeplinkData(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);

		return myPrefs.getString("deeplink_data","");
	}

	public static void saveReservedWord(Context context, String key, String value)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY_NOTI, Context.MODE_PRIVATE);
		SharedPreferences.Editor ed = myPrefs.edit();
		ed.putString(key, value);
		ed.apply();
	}

	public static Map<String, ?> getlAllReservedWord(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY_NOTI, Context.MODE_PRIVATE);
		return myPrefs.getAll();
	}

	/**
	 * @param context
	 * @return
	 */
	public static Map<String,?> getAllData(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);
		return myPrefs.getAll();
	}
	
	/**
	 * @param context
	 */
	public static void clearData(Context context,String key)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);

		SharedPreferences.Editor ed = myPrefs.edit();
		ed.remove(key);
    	ed.apply();
	}
	
	public static void clearDataAll(Context context)
	{
		SharedPreferences myPrefs = context.getSharedPreferences(PREF_KEY, Context.MODE_PRIVATE);

		SharedPreferences.Editor ed = myPrefs.edit();
		ed.clear();		
    	ed.apply();
	}
}
