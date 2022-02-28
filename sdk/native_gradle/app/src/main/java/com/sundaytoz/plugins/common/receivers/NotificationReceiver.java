package com.sundaytoz.plugins.common.receivers;


import java.util.Map;
import java.util.regex.Pattern;

import com.sundaytoz.plugins.common.utils.NotificationUtil;
import com.sundaytoz.plugins.common.utils.StorageUtil;

import android.annotation.TargetApi;
import android.app.NotificationManager;
import android.app.NotificationChannel;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.res.Resources;
import android.graphics.BitmapFactory;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import androidx.core.app.NotificationCompat;
import androidx.core.app.NotificationCompat.Builder;
import android.text.Html;
import android.util.Log;
import android.widget.RemoteViews;

public class NotificationReceiver extends BroadcastReceiver
{
	private static final String LOG_TAG = "Sundaytoz";

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

	
	private static int getResourcesColor(Resources paramResources, int paramInt)
	{
		if (Build.VERSION.SDK_INT < 23) {}
		for (paramInt = paramResources.getColor(paramInt);; paramInt = getResourcesColorNew(paramResources, paramInt)) {
			return paramInt;
		}
	}
	
	@TargetApi(23)
	private static int getResourcesColorNew(Resources paramResources, int paramInt)
	{
		return paramResources.getColor(paramInt, null);
	}	

	// adapted from post by Phil Haack and modified to match better
	public final static String tagStart=
			"\\<\\w+((\\s+\\w+(\\s*\\=\\s*(?:\".*?\"|'.*?'|[^'\"\\>\\s]+))?)+\\s*|\\s*)\\>";
	public final static String tagEnd=
			"\\</\\w+\\>";
	public final static String tagSelfClosing=
			"\\<\\w+((\\s+\\w+(\\s*\\=\\s*(?:\".*?\"|'.*?'|[^'\"\\>\\s]+))?)+\\s*|\\s*)/\\>";
	public final static String htmlEntity=
			"&[a-zA-Z][a-zA-Z0-9]+;";
	public final static Pattern htmlPattern=Pattern.compile(
			"("+tagStart+".*"+tagEnd+")|("+tagSelfClosing+")|("+htmlEntity+")",
			Pattern.DOTALL
	);

	/**
	 * Will return true if s contains HTML markup tags or entities.
	 *
	 * @param s String to test
	 * @return true if string contains HTML
	 */
	public static boolean isHtml(String s) {
		boolean ret=false;
		if (s != null) {
			ret=htmlPattern.matcher(s).find();
		}
		return ret;
	}
	private CharSequence convertHtmlToCharsequence(String text)
	{
		if (android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.N)
		{
			// noinspection deprecation
			return Html.fromHtml(text);
		}
		return Html.fromHtml(text, Html.FROM_HTML_MODE_LEGACY);
	}

	private CharSequence convertRawToString(String rawString, boolean isBold )
	{
		CharSequence retMessage = "";
		String bundleMessage = rawString;

		boolean isHtmlTag = isHtml(bundleMessage);

		Log.d(LOG_TAG, "NotificationReceiver::onReceive isHtmlTag :" + isHtmlTag );
		if( isHtmlTag )
		{
			retMessage = convertHtmlToCharsequence(bundleMessage);
		}
		else
		{
			bundleMessage = bundleMessage.replace("\\r\\n", "\r\n");
			bundleMessage = bundleMessage.replace("\\n", "\n");
			bundleMessage = bundleMessage.replace("\\r", "\r");
			bundleMessage = bundleMessage.replace(".", "\n");

			retMessage = bundleMessage;
		}
		if( isBold && isHtmlTag == false )
		{
			retMessage = convertHtmlToCharsequence(("<b>" + bundleMessage +"</b>"));
		}
		return retMessage;
	}

