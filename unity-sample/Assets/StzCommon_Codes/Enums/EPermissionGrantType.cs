public enum EPermissionGrantType
{
	DENIED = -1,
	GRANTED = 0,		//퍼미션 이미 승인 됨.
	NEED_EXPLANATION = 1,		//퍼미션 획득하기 위해 설명이 필요함.
	NOT_NEED_EXPLANATION = 2	//퍼미션을 바로 요청 가능.
}