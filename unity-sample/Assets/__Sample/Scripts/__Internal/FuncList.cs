using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sundaytoz;

public static class FuncList
{
	static STZq<String> stzq = new STZq<string>();
	
	public static void MakeUI()
	{
		Debug.Log("Main::MakeUI()->1");
		List<string> permissions = new List<string>();
		permissions.Add("android.permission.WRITE_EXTERNAL_STORAGE");
		ApiTestEditor _apiTestEditor = ApiTestEditor.Begin("NativeExtension");
		_apiTestEditor
			/*.SetDataAction("Send Email", () =>
				SundaytozNativeExtension.Instance.SendEmail(
					(isSuccess) => { Debug.Log($"Main::SendEmail()->isSuccess:{isSuccess}"); },
					"jongwoo.park@sundaytoz.com", "title", "content"))*/

			.SetDataAction("Send Email", () =>
				
				stzq.Push("SendEmail")
					)

			.SetDataAction("Add Local Notification",
				() => SundaytozNativeExtension.Instance.AddLocalNotification(
					(success) => { ApiTestEditor.Log($"Main::AddLocalNotification()->success:{success}"); }, 1, 5,
					"title", "message", 1, 1))
			.SetDataAction("Cancel Local Notification", () =>
				SundaytozNativeExtension.Instance.CancelLocalNotification(null, 1, 1))
			.SetDataAction("Cancel Local Notification All", () =>
				SundaytozNativeExtension.Instance.CancelLocalNotificationAll(null))
			.SetDataAction("Set Text To System Clipboard", () =>
				SundaytozNativeExtension.Instance.SetTextToSystemClipboard(null, "hello world"))

#if UNITY_ANDROID

#elif UNITY_IOS

			.SetDataAction("Regist APNS Notification", () =>
				SundaytozNativeExtension.Instance.RegistAPNSNotification(null))
#endif

			.End();




		_apiTestEditor.UpdateUI();
	}

	private static void unityFunction()
	{

		//----------------------------------------------------------------
		// <1> Non waiting void func
		//----------------------------------------------------------------
		//----------------------------------------------------------------
		// <2> Non waiting <T> func
		//----------------------------------------------------------------
		//----------------------------------------------------------------
		// <3> Listner func
		//----------------------------------------------------------------

	}


	private static void iosFunction()
	{


	}

	private static void aosFunction(ApiTestEditor _apiTestEditor)
	{
		/*_apiTestEditor
			 .SetDataAction("Is Installed", () =>
				 SundaytozNativeExtension.Instance.IsInstalled(OnResponseIsInstalled, "com.sundaytoz.kakao.wbb"))
			 .SetDataAction("Show Alert", () =>
				 SundaytozNativeExtension.Instance.ShowAlert(null, "title", "message"))
			 .SetDataAction("GetSignature", () =>
				 SundaytozNativeExtension.Instance.GetSignature(OnResponseWithTextContent))
			 .SetDataAction("GetPermissionGrantType", () =>
				 SundaytozNativeExtension.Instance.GetPermissionGrantType(onResponseWithPermissionStatus, "android.permission.WRITE_EXTERNAL_STORAGE"))
			 .SetDataAction("GetAllUngrantedPermissions", () =>
				 SundaytozNativeExtension.Instance.GetAllUngrantedPermissions(OnResponsewithPermissionStatusList))
			 .SetDataAction("RequestPermissions1", () =>
				 SundaytozNativeExtension.Instance.RequestPermission((isSuccess, permissionList) =>
				 {
					 ApiTestEditor.Log($"Main::RequestPermissions(1)->isSuccess:{isSuccess}, permissionList is {(permissionList == null ? "null" : "not null")}" );
					 
					 if (permissionList == null) return;
					 
					 for( var i = 0 ; i < permissionList.Count ; i++)
					 {
						 ApiTestEditor.Log($"Main::RequestPermissions(1)->i:{i}, status:{permissionList[i]}");
					 }
				 }, permissions))
			 .SetDataAction("RequestPermissions2", () =>
				 SundaytozNativeExtension.Instance.RequestPermission((isSuccess, permissionList) =>
				 {
					 ApiTestEditor.Log($"Main::RequestPermissions(2)->isSuccess:{isSuccess}");
					 for( var i = 0 ; i < permissionList.Count ; i++)
					 {
						 ApiTestEditor.Log($"Main::RequestPermissions(2)->i:{i}, status:{permissionList[i]}");
					 }
				 }, null))
			 .SetDataAction("ClearInstallReferer", () =>
				 SundaytozNativeExtension.Instance.ClearInstallReferrer(OnResponseSimple))*/
	}


}