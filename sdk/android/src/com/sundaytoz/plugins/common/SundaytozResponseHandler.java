package com.sundaytoz.plugins.common;

import org.json.JSONObject;

import android.content.Context;

public class SundaytozResponseHandler
{
	/* 현재 활성화된 콜백 핸들러 */
	public static SundaytozResponseHandler current = null;
	
	/*  */ 
    private Context _context = null;

	/**
	 * @param inContext
	 */
	public SundaytozResponseHandler(Context inContext)
	{
		_context = inContext;
	}
	
	/**
	 * 
	 */
	public void onStart()
	{
	}
	
	/**
	 * @param inStatusCode
	 * @param inResult
	 */
	public void onComplete(int inStatusCode, JSONObject inResult)
	{
	}
	
	/**
	 * @param inStatusCode
	 * @param inResult
	 */
	public void onError(int inStatusCode, JSONObject inResult)
	{
	}
}
