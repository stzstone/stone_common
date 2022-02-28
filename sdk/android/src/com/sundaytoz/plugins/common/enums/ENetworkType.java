package com.sundaytoz.plugins.common.enums;

public enum ENetworkType
{
	/**
	 * 오프라인
	 */
	NOT_CONNECTED(-1),
	
	/**
	 * WIFI
	 */
	WIFI(1),
	
	/**
	 * 3G/LTE
	 */
	MOBILE(2);
	
	private final int _code;

	ENetworkType(final int inCode)
	{
	   _code = inCode;
	}

	/**
	* 숫자로 구성된 에러코드
	* @return 숫자로 구성된 에러코드
	*/
	public int getType()
	{
		return _code;
	}
	
	/**
	 * 네트워크 상태명
	 * @return
	 */
	public String getTypeName()
	{
		switch(_code)
		{
		case -1: { return "NOT_CONNECTED"; }
		case 1: { return "WIFI"; }
		case 2: { return "MOBILE"; }
		}
		
		return "UNKNOWN";
	}
}
