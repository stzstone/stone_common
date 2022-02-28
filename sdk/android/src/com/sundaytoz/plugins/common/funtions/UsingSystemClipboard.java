package com.sundaytoz.plugins.common.funtions;

import android.app.Activity;
import android.content.Context;
import android.util.Log;
import android.widget.Toast;

public class UsingSystemClipboard 
{
	private static final String TAG = "UsingSystemClipboard";
	
	public static void setToClipboard(Activity inActivity, String text)
	{
		Context context = inActivity.getApplicationContext();
		
		if (context == null)
			return;
		
		try {
			if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.HONEYCOMB) {
		        android.text.ClipboardManager clipboard = (android.text.ClipboardManager) context.getSystemService(Context.CLIPBOARD_SERVICE);
		        clipboard.setText(text);
		    } else {
		        android.content.ClipboardManager clipboard = (android.content.ClipboardManager) context.getSystemService(Context.CLIPBOARD_SERVICE);
		        android.content.ClipData clip = android.content.ClipData.newPlainText("Copied Text", text.toString());
		        clipboard.setPrimaryClip(clip);
		    }			
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	public static String getFromClipboard(Activity inActivity)
	{
		Context context = inActivity.getApplicationContext();
		
		if (context == null)
			return null;
		
		try {
			if(android.os.Build.VERSION.SDK_INT < android.os.Build.VERSION_CODES.HONEYCOMB) {
		        android.text.ClipboardManager clipboard = (android.text.ClipboardManager) context.getSystemService(Context.CLIPBOARD_SERVICE);
		        return clipboard.getText().toString();
		    } else {
		        android.content.ClipboardManager clipboard = (android.content.ClipboardManager) context.getSystemService(Context.CLIPBOARD_SERVICE);
		        return clipboard.getPrimaryClip().getItemAt(0).toString();
		    }	
		} catch (Exception e) {
			e.printStackTrace();
			return null;
		}
		
	}

}
