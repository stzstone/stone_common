package com.sundaytoz.plugins.common.utils;

import com.sundaytoz.plugins.common.enums.EPermissionGrantType;
import android.app.Activity;
import android.content.pm.PackageManager;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;

public class PermissionUtil {
	
	public static final int PERMISSION_REQUEST_CODE = 0;
	
	/**
	 * 현재 퍼미션 획득 상태 반환
	 * 
	 * @param inContext
	 * @return
	 */
	public static EPermissionGrantType getPermissionGrantStatus(Activity inActivity, final String permission) {
		STZLog.d(String.format("CheckPermission::-> permission : %s", permission));

		EPermissionGrantType ret = EPermissionGrantType.NONE;

		if (ContextCompat.checkSelfPermission(inActivity, permission) == PackageManager.PERMISSION_GRANTED) {
			ret = EPermissionGrantType.ALREADY_GRANTED;
		} else {
			if (ActivityCompat.shouldShowRequestPermissionRationale(inActivity, permission)) {
				ret = EPermissionGrantType.NEED_EXPLANATION;
			}
			ret = EPermissionGrantType.NOT_NEED_EXPLANATION;
		}

		return ret;
	}

}
