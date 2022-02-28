using UnityEngine;
using STZ_Common;

public abstract class SundaytozPluginBase : ScriptableObject
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inCall"></param>
	abstract public void request(IStzNativeCall inCall);

    /// <summary>
    /// 현재 네트워크 상태 반환 
    /// </summary>
    /// <returns></returns>
    abstract public string GetNetworkStatus();

    // /// <summary>
    // /// 키스토어 시그니쳐 값 반환 
    // /// </summary>
    // /// <returns></returns>
    // abstract public string GetSignature();

	// /// <summary>
	// /// 현재 퍼미션 획득 상태 반환
	// /// </summary>
	// /// <returns>The permission grant status.</returns>
	// /// <param name="permissionName">Permission name.</param>
	// abstract public EPermissionGrantType GetPermissionGrantStatus (string permissionName);

	// /// <summary>
	// /// 전체 퍼미션 중 획득이 필요한 퍼미션 리턴
	// /// </summary>
	// /// <returns>PERMISSION_A|PERMISSION_B</returns>
	// /// <param name="permissionName">Permission name.</param>
	// abstract public string GetAllPermissions ();

	// /// <summary>
	// /// 앱 정보 화면으로 유도
	// /// </summary>
	// abstract public void showAppDetail ();

	// /// <summary>
	// /// 앱 인스톨 여부
	// /// </summary>
	// abstract public bool IsInstalled(string packageName);
}