	public void onReceive(Context context, Intent intent)
	{
		Log.d(LOG_TAG, "[StzCommon] onReceive");
		
		Resources localResources = context.getResources();
		Builder notiBuilder = null;
		int large_icon_id = 0;
		CharSequence title = "";
		CharSequence message = "";
		String app_param = "";
		Bundle bundle = intent.getExtras();
		int alarmId = 0;
		int notiType = 0;

		int title_color = 0;	//	"color_notif_text";
		int msg_color = 0;		//  "color_notif_title";
		int title_size 	= 18;
		int msg_size 	= 13;

		String bg_image_name 	= "notif_background";
		
		try
		{
			Map<String,String> savedNotiInfo = StorageUtil.getNotificationInfo(context);

			title = convertRawToString(bundle.getString("title"), true );

			message = convertRawToString(bundle.getString("message"), false );

			alarmId 	= bundle.getInt("id");
			notiType 	= bundle.getInt("notiType");
			
			if (title==null){
				title = "";
			}
			if (message==null){
				message = "";
			}	
			
			if (title == "" && message == ""){
				return;
			}
			String tmp 		= null;
			
			app_param	= bundle.getString("app_param");

			tmp 		= bundle.getString("bg_image_name");
			if( !tmp.isEmpty()){
				bg_image_name = tmp;
			}
			
			tmp 		= bundle.getString("msg_color");
			if( tmp == null || tmp.isEmpty()){
				tmp = savedNotiInfo.get("msg_color");
			}
			if( tmp != null && !tmp.isEmpty()){
				msg_color = Color.parseColor(tmp);
			}
			
			tmp 		= bundle.getString("title_color");
			if( tmp == null || tmp.isEmpty()){
				tmp = savedNotiInfo.get("title_color");
			}			
			if( tmp != null && !tmp.isEmpty()){
				title_color = Color.parseColor(tmp);
			}
			

			int noti_icon_id = 0;

			if (Build.VERSION.SDK_INT >= 21)
			{
				noti_icon_id = localResources.getIdentifier("ic_notif_white", "drawable", context.getPackageName());
			}

			if (noti_icon_id==0)
			{
				noti_icon_id = localResources.getIdentifier("ic_notif", "drawable", context.getPackageName());
			}

			if (noti_icon_id==0)
			{
				noti_icon_id = localResources.getIdentifier("app_icon", "drawable", context.getPackageName());
			}
			
			large_icon_id = localResources.getIdentifier("ic_notif_large", "drawable", context.getPackageName());
			
			if(noti_icon_id==0)
			{
				large_icon_id = localResources.getIdentifier("app_icon", "drawable", context.getPackageName());	
			}

			notiBuilder = new NotificationCompat.Builder(context)
					.setSmallIcon(noti_icon_id)
					.setContentTitle(title)
					.setContentText(message)
					.setTicker(title)
					.setAutoCancel(true)					
					.setDefaults(1);
		}
		catch(Exception ex)
		{
			Log.d(LOG_TAG, "[StzCommon] Exception " + ex.getMessage() );
			ex.printStackTrace();
			return;
		}

		if (large_icon_id!=0){
			notiBuilder.setLargeIcon( BitmapFactory.decodeResource(context.getResources(), large_icon_id) );
			Log.d(LOG_TAG, "[StzCommon] setLargeIcon " + large_icon_id);
		}

		if (Build.VERSION.SDK_INT >= 21)
		{
			int color_notif_bkg = localResources.getIdentifier("color_notif_bkg", "color", context.getPackageName());
			if (color_notif_bkg != 0) {
				notiBuilder.setColor(getResourcesColor(localResources, color_notif_bkg));
			}
			Log.d(LOG_TAG, "[StzCommon] color_notif_bkg " + color_notif_bkg);
		}

		int notification_layout_id 	= localResources.getIdentifier("notification_default", "layout", context.getPackageName());
		Log.d(LOG_TAG, "[StzCommon] notification_layout_id " + notification_layout_id);
		
		int notif_background_id 	= localResources.getIdentifier(bg_image_name, "drawable", context.getPackageName());
		Log.d(LOG_TAG, "[StzCommon] notif_background_id " + notif_background_id);
		
		RemoteViews remote_view = new RemoteViews(context.getPackageName(), notification_layout_id);
		if (notif_background_id!=0){
			int show_bg = localResources.getIdentifier("notif_background", "id", context.getPackageName());
			if (show_bg != 0) {
				remote_view.setImageViewBitmap(show_bg, BitmapFactory.decodeResource(context.getResources(), notif_background_id));
			}
			Log.d(LOG_TAG, "[StzCommon] show_bg " + show_bg);
		}
		
		if (large_icon_id!=0){
			int notif_image_id = localResources.getIdentifier("notif_image", "id", context.getPackageName());
			if (notif_image_id != 0) {
				remote_view.setImageViewBitmap(notif_image_id, BitmapFactory.decodeResource(context.getResources(), large_icon_id));
			}
			Log.d(LOG_TAG, "[StzCommon] notif_image_id " + notif_image_id);
		}		
		int notif_title_id = localResources.getIdentifier("notif_title", "id", context.getPackageName());
		if (notif_title_id != 0)
		{
			remote_view.setTextViewText(notif_title_id, title);
			if (title_color!=0){
				remote_view.setTextColor(notif_title_id, title_color);
			}else{
				int color_notif_title = localResources.getIdentifier("color_notif_title", "color", context.getPackageName());
				if (color_notif_title != 0) {
					remote_view.setTextColor(notif_title_id, getResourcesColor(localResources, color_notif_title));
				}
			}
			if( title_size != 0 )
			{
				Log.d(LOG_TAG, "title_size " + title_size);
				remote_view.setTextViewTextSize(notif_title_id, android.util.TypedValue.COMPLEX_UNIT_DIP, title_size);
			}

			Log.d(LOG_TAG, "[StzCommon] color_notif_title " + title_color);
		}
		
		int notif_message_id = localResources.getIdentifier("notif_message", "id", context.getPackageName());
		if (notif_message_id != 0)
		{
			remote_view.setTextViewText(notif_message_id, message);
			if (msg_color!=0){
				remote_view.setTextColor(notif_message_id, msg_color);
			}else{
				int color_notif_text = localResources.getIdentifier("color_notif_text", "color", context.getPackageName());
				if (color_notif_text != 0) {
					remote_view.setTextColor(notif_message_id, getResourcesColor(localResources, color_notif_text));
				}
			}			
			if( msg_size != 0 )
			{
				Log.d(LOG_TAG, "[StzCommon] msg_size " + msg_size);
				remote_view.setTextViewTextSize(notif_message_id, android.util.TypedValue.COMPLEX_UNIT_DIP, msg_size);
			}
			
			Log.d(LOG_TAG, "[StzCommon] color_notif_text " + msg_color);
		}
		notiBuilder.setContent(remote_view);
		notiBuilder.setStyle(null);

		
		
		String className = StorageUtil.getMainActivityName(context);
		if(className == null || className.isEmpty()){
			className = "com.unity3d.player.UnityPlayerActivity";
		}
		Log.d(LOG_TAG, "[StzCommon] className " + className );
		
		String ns = Context.NOTIFICATION_SERVICE;
		NotificationManager notificationManager = (NotificationManager)context.getSystemService(ns);
		
		Intent notificationIntent = null;
		try{
			notificationIntent = new Intent(context, Class.forName(className));
		}catch(Exception e){
			Log.d(LOG_TAG, "[StzCommon] Exception " + e.getMessage() );
			return;
		}
		
		notificationIntent.putExtra("app_param", app_param);
		notificationIntent.addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP | Intent.FLAG_ACTIVITY_CLEAR_TOP);

