package com.sundaytoz.plugins.common.utils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import android.app.PendingIntent;
import android.content.Context;

public class NotificationUtil
{
	public static String APP_PARAM = "";
	public static String KEY_ALARM_INFO = "alarm_info";
	public static String KEY_ALARM_ID = "alarm_id";
	public static String kET_PENDING_TYPE = "pending_type";
	
	/**
	 * 노티 정보 추가 
	 * @param context
	 * @param alarmId
	 * @param pendingType
	 * @return
	 */
	public static boolean addAlarmInfoToSharedPref(Context context, int alarmId, int pendingType)
	{ 
		List<Map<String,Integer>> alarmInfos = getAlaramInfoFromSharedPref(context);
		
		if(null == alarmInfos) 
			alarmInfos = new ArrayList<Map<String,Integer>>();
		
		Map<String,Integer> alarmInfo = new HashMap<String, Integer>();
		
		alarmInfo.put(KEY_ALARM_ID,alarmId);
		alarmInfo.put(kET_PENDING_TYPE,pendingType);
		
		alarmInfos.add(alarmInfo);
		
		return SharedUtil.getInstance(context, context.getApplicationInfo().className).putObject(KEY_ALARM_INFO, alarmInfos);
	}

	/**
	 * 저장된 노티 정보 조회 
	 * @param context 
	 * @return
	 */
	@SuppressWarnings("unchecked")
	public static List<Map<String,Integer>> getAlaramInfoFromSharedPref(Context context)
	{
		SharedUtil sUtil = SharedUtil.getInstance(context, context.getApplicationInfo().className);
		
		Object alarmInfos = (List<Map<String,Integer>>)sUtil.getObject(KEY_ALARM_INFO);
		
		if(null != alarmInfos)
			return (List<Map<String,Integer>>)alarmInfos;
		else
			return null;
	}
	
	/**
	 * 저장된 노티 초기화 
	 * 
	 * @param context
	 * @return
	 */
	public static boolean resetAlarmInfoFromSahredPref(Context context)
	{
		return SharedUtil.getInstance(context, context.getApplicationInfo().className).remove(KEY_ALARM_INFO);
	}
	
	/**
	 * 해당 노티 제거 
	 * @param context
	 * @param alarmId
	 * @param pendingType
	 */
	public static void removeAlarmInfoFromSahredPref(Context context, int alarmId, int pendingType)
	{ 
		List<Map<String,Integer>> alarmInfos = getAlaramInfoFromSharedPref(context);
		
		if(null != alarmInfos && alarmInfos.size() > 0)
		{ 
			Iterator<Map<String,Integer>> it = alarmInfos.iterator();
			
			while(it.hasNext())
			{
				Map<String,Integer> alarmInfo = it.next();
				
				if(alarmInfo.get(KEY_ALARM_ID) == alarmId && alarmInfo.get(kET_PENDING_TYPE) == pendingType)
				{
					it.remove();
				}
			}
			
			SharedUtil.getInstance(context, context.getApplicationInfo().className).putObject(KEY_ALARM_INFO, alarmInfos);
		}
	}
	
	/**
	 * 펜딩 타입 조회 
	 * @param notiType
	 * @return
	 */
	public static int toPendingType(int notiType)
	{ 
		switch(notiType)
		{ 
			case 1 : return PendingIntent.FLAG_CANCEL_CURRENT;
			case 2 : return PendingIntent.FLAG_NO_CREATE;
			case 3 : return PendingIntent.FLAG_ONE_SHOT;
			case 4 : return PendingIntent.FLAG_UPDATE_CURRENT;
		}
		
		return PendingIntent.FLAG_CANCEL_CURRENT;
	}
}
