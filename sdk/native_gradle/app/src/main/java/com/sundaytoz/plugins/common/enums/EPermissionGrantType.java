package com.sundaytoz.plugins.common.enums;

public enum EPermissionGrantType {

	/**
	 * 퍼미션 획득 상태 초기화값.
	 */
	NONE(-1),

	/**
	 * 퍼미션 이미 승인 됨.
	 */
	ALREADY_GRANTED(0),

	/**
	 * 퍼미션 획득하기 위해 설명이 필요함.
	 */
	NEED_EXPLANATION(1),

	/**
	 * 퍼미션을 바로 요청 가능.
	 */
	NOT_NEED_EXPLANATION(2);

	private final int _code;

	EPermissionGrantType(final int inCode) {
		_code = inCode;
	}

	/**
	 * 숫자로 구성된 획득 요청 결과.
	 * 
	 * @return 숫자로 구성된 획득 요청 결과.
	 */
	public int getType() {
		return _code;
	}

	/**
	 * 퍼미션 획득 요청 결과.
	 * 
	 * @return
	 */
	public String getTypeName() {
		switch (_code) {
		case 0: {
			return "ALEADY GRANTED";
		}
		case 1: {
			return "NEED EXPLANATION";
		}
		case 2: {
			return "NOT NEED EXPLANATION";
		}
		}

		return "UNKNOWN";
	}
}
