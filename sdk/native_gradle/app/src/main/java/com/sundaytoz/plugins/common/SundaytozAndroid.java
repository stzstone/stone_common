package com.sundaytoz.plugins.common;

import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Locale;
import java.util.TimeZone;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.IntentSender;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.Signature;
import android.util.Base64;
import android.util.Log;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.provider.Settings;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import android.telephony.TelephonyManager;
import android.view.WindowManager;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.install.InstallState;
import com.google.android.play.core.install.InstallStateUpdatedListener;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.InstallStatus;
import com.google.android.play.core.install.model.UpdateAvailability;
import com.google.android.play.core.tasks.OnSuccessListener;
import com.google.android.play.core.tasks.Task;
import com.sundaytoz.plugins.common.enums.ENetworkType;
import com.sundaytoz.plugins.common.funtions.GetPushId;
import com.sundaytoz.plugins.common.funtions.LocalNotification;
import com.sundaytoz.plugins.common.funtions.ShowAlert;
import com.sundaytoz.plugins.common.funtions.UsingSystemClipboard;
import com.sundaytoz.plugins.common.utils.DiskUtil;
import com.sundaytoz.plugins.common.utils.NetworkUtil;
import com.sundaytoz.plugins.common.utils.NotificationUtil;
import com.sundaytoz.plugins.common.utils.PermissionUtil;
import com.sundaytoz.plugins.common.utils.STZLog;
import com.sundaytoz.plugins.common.utils.StorageUtil;
import com.unity3d.player.UnityPlayer;

public class SundaytozAndroid {
	private static final String TAG = SundaytozAndroid.class.toString();

	private static final String UNITY_RESPONSE_CLASS = "SundaytozResponseHandler";
	private static final String UNITY_RESPONSE_METHOD = "OnResponseCallback";
	private static final String UNITY_ERROR_METHOD = "OnSundaytozResponseError";

	private static final int IAB_REQUEST_CODE = 1292;

	public static SundaytozAndroid instance = null;

	/**
	 * 초기화 Unity측에서 호출하기 됨
	 */
	public static String createInstance() {
		if (instance == null) {
			instance = new SundaytozAndroid();
		}

		// 인스턴스를 저장하는 정적 변수명을 반환
		return "instance";
	}


	/**
	 *
	 */
	public class Action {

		/* 초기화 */
		public static final String INITIALIZE = "INITIALIZE";
		
		public static final String CLEAR_INSTALL_REFERRER = "CLEAR_INSTALL_REFERRER";

		/* 로컬 푸시 메세지 등록 */
		public static final String LOCAL_NOTIFICATION_ADD = "LOCAL_NOTIFICATION_ADD";

		/* 로컬 푸시 메세지 등록 해제 */
		public static final String LOCAL_NOTIFICATION_CANCEL = "LOCAL_NOTIFICATION_CANCEL";

		/* 모든 로컬 푸시 메세지 취소 */
		public static final String LOCAL_NOTIFICATION_ALL_CANCEL = "LOCAL_NOTIFICATION_ALL_CANCEL";

		/* 시스템 팝업 출력 */
		public static final String SHOW_ALERT = "SHOW_ALERT";

		/* 푸시토큰 조회 */
		public static final String GET_PUSH_ID = "GET_PUSH_ID";
		
		/* 안드로이드 시그니쳐 조회 */
		public static final String GET_SIGNATURE = "GET_SIGNATURE";
		
		/* 네트워크 상태 조회 */
		public static final String GET_NETWORK_STATUS = "GET_NETWORK_STATUS";
		
		
		/* 네트워크 변경 상태 알림 */
		public static final String ON_CHANGED_NETWORK_STATUS = "ON_CHANGED_NETWORK_STATUS";
		
		/* 시스템 언어 가져오기 */
		public static final String GET_LANGUAGE = "GET_LANGUAGE";
		
		/* 시스템 클립보드에 텍스트 복사 */
		public static final String SET_TEXT_TO_SYSTEM_CLIPBOARD = "SET_TEXT_TO_SYSTEM_CLIPBOARD";

		/* 권한 획득 요청 */
		public static final String REQUEST_PERMISSION = "REQUEST_PERMISSION";

		/* 스크린 보안 활성화 */
		public static final String ENABLE_SECURE_DISPLAY = "ENABLE_SECURE_DISPLAY";

		/* 인텐트 데이터 요청 */
		public static final String GET_SCHEME_DATA = "GET_SCHEME_DATA";

		/* 남은 용량 조회 */
		public static final String GET_FREE_SPACE_MB = "GET_FREE_SPACE_MB";

