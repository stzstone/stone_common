package com.sundaytoz.plugins.common;

import android.util.Log;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import com.sundaytoz.plugins.common.utils.StorageUtil;

public class SundaytozInstallReceiver extends BroadcastReceiver
{
	@Override
	public void onReceive(Context context, Intent intent)
	{
		//[__INSTALL_RECEIVER_CONTENT__]
		String referrer = intent.getStringExtra("referrer");
		StorageUtil.saveInstallReferer(context, referrer);
		Log.d("STZCommon", "onReceive() referrer: " + referrer);
	}
}
