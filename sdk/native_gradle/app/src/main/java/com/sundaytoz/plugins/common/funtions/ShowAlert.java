package com.sundaytoz.plugins.common.funtions;

import org.json.JSONObject;

import com.sundaytoz.plugins.common.SundaytozResponseHandler;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;

public class ShowAlert
{
	/* 현재 활성화된 팝업 메세지 */
	private static AlertDialog current = null;
	
	/**
	 * 시스템 팝업 메세지를 출력
	 * @param inActivity 
	 * @param inTitle 제목
	 * @param inMessage 내용
	 * @param inOkayText 확인 버튼명(0), 생략시 null
	 * @param inCancelText 취소 버튼명(1), 생략시 null
	 */
	public ShowAlert(Activity inActivity, String inTitle, String inMessage, String inOkayText, String inCancelText)
	{
		try
		{
			dismissAlerts();

			AlertDialog.Builder builder = new AlertDialog.Builder(inActivity);
		    builder.setTitle(inTitle);
		    builder.setMessage(inMessage);
		    
		    builder.setPositiveButton(inOkayText, new DialogInterface.OnClickListener()
		    {
				@Override
				public void onClick(DialogInterface arg0, int arg1)
				{
					sendOkay();
				}
			});
		    
		    if(null != inCancelText)
		    { 
		    	String[] alterBtnList = inCancelText.split(";");
		    	
		    	String btnLabel = alterBtnList[0];
		    	
		    	builder.setNegativeButton(btnLabel, new DialogInterface.OnClickListener() 
		    	{
					@Override
					public void onClick(DialogInterface arg0, int arg1)
					{
						sendCancel();
			    	}
			    });
		    	
		    	if(alterBtnList.length > 1)
		    	{ 
		    		btnLabel = alterBtnList[0];
		    		
		    		builder.setNeutralButton(btnLabel, new DialogInterface.OnClickListener()
		    		{
						@Override
						public void onClick(DialogInterface arg0, int arg1)
						{
							sendNatural();
				    	}
				    });
		    	}
		    }
		    
		    builder.create();
		    
		    current = builder.show();
		}
		catch(Exception e)
		{
			e.printStackTrace();

			sendCancel();
		}
	}
	
	/**
	 * 확인 선택 처리
	 */
	private static void sendOkay()
	{
		if(SundaytozResponseHandler.current != null)
		{
			try
			{
				JSONObject result = new JSONObject();
				result.put("button", "0");
				
				SundaytozResponseHandler.current.onComplete(1, result);
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}
		}
	}
	
	/**
	 * 취소 선택 처리
	 */
	private static void sendCancel()
	{
		if(SundaytozResponseHandler.current != null)
		{
			try
			{
				JSONObject result = new JSONObject();
				result.put("button", "1");
				
				SundaytozResponseHandler.current.onComplete(1, result);
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}
		}
	}
	
	/**
	 * 무시된 처리
	 */
	private static void sendNatural()
	{
		if(SundaytozResponseHandler.current != null)
		{
			try
			{
				JSONObject result = new JSONObject();
				result.put("button", "2");
				
				SundaytozResponseHandler.current.onComplete(1, result);
			}
			catch(Exception e)
			{
				e.printStackTrace();
			}
		}
	}
	
	/**
	 * 현재 활성화된 시스템 팝업 인스턴스
	 * @return
	 */
	public static AlertDialog getAlert()
	{
		return current;
	}
	
	/**
	 * 현재 활성화된 시스템 팝업이 있는 경우 모두 제거
	 */
	public static void dismissAlerts()
	{ 
		if(null != current && current.isShowing())
		{
			current.dismiss();
			current = null;
		}
	}
}