		PendingIntent contentIntent = PendingIntent.getActivity(context.getApplicationContext(), alarmId, notificationIntent, PendingIntent.FLAG_ONE_SHOT );


		notificationIntent.addCategory("android.intent.category.LAUNCHER");
		notificationIntent.setAction("android.intent.action.MAIN");

		notiBuilder.setContentIntent(contentIntent);
		notificationManager.cancel(alarmId);

		if (Build.VERSION.SDK_INT > 25)
		{
			// The user visible name of the channel.
			int app_name_id = localResources.getIdentifier("app_name", "string", context.getPackageName());	
			CharSequence name = context.getString(app_name_id);

			final StringBuilder sb = new StringBuilder(name.length());
	        sb.append(name);
			String id = sb.toString();

			int importance = NotificationManager.IMPORTANCE_LOW;
			NotificationChannel mChannel = new NotificationChannel(id, name, importance);
			// Configure the notification channel.
			mChannel.enableLights(true);
			// Sets the notification light color for notifications posted to this
			// channel, if the device supports this feature.
			mChannel.setLightColor(Color.RED);
			mChannel.enableVibration(true);
			mChannel.setVibrationPattern(new long[]{100, 200, 300, 400, 500, 400, 300, 200, 400});
			notificationManager.createNotificationChannel(mChannel);
			
			notiBuilder.setChannelId(id);
		}

		notificationManager.notify(alarmId, notiBuilder.build());

		// 저장된 노티 정보 제거 
		int pendingType = NotificationUtil.toPendingType(notiType);
		NotificationUtil.removeAlarmInfoFromSahredPref(context, alarmId, pendingType);
		
		Log.d(LOG_TAG, "[StzCommon] END");
		return;		
	}
}