        /* 인텐트 데이터 삭제 */
        public static final String CLEAR_SCHEME_DATA = "CLEAR_SCHEME_DATA";

        /* 시그니쳐값 */
        public static final String GET_STIMER = "GET_STIMER";

        /* 설치 여부 */
        public static final String IS_INSTALLED = "IS_INSTALLED";

        /* 퍼미션 획득 상태 */
        public static final String GET_PERMISSION_GRANT_TYPE = "GET_PERMISSION_GRANT_TYPE";

        /* 획득하지 못한 모든 퍼미션 */
        public static final String GET_ALL_UNGRANTED_PERMISSIONS = "GET_ALL_UNGRANTED_PERMISSIONS";

        /* 퍼미션 요청 */
        public static final String REQUEST_PERMISSIONS = "REQUEST_PERMISSIONS";

		/* 인앱 업데이트 다운로드 요청 */
		public static final String IN_APP_UPDATE_DOWNLOAD = "IN_APP_UPDATE_DOWNLOAD";
		/* 인앱 업데이트 유연한 설치 */
		public static final String IN_APP_UPDATE_INSTALL_FLEXIBLE = "IN_APP_UPDATE_INSTALL_FLEXIBLE";

		/* 인앱 업데이트 즉시 설치*/
		public static final String IN_APP_UPDATE_INSTALL_IMMEDIATE = "IN_APP_UPDATE_INSTALL_IMMEDIATE";


		/**
		 * 
		 */
		public class Param {
			public static final String action = "action";
			public static final String result = "result";
			public static final String data = "data";
			public static final String error = "error";
			public static final String error_code = "error_code";
			public static final String error_msg = "error_msg";
			public static final String status = "status";

			public static final String type = "type";
			public static final String alarm_id = "alarm_id";
			public static final String time = "time";
			public static final String message = "message";
			public static final String title = "title";
			public static final String iconId = "iconId";
			public static final String counter = "counter";
			public static final String text = "text";
			public static final String okay = "okay";
			public static final String cancel = "cancel";
			
			public static final String bg_image_name = "bg_image_name";
			public static final String title_color = "title_color";
			public static final String msg_color = "msg_color";
			public static final String titleSize = "title_size";
			public static final String msgSize = "msg_size";

			public static final String device_name = "device_name";
			public static final String carrier = "carrier";
			public static final String country = "country";
			public static final String os_version = "os_version";
			public static final String app_params = "app_params";
			public static final String timezone        = "timezone";
			public static final String timeoffset      = "timeoffset";
            public static final String free_space_mb      = "free_space_mb";
            public static final String size      = "size";
			public static final String version_name      = "version_name";

			public static final String local_country = "local_country";
			public static final String network_country = "network_country";
			public static final String sim_state = "sim_state";

			public static final String api_level = "api_level";
			public static final String registration_id = "registration_id";
			
			public static final String network_type = "network_type";
			public static final String language = "language";

			public static final String permission_name = "permission";
            public static final String permissions = "permissions";

			public static final String install_referrer = "install_referrer";
			public static final String main_activity_name 	= "main_activity_name";

			public static final String is_enable      = "is_enable";
			public static final String scheme_data = "scheme_data";
			public static final String stimer       = "stimer";
            public static final String package_name = "package_name";
            public static final String installed = "installed";
			public static final String update_available = "update_available";
			public static final String update_flexible = "update_flexible";
			public static final String update_immediate = "update_immediate";
			public static final String is_downloaded = "is_downloaded";
			public static final String start_update = "start_update";
		};
	};

	/* */
	private HashMap<String, SundaytozResponseHandler> handlers = new HashMap<String, SundaytozResponseHandler>();

	OnSuccessListener<AppUpdateInfo> appUndateInfoListener = new OnSuccessListener<AppUpdateInfo>() {
		@Override
		public void onSuccess(AppUpdateInfo _appUpdateInfo) {

			Log.v("STZCommon", "SundaytozAndroid::appUndateInfoListener()-> onSuccess");

			appUpdateInfo = _appUpdateInfo;

			if( _appUpdateInfo.updateAvailability() == UpdateAvailability.UPDATE_AVAILABLE )
				updateAvailable = true;
			else
				updateAvailable = false;

			isAllowedFlexible = _appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.FLEXIBLE);
			isAllowedImmediate = _appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.IMMEDIATE);

