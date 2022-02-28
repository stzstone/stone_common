package com.sundaytoz.plugins.common.receivers;

import com.sundaytoz.plugins.common.receivers.StzNotificationTask;
import com.sundaytoz.plugins.common.utils.StorageUtil;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class NotificationReceiver extends BroadcastReceiver
{
	private static final String LOG_TAG = "Sundaytoz";
	private static final int EXPANDED_NOTI = 1000;

	public static NotificationReceiver instance = null;
	
	/**
	 * 초기화
	 * Unity측에서 호출하기 됨 
	 */
	public static String createInstance()
	{
		if(instance == null)
		{
			instance = new NotificationReceiver();
		}
		
		// 인스턴스를 저장하는 정적 변수명을 반환 
		return "instance";
	}

	public void onReceive(Context context, Intent intent)
	{
		Log.d(LOG_TAG, "[StzCommon] onReceive");

		StzNotificationTask task = new StzNotificationTask();
		Bundle bundle = intent.getExtras();
		int notiType = bundle.getInt("notiType");
		String imageUrl = bundle.getString("image_url");

		Log.d(LOG_TAG, "[StzCommon] onReceive : notiType : " + notiType + ", imageUrl : " + imageUrl );
		task.SetNotiParams(context, intent);
		if( notiType == EXPANDED_NOTI && imageUrl != null)
		{
			task.execute(new String[]{imageUrl});
		}	
		else
		{
			task.createCumstomNoti();
		}

		Log.d(LOG_TAG, "[StzCommon] END");
		return;		
	}
}