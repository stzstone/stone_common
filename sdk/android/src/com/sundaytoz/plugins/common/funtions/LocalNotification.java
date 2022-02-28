package com.sundaytoz.plugins.common.funtions;

import java.util.Calendar;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.drawable.Drawable;
import android.util.Log;

import com.sundaytoz.plugins.common.receivers.NotificationReceiver;
import com.sundaytoz.plugins.common.utils.NotificationUtil;
import com.sundaytoz.plugins.common.utils.StorageUtil;

public class LocalNotification
{
	private static final String TAG = "LocalNotification";
	
	/**
	 * 로컬 푸시 메세지 등록 
	 * @param inActivity
	 * @param inAlarmId
	 * @param inTime
	 * @param inMessage
	 * @param inTitle
	 * @param inIcodId drawable.xxx
	 * @param inCounter
	 * @param inNotiType
	 */
	public static void add(Activity inActivity, int inAlarmId, int inTime, String inMessage, String inTitle, int inCounter, int inNotiType, String inBgImageName, String inTitleColor,  String inMsgColor, int titleSize, int msgSize)
	{
		try
		{
					Log.d(TAG, "LocalNotification::add()=>inAlarmId" + inAlarmId
			+ ", inTime : " +  inTime 
			+ ", inMessage : " + inMessage
			+ ", inTitle : " + inTitle
			+ ", inCounter : " + inCounter
			+ ", inNotiType : " + inNotiType );

			
			final int pendingType = NotificationUtil.toPendingType(inNotiType);
			
			AlarmManager alarmManager = (AlarmManager)inActivity.getSystemService(Context.ALARM_SERVICE);
			
			Calendar date = Calendar.getInstance();
			date.add(Calendar.SECOND, inTime);
			
			Intent intent = new Intent(inActivity, NotificationReceiver.class);
			intent.putExtra("id", inAlarmId);
			intent.putExtra("title", inTitle);
			intent.putExtra("message", inMessage);
			intent.putExtra("counter", inCounter);
			intent.putExtra("notiType", inNotiType);
			
			intent.putExtra("bg_image_name", 	inBgImageName);
			intent.putExtra("title_color", 		inTitleColor);
			intent.putExtra("msg_color", 		inMsgColor);
			intent.putExtra("titleSize", 		titleSize);
			intent.putExtra("msgSize", 			msgSize);
			StorageUtil.saveNotificationInfo(inActivity.getApplicationContext(), inTitleColor, inMsgColor, titleSize, msgSize);

			
			intent.putExtra("__class__", StorageUtil.getMainActivityName(inActivity.getApplicationContext()) );
			PendingIntent pending = PendingIntent.getBroadcast(inActivity, inAlarmId, intent, pendingType);
	
			// 알람 정보 저장 
			NotificationUtil.addAlarmInfoToSharedPref(inActivity, inAlarmId, pendingType);
			
			// 알람 세팅
			alarmManager.set(AlarmManager.RTC_WAKEUP, date.getTimeInMillis(), pending);
			
			Log.d(TAG, "Added intent=" + intent.toString());
		}
		catch (Exception ex)
		{
			Log.d(TAG, "Failed notification adding!");
			ex.printStackTrace();
		}
	}

	/**
	 * 로컬 푸시 메세지 취소 
	 * @param inActivity
	 * @param inAlarmId
	 * @param inNotiType
	 */
	public static void cancel(Activity inActivity, int inAlarmId, int inNotiType)
	{
		try
		{ 
			int pendingFlag = NotificationUtil.toPendingType(inNotiType);
			
			AlarmManager alarmManager = (AlarmManager)inActivity.getSystemService(Context.ALARM_SERVICE);
			
			Intent intent = new Intent(inActivity, NotificationReceiver.class );
			
			PendingIntent pending = PendingIntent.getBroadcast(inActivity, inAlarmId, intent, pendingFlag);
			
			alarmManager.cancel(pending);
			
			//  저장된 노티 정보 제거 
			NotificationUtil.removeAlarmInfoFromSahredPref(inActivity, inAlarmId, pendingFlag);
			
			Log.d(TAG, "remove alarm_id=" + inAlarmId );
		}
		catch(Exception ex)
		{
			ex.printStackTrace();
		}
	}
	
	/**
	 * 등록된 전체 푸시 메세지 취소 
	 * @param inActivity
	 */
	public static void cancelAll(Activity inActivity)
	{
		try
		{
			AlarmManager alarmManager = (AlarmManager)inActivity.getSystemService(Context.ALARM_SERVICE);
			
			Intent intent = new Intent(inActivity, NotificationReceiver.class );
			
			// 저장된 알람 정보 조회 
			List<Map<String,Integer>> alarmInfos = NotificationUtil.getAlaramInfoFromSharedPref(inActivity);
			
			if(null != alarmInfos)
			{ 
				Iterator<Map<String,Integer>> it = alarmInfos.iterator();
				
				while(it.hasNext())
				{
					Map<String,Integer> alarmInfo = it.next();
					
					PendingIntent pending = PendingIntent.getBroadcast(inActivity, alarmInfo.get(NotificationUtil.KEY_ALARM_ID), intent, alarmInfo.get(NotificationUtil.kET_PENDING_TYPE));
					alarmManager.cancel(pending);
				}
			}

			// 알람 제거 
			NotificationUtil.resetAlarmInfoFromSahredPref(inActivity);

			// 푸시 서버스 제거 
			android.app.NotificationManager notificationManager =  (android.app.NotificationManager)inActivity.getSystemService(android.content.Context.NOTIFICATION_SERVICE);
    		notificationManager.cancelAll();
    		
    		Log.d(TAG, "Cancel all notification(s)!");
    	}
		catch(Exception ex)
		{
			ex.printStackTrace();
    	}
	}
}