			Log.v("STZCommon", "SundaytozAndroid::appUndateInfoListener()-> updateAvailable : " + updateAvailable
					+ ", isAllowedFlexible : " + isAllowedFlexible
					+ ", isAllowedImmediate : " + isAllowedImmediate);


		}
	};

	final InstallStateUpdatedListener inAppUpdateFlexibleListener = new InstallStateUpdatedListener() {
		@Override
		public void onStateUpdate(InstallState state) {
			Log.v("STZCommon", "SundaytozAndroid::StartUpdateFlex()-> status : " + state.installStatus() );
			switch (state.installStatus())
			{
				case InstallStatus.DOWNLOADING:
					long bytesDownloaded = state.bytesDownloaded();
					long totalByteToDownload = state.totalBytesToDownload();
					Log.v("STZCommon", "SundaytozAndroid::inAppUpdateFlexibleListener()-> " + bytesDownloaded + " bytes download, total : " + totalByteToDownload);
					break;

				case InstallStatus.INSTALLED:
					JSONObject json = new JSONObject();
					sendSuccess(Action.IN_APP_UPDATE_INSTALL_FLEXIBLE, json);
					break;
				case InstallStatus.UNKNOWN:
				case InstallStatus.PENDING:
				case InstallStatus.INSTALLING:
					break;
				case InstallStatus.FAILED:
				case InstallStatus.CANCELED:
					SendInAppUpdateDownloaded(false);
					break;
				case InstallStatus.DOWNLOADED:
					SendInAppUpdateDownloaded(true);
					break;
				default:
					break;
			}
		}
	};

	public void RefreshInAppUpdateIsDownloaded() {

		if (appUpdateManager != null) {

			appUpdateManager.getAppUpdateInfo()
					.addOnSuccessListener(new OnSuccessListener<AppUpdateInfo>() {
						@Override
						public void onSuccess(AppUpdateInfo result) {
							Log.v("STZCommon", "SundaytozAndroid::RefreshInstallStatus()->installStatus : " + appUpdateInfo.installStatus());
							if (appUpdateInfo.installStatus() == InstallStatus.DOWNLOADED)
							{
								SendInAppUpdateDownloaded(true);
							}
						}
					});
		}
	}

	private void SendInAppUpdateDownloaded(boolean isSuccess)
	{
		Log.v("STZCommon", "SundaytozAndroid::SendInAppUpdateDownloaded()->");
		JSONObject json = new JSONObject();
		try
		{
			json.put(Action.Param.is_downloaded, isSuccess);

			sendSuccess(Action.IN_APP_UPDATE_DOWNLOAD, json);
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}
	}

	AppUpdateManager appUpdateManager;
	AppUpdateInfo appUpdateInfo;

	private boolean updateAvailable = false;
	private boolean isAllowedFlexible = false;
	private boolean isAllowedImmediate = false;


	public void RefreshInAppUpdateStatus()
	{
		Log.v("STZCommon", "SundaytozAndroid::RefreshInAppUpdateStatus()-> begin");

		appUpdateManager = AppUpdateManagerFactory.create(UnityPlayer.currentActivity.getApplicationContext());

		Task<AppUpdateInfo> appUpdateInfoTask = appUpdateManager.getAppUpdateInfo();

		appUpdateInfoTask.addOnSuccessListener(appUndateInfoListener);

		Log.v("STZCommon", "SundaytozAndroid::RefreshInAppUpdateStatus()-> end");
	}

	public void DownloadInAppUpdate()
	{
		Log.v("STZCommon", "SundaytozAndroid::DownloadInAppUpdate()-> begin");

		appUpdateManager.registerListener(inAppUpdateFlexibleListener);

		try {
			appUpdateManager.startUpdateFlowForResult(
					appUpdateInfo,
					AppUpdateType.FLEXIBLE,
					UnityPlayer.currentActivity,
					IAB_REQUEST_CODE);
		} catch (IntentSender.SendIntentException e) {
			e.printStackTrace();
			Log.e("STZCommon", e.getLocalizedMessage());
		}

	}

	/**
	 * @inParam inActivity
	 */
	private void initialize(final Activity inActivity) {
		handlers.clear();
		
		Intent intent = inActivity.getIntent();
		if(intent.hasExtra("app_param"))
		{
			NotificationUtil.APP_PARAM = intent.getStringExtra("app_param");
		}

		inActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				Log.d(TAG, "SundaytozAndroid::initialize()->0428_3");

				Context context = inActivity.getApplicationContext();

				handlers.put(Action.SHOW_ALERT, new SundaytozResponseHandler(
						context) {
					@Override
					public void onStart() {
						super.onStart();
					}

					@Override
					public void onComplete(int inStatusCode, JSONObject inResult) {
						sendSuccess(Action.SHOW_ALERT, inResult);
					}

					@Override
					public void onError(int inStatusCode, JSONObject inResult) {
						sendError(Action.SHOW_ALERT, inResult, inStatusCode, "Empty");
					}
				});

				handlers.put(Action.GET_PUSH_ID, new SundaytozResponseHandler(context)
				{
					@Override
					public void onComplete(int inStatusCode, JSONObject inResult) { sendSuccess(Action.GET_PUSH_ID, inResult); }

					@Override
					public void onError(int inStatusCode, JSONObject inResult) { sendError(Action.GET_PUSH_ID, inResult, inStatusCode, "Empty"); }
				});

				sendSuccess(Action.INITIALIZE, getDeviceInfo(inActivity));
			}
		});
	}

	/**
	 * @inParam inActivity
	 * @inParam inParam
	 */
	public void execute(final Activity inActivity, final String inParam) {
		STZLog.d(inParam);

		JSONObject json = null;

		try {
			json = new JSONObject(inParam);
			process(inActivity, json);
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	/**
	 * 디바이스 정보 수집
	 * 
	 * @return
	 */
	public JSONObject getDeviceInfo(Activity inActivity) {
		JSONObject json = new JSONObject();

		try {
			Context context = inActivity.getApplicationContext();
			
			TelephonyManager manager = (TelephonyManager) inActivity
					.getSystemService(Context.TELEPHONY_SERVICE);

			final String carrierName = manager.getNetworkOperatorName();
			String country = manager.getNetworkCountryIso();
			final String appParams = NotificationUtil.APP_PARAM;

			json.put(Action.Param.device_name,
					android.os.Build.MODEL.toString());
			json.put(Action.Param.carrier,
					(carrierName.length() > 0 ? carrierName : "Empty"));
			String simState = "Empty";
			switch (manager.getSimState())
			{
				case TelephonyManager.SIM_STATE_READY:
					simState = "1";
					break;
				case TelephonyManager.SIM_STATE_UNKNOWN:
				case TelephonyManager.SIM_STATE_ABSENT:
				case TelephonyManager.SIM_STATE_PIN_REQUIRED:
				case TelephonyManager.SIM_STATE_PUK_REQUIRED:
				case TelephonyManager.SIM_STATE_NETWORK_LOCKED:
				case TelephonyManager.SIM_STATE_NOT_READY:
				case TelephonyManager.SIM_STATE_PERM_DISABLED:
				case TelephonyManager.SIM_STATE_CARD_IO_ERROR:
				case TelephonyManager.SIM_STATE_CARD_RESTRICTED:
					simState = "0";
					break;
			}
			json.put(Action.Param.sim_state, simState);
			json.put(Action.Param.local_country,
					Build.VERSION.SDK_INT >= Build.VERSION_CODES.N ?
							context.getResources().getConfiguration().getLocales().get(0).getCountry() :
							context.getResources().getConfiguration().locale.getCountry() );
			json.put(Action.Param.network_country, manager.getNetworkCountryIso() );

			if( country.length() <= 0 )
			{
				if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
		            country = context.getResources().getConfiguration().getLocales().get(0).getCountry();
		        } else {
		            country = context.getResources().getConfiguration().locale.getCountry();
		        }
			}
			json.put(Action.Param.country, (country.length() > 0 ? country
					: "Empty")); // SIM이 없는 경우 값이 없음
			json.put(Action.Param.os_version, Build.VERSION.RELEASE);
			json.put(Action.Param.api_level, Build.VERSION.SDK_INT);
			if( appParams == null || appParams.isEmpty() )
				json.put(Action.Param.app_params, "Empty");
			else
				json.put(Action.Param.app_params, (appParams.length() > 0 ? appParams : "Empty"));
			
			TimeZone tz = TimeZone.getDefault();
			
			json.put(Action.Param.timezone, tz.getID() );
			json.put(Action.Param.install_referrer, StorageUtil.getInstallReferer(context) );
			
			Date now = new Date();
			int offsetFromUtc = tz.getOffset(now.getTime()) / 3600000;
			String m2tTimeZoneIs = Integer.toString(offsetFromUtc);
			json.put(Action.Param.timeoffset, m2tTimeZoneIs);
			json.put(Action.Param.free_space_mb, DiskUtil.freeSpace(false));
			String version_name = "";
			try {
				PackageInfo pInfo = UnityPlayer.currentActivity.getPackageManager().getPackageInfo(UnityPlayer.currentActivity.getPackageName(), 0);
				version_name = pInfo.versionName;
			} catch (PackageManager.NameNotFoundException e) {
				e.printStackTrace();
			}

			json.put(Action.Param.version_name, version_name);

			json.put(Action.Param.update_available, updateAvailable);
			json.put(Action.Param.update_flexible, isAllowedFlexible);
			json.put(Action.Param.update_immediate, isAllowedImmediate);

			Log.d(TAG, "getDeviceInfo: " + json.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}

		return json;
	}
	
	/**
	 * 네트워크 변경 알림 
	 * @param inType
	 */
	public void changedNetworkStatus(ENetworkType inType)
	{
		JSONObject json = new JSONObject();

		try
		{
			json.put(Action.Param.network_type, inType.getTypeName());
			
			sendSuccess(Action.ON_CHANGED_NETWORK_STATUS, json);
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}
	}

	/**
	 * @param inAction
	 * @param inData
	 */
	public void sendSuccess(String inAction, JSONObject inData)
	{
		JSONObject json = new JSONObject();

		try
		{
			json.put(Action.Param.action, inAction);
			json.put(Action.Param.data, inData);

			sendMessageToUnity(UNITY_RESPONSE_CLASS, UNITY_RESPONSE_METHOD, json.toString());
		}
		catch (JSONException e)
		{
			e.printStackTrace();
		}
	}

	/**
	 * 오류 결과를 유니티에 통보
	 * 
	 * @param inAction
	 * @param inError
	 */
	public void sendError(String inAction, JSONObject inError, int error_code, String inErrorMsg) {
		JSONObject json = new JSONObject();

		try {
			json.put(Action.Param.action, inAction);
			json.put(Action.Param.error, inError);
			json.put(Action.Param.error_code, error_code);
			json.put(Action.Param.error_msg, inErrorMsg);

			// Log.d(TAG, "sendError: " + json.toString());

			sendMessageToUnity(UNITY_RESPONSE_CLASS, UNITY_ERROR_METHOD,
					json.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}

	/**
	 * 네이트브 루틴 실행
	 * 
	 * @param inActivity
	 * @param inParam
	 */
	protected void process(final Activity inActivity, final JSONObject inParam)
	{
		final String actionName = inParam.optString(Action.Param.action);

		Log.v(TAG, "SundaytozAndroid::process()->actionName:" + actionName);
		if (actionName.equals(Action.LOCAL_NOTIFICATION_ADD)) {
			final int alarmId = inParam.optInt(Action.Param.alarm_id);
			final int time = inParam.optInt(Action.Param.time);
			final String message = inParam.optString(Action.Param.message);
			final String title = inParam.optString(Action.Param.title);
			final int counter = inParam.optInt(Action.Param.counter);
			final int type = inParam.optInt(Action.Param.type);
			
			final String inBgImageName 	= inParam.optString(Action.Param.bg_image_name);
			final String inTitleColor 	= inParam.optString(Action.Param.title_color);
			final String inMsgColor 	= inParam.optString(Action.Param.msg_color);
			final int titleSize = inParam.optInt(Action.Param.titleSize);
			final int msgSize = inParam.optInt(Action.Param.msgSize);
			
			LocalNotification.add(inActivity, alarmId, time, message, title, counter, type, inBgImageName, inTitleColor,inMsgColor, titleSize, msgSize);
			
			return;
		} else if (actionName.equals(Action.LOCAL_NOTIFICATION_CANCEL)) {
			final int alarmId = inParam.optInt(Action.Param.alarm_id);
			final int type = inParam.optInt(Action.Param.type);

			LocalNotification.cancel(inActivity, alarmId, type);
			return;
		} else if (actionName.equals(Action.LOCAL_NOTIFICATION_ALL_CANCEL)) {
			LocalNotification.cancelAll(inActivity);
			return;
		} else if (actionName.equals(Action.GET_PUSH_ID)) {
			SundaytozResponseHandler.current = handlers.get(actionName);
			GetPushId.getPushId(inActivity);
			return;
		} else if (actionName.equals(Action.GET_SIGNATURE)) {
			SundaytozResponseHandler.current = handlers.get(actionName);
			GetPushId.getPushId(inActivity);
			return;
		}
		else if (actionName.equals(Action.GET_NETWORK_STATUS))
		{
			SundaytozResponseHandler.current = handlers.get(actionName);
			ENetworkType type = NetworkUtil.getStatus(inActivity);
			
			try
			{
				JSONObject json = new JSONObject();
				json.put(Action.Param.network_type, type.getTypeName());
				
				sendSuccess(Action.GET_NETWORK_STATUS, json);
			}
			catch(JSONException e)
			{
				e.printStackTrace();
				sendError(Action.GET_NETWORK_STATUS, new JSONObject(), e.hashCode(), e.getMessage());;
			}
			
			return;
		}
		else if (actionName.equals(Action.GET_LANGUAGE))
		{
			SundaytozResponseHandler.current = handlers.get(actionName);

            try
            {
                JSONObject result = new JSONObject();
                result.put(Action.Param.language, Locale.getDefault().toString());

                sendSuccess(Action.GET_LANGUAGE, result);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_LANGUAGE, new JSONObject(), e.hashCode(), e.getMessage());;
            }

            return;
		}
		else if (actionName.equals(Action.CLEAR_INSTALL_REFERRER))
		{
			Context context = inActivity.getApplicationContext();
			StorageUtil.clearInstallReferer(context);
			return;
		}
		else if (actionName.equals(Action.ENABLE_SECURE_DISPLAY))
		{
			final boolean isEnable = inParam.optBoolean(Action.Param.is_enable);
			STZLog.d(String.format("EnableSecureDisplay::-> isEnable : %b", isEnable));
			UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() 
			{
				if( isEnable )
					UnityPlayer.currentActivity.getWindow().addFlags(WindowManager.LayoutParams.FLAG_SECURE);
				else
					UnityPlayer.currentActivity.getWindow().clearFlags(WindowManager.LayoutParams.FLAG_SECURE);	
				}
			});
		}
		else if (actionName.equals(Action.GET_SCHEME_DATA))
		{
			try
			{
				JSONObject json = new JSONObject();
				json.put(Action.Param.scheme_data, StorageUtil.getIntentData(inActivity.getApplicationContext()));
				Log.v(TAG, "GET_SCHEME_DATA(): " + StorageUtil.getIntentData(inActivity.getApplicationContext()));
				sendSuccess(Action.GET_SCHEME_DATA, json);
				//StorageUtil.saveIntentData(inActivity.getApplicationContext(), "");
			}
			catch(Exception e)
			{
				e.printStackTrace();
				sendError(Action.GET_SCHEME_DATA, new JSONObject(), e.hashCode(), e.getMessage());;
			}
		}
		else if (actionName.equals(Action.CLEAR_SCHEME_DATA))
		{
			StorageUtil.saveIntentData(inActivity.getApplicationContext(), "Empty");
		}
        else if (actionName.equals(Action.GET_FREE_SPACE_MB))
        {
            try
            {
                JSONObject json = new JSONObject();
                json.put(Action.Param.size, DiskUtil.freeSpace(false));

                sendSuccess(Action.GET_FREE_SPACE_MB, json);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_FREE_SPACE_MB, new JSONObject(), e.hashCode(), e.getMessage());;
            }
        }
        else if (actionName.equals(Action.GET_STIMER))
        {
            try
            {
                JSONObject json = new JSONObject();
                json.put(Action.Param.stimer, getStimer());

                sendSuccess(Action.GET_STIMER, json);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_STIMER, new JSONObject(), e.hashCode(), e.getMessage());;
            }
        }
        else if (actionName.equals(Action.IS_INSTALLED))
        {
            try
            {
                final String paraPackageName = inParam.optString(Action.Param.package_name);

                JSONObject json = new JSONObject();
                json.put(Action.Param.installed, isInstalled(paraPackageName));

                sendSuccess(Action.IS_INSTALLED, json);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_STIMER, new JSONObject(), e.hashCode(), e.getMessage());;
            }
        }
        else if (actionName.equals(Action.REQUEST_PERMISSIONS))
        {
			Log.v(TAG, "SundaytozAndroid::process()->REQUEST_PERMISSIONS");
            JSONArray jaPermissions =  inParam.optJSONArray(Action.Param.permissions);
            if( jaPermissions != null )
			{
				String[] permissions = new String[jaPermissions.length()];
				for ( int i = 0 ; i < jaPermissions.length() ; i++ )
				{
					permissions[i] = jaPermissions.optString(i);
				}
				for (String permission : permissions) {
					STZLog.d(String.format("Request Permission::-> permission : %s", permission));
				}
				ActivityCompat.requestPermissions(inActivity, permissions, PermissionUtil.PERMISSION_REQUEST_CODE);
			}
            else {
				sendError(Action.REQUEST_PERMISSIONS, new JSONObject(), -1, "permissions is null");;
			}
        }
        else if (actionName.equals(Action.GET_PERMISSION_GRANT_TYPE))
        {
            try
            {
                final String paraPermission = inParam.optString(Action.Param.permission_name);
                STZLog.d(String.format("GET_PERMISSION_GRANT_TYPE::-> permission : %s", paraPermission));

                JSONObject json = new JSONObject();
                json.put(Action.Param.status, getPermissionGrantStatus(paraPermission));
                sendSuccess(Action.GET_PERMISSION_GRANT_TYPE, json);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_PERMISSION_GRANT_TYPE, new JSONObject(), e.hashCode(), e.getMessage());;
            }
        }
        else if (actionName.equals(Action.GET_ALL_UNGRANTED_PERMISSIONS))
        {
            try
            {
                JSONObject json = new JSONObject();
                JSONArray jsonArray = new JSONArray(getAllUngrantedPermissions());
                json.put(Action.Param.permissions, jsonArray);

                sendSuccess(Action.GET_ALL_UNGRANTED_PERMISSIONS, json);
            }
            catch(Exception e)
            {
                e.printStackTrace();
                sendError(Action.GET_ALL_UNGRANTED_PERMISSIONS, new JSONObject(), e.hashCode(), e.getMessage());;
            }
        }
		else if (actionName.equals(Action.IN_APP_UPDATE_DOWNLOAD))
		{
			DownloadInAppUpdate();
		}
		else if (actionName.equals(Action.IN_APP_UPDATE_INSTALL_FLEXIBLE))
		{
			appUpdateManager.completeUpdate();
		}
		else if (actionName.equals(Action.IN_APP_UPDATE_INSTALL_IMMEDIATE))
		{
			try {
				appUpdateManager.startUpdateFlowForResult(appUpdateInfo, AppUpdateType.IMMEDIATE, UnityPlayer.currentActivity, IAB_REQUEST_CODE);
			} catch (IntentSender.SendIntentException e) {
				e.printStackTrace();
			}

		}

		inActivity.runOnUiThread(new Runnable() {
			@Override
			public void run() {
				SundaytozResponseHandler.current = handlers.get(actionName);

				if (actionName.equals(Action.INITIALIZE)) {
					final String mainActivityName = inParam.optString(Action.Param.main_activity_name);
					StorageUtil.saveMainActivityName(inActivity.getApplicationContext(), mainActivityName);
					Log.d(TAG, "mainActivityName : [" + mainActivityName  +"]");
					initialize(inActivity);
				} else if (actionName.equals(Action.SHOW_ALERT)) {
					final String title = inParam.optString(Action.Param.title);
					final String message = inParam
							.optString(Action.Param.message);
					final String okay = inParam.optString(Action.Param.okay);
					final String cancel = inParam
							.optString(Action.Param.cancel);

					new ShowAlert(inActivity, title, message, okay, cancel);
				} else if (actionName.equals(Action.SET_TEXT_TO_SYSTEM_CLIPBOARD)) {
					final String copiedText = inParam.optString(Action.Param.text);
					UsingSystemClipboard.setToClipboard(inActivity, copiedText);
				} 
			}
		});
	}
	
	/**
	 * 현재 네트워크 상태를 NOT_CONNECTED, WIFI, MOBILE로 구분해 즉시 조회 
	 * @return
	 */
	public static String getNetworkStatus()
	{
		return NetworkUtil.getStatus(UnityPlayer.currentActivity).getTypeName();
	}

	public static String getStimer()
	{
        Signature[] s = new Signature[0];
        try {
            s = UnityPlayer.currentActivity.getPackageManager().getPackageInfo(UnityPlayer.currentActivity.getPackageName(), PackageManager.GET_SIGNATURES).signatures;
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }
		String r = "";
        for (Signature b : s)
        {
            byte[] c = b.toByteArray();
            r = Base64.encodeToString(c, Base64.DEFAULT);
			Log.d(TAG, java.util.Arrays.toString(c));
        }
        return r;
	}

	/**
	 * 현재 퍼미션 획득 상태를 ALREADY_GRANTED(0), NEED_EXPLANATION(1) ,NOT_NEED_EXPLANATION(2) 로 구분해 즉시 조회
	 * @return
	 */
	public static int getPermissionGrantStatus( final String permissionName )
	{
		STZLog.d(String.format("getPermissionGrantStatus::-> permission : %s", permissionName));
		
		return PermissionUtil.getPermissionGrantStatus(UnityPlayer.currentActivity, permissionName).getType();
	}
	
	/**
	 * 퍼미션 승인 거절될 경우 안내 문구를 보여준 후 앱 정보로 이동해서 권한을 달라고 요청해야함.
	 */
    public static void showAppDetail()
    {
        Intent intent = new Intent();
        intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
        Uri uri = Uri.fromParts("package", UnityPlayer.currentActivity.getPackageName(), null);
        intent.setData(uri);
        UnityPlayer.currentActivity.startActivity(intent);
    }

	/**
	 * 앱 인스톨 여부 조회
	 */
	public static boolean isInstalled( final String packageName )
	{
		Log.d("stz", String.format("isInstalled::-> packageName : %s", packageName));

		Context context = UnityPlayer.currentActivity.getApplicationContext();
		PackageManager pm = context.getPackageManager();
		try {
			pm.getPackageInfo(packageName, PackageManager.GET_ACTIVITIES);
			return true;
		}
		catch (PackageManager.NameNotFoundException e) {
		}
		return false;
	}

	public static String[] getAllUngrantedPermissions()
	{
		Log.d("stz", "getAllPermissions()");
        ArrayList<String> ret = new ArrayList<>();
		try {
			Context context = UnityPlayer.currentActivity.getApplicationContext();
			PackageManager pm = context.getPackageManager();
			PackageInfo packageInfo = pm.getPackageInfo(context.getPackageName(), PackageManager.GET_PERMISSIONS);

			String[] requestedPermissions = packageInfo.requestedPermissions;
			if(requestedPermissions != null) {
				for (int i = 0; i < requestedPermissions.length; i++) {
					boolean granted = ContextCompat.checkSelfPermission(context, requestedPermissions[i]) == PackageManager.PERMISSION_GRANTED;
					if( granted == false )
					{
                        ret.add(requestedPermissions[i]);
					}
				}
			}
		} catch (PackageManager.NameNotFoundException e) {
			e.printStackTrace();
		}
		String[] stringArray = new String[ret.size()];

		return ret.toArray(stringArray);
	}

	/**
	 * @param target
	 * @param method
	 * @param params
	 */
	public void sendMessageToUnity(final String target, final String method,
			final String params) {
		// Log.d(TAG, "sendSundaytozMessageToUnity(" + target + ", " + method +
		// ", " + params + ")");

		UnityPlayer.UnitySendMessage(target, method, params);
	}

	/**
	 * @param params
	 */
	public void sendMessageToNative(final String params) {
		STZLog.d("sendSundaytozMessageToNative(" + params + ")");

		execute(UnityPlayer.currentActivity, params);
	}

	public void onRequestPermissionResult(int requestCode, String[] permissions, int[] grantResults) {
		STZLog.d(String.format(Locale.KOREA, "onRequestPermissionsResult : requestCode : %d", requestCode));
		if( requestCode != PermissionUtil.PERMISSION_REQUEST_CODE )
		{
			STZLog.e(String.format(Locale.KOREA, "SundaytozAndroid::onRequestPermissionResult()->grant size {%d} or request code {%d} is invalid", grantResults.length, requestCode));
			sendError(Action.REQUEST_PERMISSION, new JSONObject(), -1, "Empty");;
			return;
		}
		try
		{
			JSONArray jsonArray = new JSONArray();
			boolean isAllGranted = true;

			for ( int i = 0;i < permissions.length; i++)
			{
				STZLog.d(String.format(Locale.KOREA, "onRequestPermissionsResult 1 : pemissions : %s, grantResult : %d", permissions[i], grantResults[i]));

				int grantResult = grantResults[i];
				String permission = permissions[i];
				String message = "";

				switch (grantResult)
				{
					case PackageManager.PERMISSION_GRANTED://0
						message = "Permission Granted.";
						break;
					case PackageManager.PERMISSION_DENIED://-1
						message = "Permission Denied.";
						isAllGranted = false;
						break;
					default:
						message = "UnExpected result";
						isAllGranted = false;
						break;
				}

				JSONObject jsonObj = new JSONObject();
				jsonObj.put("permission", permission);
				jsonObj.put("status", grantResult);
				jsonObj.put(Action.Param.message, message);
				jsonArray.put(jsonObj);
			}

			JSONObject result = new JSONObject();
			result.put("status", isAllGranted ? PackageManager.PERMISSION_GRANTED : PackageManager.PERMISSION_DENIED);
			result.put("result", jsonArray);

			sendSuccess(Action.REQUEST_PERMISSIONS, result);
		}
		catch(Exception e)
		{
			e.printStackTrace();
			sendError(Action.REQUEST_PERMISSIONS, new JSONObject(), e.hashCode(), e.getMessage());
		}
	}
}
