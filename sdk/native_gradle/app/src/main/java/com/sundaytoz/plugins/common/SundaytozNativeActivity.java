package com.sundaytoz.plugins.common;

import android.content.Intent;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import com.sundaytoz.plugins.common.utils.StorageUtil;
import com.unity3d.player.UnityPlayerActivity;
import org.json.JSONException;
import org.json.JSONObject;

public class SundaytozNativeActivity extends UnityPlayerActivity {

	String stzSchema;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		Log.v("STZCommon", "SundaytozNativeActivity::onCreate()->v2.0.0_0406_7");
		try {
			Bundle bundle = getPackageManager().getApplicationInfo(getPackageName(), PackageManager.GET_META_DATA).metaData;
			String STZ_SCHEMA = "stzschema";
			stzSchema = bundle.getString(STZ_SCHEMA);
		} catch (PackageManager.NameNotFoundException e) {
			e.printStackTrace();
		}

		SundaytozAndroid.createInstance();

		SundaytozAndroid.instance.RefreshInAppUpdateStatus();

		Log.v("STZCommon", "SundaytozAndroid::onCreate()-> begin");
	}
	@Override
	public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults) {
		super.onRequestPermissionsResult(requestCode, permissions, grantResults);
		if (SundaytozAndroid.instance!=null){
			SundaytozAndroid.instance.onRequestPermissionResult( requestCode, permissions, grantResults);
		}
	}


	@Override
	protected void onResume() {
		super.onResume();

		Intent intent = getIntent();
		if (Intent.ACTION_VIEW.equals(intent.getAction()) && intent.getScheme().equals(stzSchema)) {
			Uri uri = intent.getData();
			JSONObject jo = new JSONObject();
			String paramValue;
			assert uri != null;
			for (String paramKey:uri.getQueryParameterNames()) {
				paramValue = uri.getQueryParameter(paramKey);
				try
				{
					jo.put(paramKey, paramValue);
				}catch ( JSONException e)
				{

				}
			}

			SundaytozAndroid.instance.RefreshInAppUpdateIsDownloaded();

			StorageUtil.saveIntentData(getApplicationContext(), jo.toString());
		}


	}
}
