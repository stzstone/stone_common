package com.sundaytoz.plugins.common.utils;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Base64;

public class SharedUtil
{
	private static SharedUtil instance;
	private static final String NAME = "com.sundaytoz.android.sdk";
	
	private Context context;
	private SharedPreferences pref;
	private SharedPreferences.Editor editor;
	private String clientId;
	
	private SharedUtil(Context context, String clientId)
	{ 
		super();
		this.context = context; 
		this.clientId = clientId;
	}

	/**
	 * @param key
	 * @param value
	 */
	public boolean putString(String key, String value)
	{
		return getEditor().putString(key, value).commit();
	}

	/**
	 * @param key
	 * @return
	 */
	public String getString(String key)
	{
		return getPref().getString(key, null);
	}

	/**
	 * @param key
	 * @param value
	 */
	public boolean putBoolean(String key, boolean value)
	{
		return getEditor().putBoolean(key, value).commit();
	}

	/**
	 * @param key
	 * @return
	 */
	public boolean getBoolean(String key)
	{
		return getPref().getBoolean(key, false);
	}

	/**
	 * @param key
	 * @param value
	 */
	public boolean putLong(String key, long value)
	{
		return getEditor().putLong(key, value).commit();
	}

	/**
	 * @param key
	 * @return
	 */
	public long getLong(String key)
	{
		return getPref().getLong(key, 0L);
	}
	
	/**
	 * 
	 * @param key
	 * @return
	 * @throws Exception
	 */
	public Object getObject(String key)
	{
		try
		{
			String objStr = getPref().getString(key, null);
			
			if(null != objStr)
			{ 
				byte [] data = Base64.decode(objStr, Base64.DEFAULT);
		        ObjectInputStream ois = new ObjectInputStream(
		                                        new ByteArrayInputStream( data));
		        Object src  = ois.readObject();
		        ois.close();
		        
		        return src;
			}
		}
		catch(Exception e)
		{ 
			e.printStackTrace();
		}
		
		return null;
	}
	
	/**
	 * 
	 * @param key
	 * @param src
	 * @return
	 */
	public boolean putObject(String key, Object src)
	{ 
		try
		{
			
			ByteArrayOutputStream baos = new ByteArrayOutputStream();
	        ObjectOutputStream oos = new ObjectOutputStream(baos);
	        oos.writeObject(src);
	        oos.close();
	        
	        String objStr = Base64.encodeToString(baos.toByteArray(), Base64.DEFAULT);
	        
	        return getEditor().putString(key, objStr).commit();
	        
		}
		catch(Exception e)
		{ 
			e.printStackTrace();
		}
		
		return false;
	}

	/**
	 * @param key
	 * @return
	 */
	public boolean has(String key)
	{
		return getPref().contains(key);
	}

	public boolean remove(String key)
	{
		return getEditor().remove(key).commit();
	}

	/**
	 * 
	 * @return
	 */
	public boolean clear()
	{
		return getEditor().clear().commit();
	}
	
	/**
	 * @param context
	 * @return
	 */
	public static SharedUtil getInstance(Context context, String clientId)
	{
		if (instance == null)
		{
			synchronized (SharedUtil.class)
			{
				instance = new SharedUtil(context.getApplicationContext(), clientId);
			}
		}

		return instance;
	}
	
	/**
	 * SharedPreference.Editor instance
	 * 
	 * @return
	 */
	private SharedPreferences.Editor getEditor()
	{
		if (editor == null)
		{
			synchronized (SharedUtil.class)
			{
				editor = getPref().edit();
			}
		}
		
		return editor;
	}

	/**
	 * @return SharedPreferences instance
	 */
	private SharedPreferences getPref()
	{
		if (pref == null)
		{
			synchronized (SharedUtil.class)
			{
				pref = context.getSharedPreferences(clientId != null ? NAME + "." + clientId : NAME, Context.MODE_PRIVATE);
			}
		}
		
		return pref;
	}
}
