package com.sundaytoz.plugins.common.funtions;

import org.json.JSONObject;

import com.sundaytoz.plugins.common.SundaytozResponseHandler;
import com.sundaytoz.plugins.common.SundaytozAndroid.Action;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

public class GetPushId 
{
	private static final String TAG = "GetPushId";
	public static void getPushId(Context inContext)
	{
		Log.d(TAG, "Get Push Id Start!");
		SharedPreferences prefs = inContext.getApplicationContext().getSharedPreferences("androidPush", Context.MODE_PRIVATE);
		String pushId = prefs.getString("push_id", "");
		
		if(pushId.equals(""))
		{
			try
			{
				JSONObject result = new JSONObject();
				result.put("status", "1");
				result.put(Action.Param.message, "failed, not stored push id in prefs");
				
				SundaytozResponseHandler.current.onError(0, result);;
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}					
		}
		else
		{
			try
			{
				JSONObject result = new JSONObject();
				result.put(Action.Param.registration_id, pushId);
				
				SundaytozResponseHandler.current.onComplete(1, result);
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}		
				
		}
	}
}
	



