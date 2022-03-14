using System;
using System.Collections.Generic;
using System.Net;
using StzEnums;
using UnityEngine;
using Sundaytoz;

public class FuncResponse
{
   private void OnResponseFreeSpaceMb(bool isSuccess, int freeSpaceMb)
	{
		ApiTestEditor.Log($"Main::OnResponseFreeSpaceMb()->isSuccess:{isSuccess}, freeSpaceMb:{freeSpaceMb}");
	}

	private void onResponseLoginAppleAccount(bool isSuccess)
	{
		ApiTestEditor.Log($"Main::onResponseLoginAppleAccount()->isSuccess:{isSuccess}");
	}

	private void OnResponseInitAppleAccount(bool isSuccess)
	{
		ApiTestEditor.Log($"Main::OnResponseInitAppleAccount()->isSuccess:{isSuccess}");
	}

	private void OnResponsewithPermissionStatusList(bool isSuccess, List<string> isPermissionGranted)
	{
		ApiTestEditor.Log($"Main::OnResponsewithPermissionStatusList()->isSuccess:{isSuccess}");

		foreach (string permission in isPermissionGranted)
		{
			ApiTestEditor.Log($"Main::OnResponsewithPermissionStatusList()->permission:{permission}");
		}
			
	}

#if UNITY_ANDROID
	private void onResponseWithPermissionStatus(bool isSuccess, EPermissionGrantType grantType)
	{
		ApiTestEditor.Log($"Main::onResponseWithPermissionStatus()->isSuccess:{isSuccess}, grantType:{grantType}");
	}
#endif

	private void OnResponseIsInstalled(bool isSuccess, bool installed)
	{
		ApiTestEditor.Log($"Main::OnResponseIsInstalled()->isSuccess:{isSuccess}, installed:{installed}");
	}

	private void OnResponseSimple(bool isSuccess)
	{
		ApiTestEditor.Log($"Main::OnResponseWithSuccess()->isSuccess:{isSuccess}");
	}

	private void OnResponseWithTextContent(bool isSuccess, string content)
	{
		ApiTestEditor.Log($"Main::OnResponseWithContent()->isSuccess:{isSuccess}, content:{content}");
	}

	private void ShowDeviceData()
	{
		var data = SundaytozNativeExtension.Instance.DeviceData;

		var log = "";

		log = log + $"Device Name:      {data.DeviceName}";
		log = log + $"\nCarrier: 		  {data.Carrier}";
		log = log + $"\nSim State: 		  {data.SimState}";
		log = log + $"\nLocal Country: 	  {data.LocalCountry}";
		log = log + $"\nNetwork Country:  {data.NetworkCountry}";
		log = log + $"\nCountry: 		  {data.Country}";
		log = log + $"\nOs Version: 	  {data.OsVersion}";
		log = log + $"\nApp Params: 	  {data.AppParams}";
		log = log + $"\nTime Offset: 	  {data.Timeoffset}";
		log = log + $"\nTimezone: 		  {data.Timezone}";
		log = log + $"\nInstall Referrer: {data.InstallReferrer}";
		log = log + $"\nVersion Name: 	  {data.VersionName}";
		log = log + $"\nFree Space Mb: 	  {data.FreeSpaceMb}";
		log = log + $"\nApi Level: 		  {data.ApiLevel}";
		log = log + $"\nInAppUpdateStatus: 		  {data.InAppUpdateStatus}";

		ApiTestEditor.Log(log);
	}

	private void InstanceOnOnChangedNetworkStatus(string networkType)
	{
		Debug.Log($"Main::InstanceOnOnChangedNetworkStatus()->networkType{networkType}");
		ApiTestEditor.Log($"Main::InstanceOnOnChangedNetworkStatus()->networkType{networkType}");
	}

	private void AddLocalNotification(int inId, int inTime, string inTitle, string inMessage, int inCounter, int inType)
	{
	}

	private void CancelLocalNotification(int inId, int inType)
	{
		
	}

	private void CancelLocalNotification()
	{
		
	}

//     /// <summary>
//     /// 디바이스 정보 출력 AddLocalNotification
//     /// </summary>
//     /// <param name="inInfo">{"carrier":"SKTelecom","device_name":"SHW-M250S","app_params":"","os_version":"4.1.2","country":"kr"}</param>
//     private void descDeviceInfo(JSONNode inInfo)
//     {
//          WriteOutputText(string.Format("device_name: {0}\ncarrier: {1}\nos_version: {2}\ncountry: {3}\napp_params: {4}\ntimezone: {5}\ntimeoffset: {6}\ninstall: {7}\nfree_space_mb: {8}\nlocal_country: {9}\nnetwork_country: {10}\nMegaByte\nuuid: {11}\nversion_name: {12}",
//             inInfo[StzNativeStringKeys.Params.device_name].Value,
//             inInfo[StzNativeStringKeys.Params.carrier].Value,
//             inInfo[StzNativeStringKeys.Params.os_version].Value,
//             inInfo[StzNativeStringKeys.Params.country].Value,
//             inInfo[StzNativeStringKeys.Params.app_params].Value,
//             inInfo[StzNativeStringKeys.Params.timezone].Value,
//             inInfo[StzNativeStringKeys.Params.timeoffset].Value,
// 	        inInfo[StzNativeStringKeys.Params.install_referer].Value,
//             inInfo[StzNativeStringKeys.Params.free_space_mb].Value,
//             #if UNITY_IOS
//             "",
//             "",
//             inInfo[StzNativeStringKeys.Params.uuid].Value,
//             ""
//             #else
//             inInfo[StzNativeStringKeys.Params.local_country].Value,
//             inInfo[StzNativeStringKeys.Params.network_country].Value,
//             SystemInfo.deviceUniqueIdentifier,
// 		    inInfo[StzNativeStringKeys.Params.version_name].Value
//             #endif
//         ));

//     }

//     /// <summary>
//     /// 테스트용 컨트롤 생성 
//     /// </summary>
//     private void setupControls()
//     {
//         GroupInfo group;

// #if UNITY_IOS
// 	    group = new GroupInfo("키체인");
// 	    {
// 		    group.AddButton("GetSharedUUID_디즈니_스토브", delegate
// 		    {
// 			    SundaytozNativeExtension.Instance.GetSharedValue("UUID", "com.sundaytoz.istovekr.joy.dev", delegate(int inStatus, JSONNode inResult)
// 			    {
// 				    WriteOutputText("GetShareKeychainValue : " + inStatus + ", inResult : " + inResult.ToString());
// 			    }, delegate(int inStatus, string error)
// 			    {
// 				    WriteOutputText("GetShareKeychainValue : " + inStatus+ ", inError : " + error);
// 			    });
// 		    });
// 		    group.AddButton("SetSharedValue", delegate
// 		    {
// 			    SundaytozNativeExtension.Instance.SetSharedValue("UUID", "com.sundaytoz.istovekr.joy.dev", "custom_data_disney_stove_1646");
// 		    });
// 		    group.AddButton("GetValue", delegate
// 		    {
// 			    SundaytozNativeExtension.Instance.GetKeychainValue(delegate(int inStatus, JSONNode inResult)
// 			    {
// 				    WriteOutputText("GetKeychainValue : " + inStatus + ", inResult : " + inResult.ToString());
// 			    }, delegate(int inStatus, string error)
// 			    {
// 				    WriteOutputText("GetKeychainValue : " + inStatus+ ", inError : " + error);
// 			    });
// 		    });
// 		    group.AddButton("SetValue", delegate
// 		    {
// 			    SundaytozNativeExtension.Instance.SetKeychainValue("custom_data_1100");
// 		    });
// 		    group.AddButton("Remove", delegate
// 		    {
// 			    SundaytozNativeExtension.Instance.SetKeychainValue("");
// 		    });
// 		    group.AddButton("UUID With TeamID", OnUUIDWithTeamID);
// 		    group.AddButton("UUID With TeamID+BundleID", OnUUIDWithTeamBundleID);
// 	    }
// 	    CreateGroup(group);
//         group = new GroupInfo("apple sign");
//         {
// 	        group.AddButton("CheckAccount", AppleCheckAccount);
// 	        group.AddButton("Login", delegate
// 	        {
// 		        AppleLogin();
// 	        });
// 	        group.AddButton("LoginWithNameProfile", delegate
// 	        {
// 		        AppleLogin("ASAuthorizationScopeFullName");
// 	        });
// 	        group.AddButton("LoginWithEmailProfile", delegate
// 	        {
// 		        AppleLogin("ASAuthorizationScopeEmail");
// 	        });
// 	        group.AddButton("LoginWithFullProfile", delegate
// 	        {
// 		        AppleLogin("ASAuthorizationScopeFullName|ASAuthorizationScopeEmail");
// 	        });
// 	        group.AddButton("Logout", AppleLogout);
//         }
//         CreateGroup(group);
// #endif
// 	    group = new GroupInfo("Get Info");
//         {
// 	        group.AddButton("StartDisneyDev", () =>
// 	        {
// 				Application.OpenURL("stzkrjoydev://sundaytoz?action=add_friend&token=hello&tag=hahaha");
// 	        });
// 	        group.AddButton("CheckDisneyDev", () =>
// 	        {
// 		        Application.OpenURL("stzkrjoydev://sundaytoz?action=add_friend&token=hello&tag=hahaha");
// 	        });
// 	        group.AddButton("Storage Space GET!", OnStorageSpaceGet);
// 	        group.AddButton("Intent Data GET!", OnIntentDataGet);
// 	        group.AddButton("Intent Data Clear!", OnIntentDataClear);
// 	        group.AddButton("ID GET!", OnPushIdGet);
// 	        group.AddButton("Signature GET!", OnSignatureGet);
// 	        group.AddButton("Get LANGUAGE", delegate {
// 		        SundaytozNativeExtension.Instance.GetLanguage(delegate(int inStatus, JSONNode inResult) {
// 			        WriteOutputText("GetLanguage get : "+inStatus+ ", inResult : " + inResult.ToString());
// 		        });
// 	        });
//         }
//         CreateGroup(group);
//         group = new GroupInfo("LocalPush");
//         {
// 	        group.AddButton("ANPS 푸시 등록", OnRegistAPNSNotification);
//             group.AddInputField("999"); // alarm id
//             group.AddInputField("app_icon"); // icon name
//             group.AddInputField("알림"); // title
//             group.AddInputField("설정한 알람이 발생했습니다."); // text
//             group.AddInputField("10"); // time(seconds)

//             group.AddButton("알람 등록", OnRegisterLocalNotification);
//             group.AddButton("알람 취소", OnUnregisterLocalNotification);
//             group.AddButton("모든 알람 취소", OnUnregisterAllLocalNotification);
//         }
//         CreateGroup(group);

// 		group = new GroupInfo("ShowAlert");
// 		{
// 			group.AddInputField("제목");
// 			group.AddInputField("내용");
// 			group.AddInputField("확인"); // 공백시 생략됨 
// 			group.AddInputField(""); // 공백시 생략됨 
			
// 			group.AddButton("시스템 팝업", OnShowAlert);
// 		}
// 		CreateGroup(group);


//         group = new GroupInfo("Network");
//         {
//             group.AddButton("현재 상태", OnGetNetworkStatus);
//             group.AddButton("상태변경 감지", OnListenNetworkChanging);
//         }
//         CreateGroup(group);

// 	    group = new GroupInfo("인스톨 여부");
// 	    {
// #if UNITY_ANDROID
// 		    group.AddInputField("com.sundaytoz.kakao.diffgame.dev");
// #else
// 		    group.AddInputField("fb245310342622069://");
// #endif
// 		    group.AddButton("인스톨 확인", OnCheckInstalled);
// 	    }
// 	    CreateGroup(group);
// #if UNITY_ANDROID
//         group = new GroupInfo("To Clipboard");
//         {
//             group.AddInputField("내용");
//             group.AddButton("복사", OnCopyToClipboard);
//         }
// 		group = new GroupInfo("권한");
// 		{
// 			group.AddInputField("권한명");
// 			group.AddButton("전체 권한 확인", OnCheckAllPermission);
// 			group.AddButton("권한 확인", OnCheckPermission);
// 			group.AddButton("다수 권한 확인", OnCheckNumberOfPermissions);
// 			group.AddButton("권한 설정", showAppDetail);
// 		}
//         CreateGroup(group);
// #endif

// #if UNITY_IOS
// 		group = new GroupInfo("StopLoading");
// 		{
// 			group.AddButton("Loading stop", StopDeviceLoading);
// 		}
// 		CreateGroup(group);

// 	    group = new GroupInfo("StartLoading");
// 	    {
// 		    group.AddButton("Loading start", StartDeviceLoading);
// 	    }
// 	    CreateGroup(group);
// #endif // UNITY_IOS
// 	    group = new GroupInfo("Etc");
// 	    {
// 		    group.AddButton("Get Scheme", GetScheme);
// 		    group.AddButton("Clear Scheme!", ClearScheme);
// 		    group.AddButton("SecureDisplay Enable", () => EnableSecureDisplay((true)));
// 		    group.AddButton("SecureDisplay Disable", () => EnableSecureDisplay((false)));
// 	    }
// 	    CreateGroup(group);
//     }

//     /// <summary>
//     /// 
//     /// </summary>
//     private void OnGetNetworkStatus()
//     {
//         WriteOutputText("Network Status: " + SundaytozNativeExtension.Instance.GetNetworkStatus() + ", isOnline: " + NetworkHelper.IsOnline().ToString());
//     }

//     private bool _listenNetworkChanging = false;
//     private void OnListenNetworkChanging()
//     {
//         _listenNetworkChanging = !_listenNetworkChanging;

//         WriteOutputText("Listen network changing: " + _listenNetworkChanging.ToString());

//         if(_listenNetworkChanging)
//         {
//             SundaytozNativeExtension.OnChangedNetworkStatus += OnChangedNetworkStatus;
//         }
//         else
//         {
//             SundaytozNativeExtension.OnChangedNetworkStatus -= OnChangedNetworkStatus;
//         }
//     }

//     private void OnChangedNetworkStatus(string inType)
//     {
//         WriteOutputText(string.Format("Network status changed to {0}", inType)); 
//     }

//     /// <summary>
//     /// 모든 로컬 푸시 메세지 취소 
//     /// </summary>
//     private void OnRegistAPNSNotification()
//     {
// 	    SundaytozNativeExtension.Instance.RegistAPNSNotification();

// 	    WriteOutputText("APNS 알람을 등록합니다!");
//     }

//     /// <summary>
//     /// 로컬 푸시 메세지 등록 
//     /// </summary>
//     private void OnRegisterLocalNotification()
//     {
//         int id = GetParamAsInt(0);

//         string iconName = GetParamAsString(1);

//         string title = GetParamAsString(2);
//         string message = GetParamAsString(3);
//         int time = GetParamAsInt(4);

//         RegisterLocalNotifications(id, time, title, message, iconName);

//         WriteOutputText(string.Format("[{0}]번 알람을 등록 했습니다.\n[{1}]초 이후에 알람이 발생합니다.", id, time));
//     }

//     /// <summary>
//     /// 로컬 푸시 메세지 취소 
//     /// </summary>
//     private void OnUnregisterLocalNotification()
//     {
//         int id = GetParamAsInt(0);

//         SundaytozNativeExtension.Instance.CancelLocalNotification(id, ELocalNotificationType.UPDATE_CURRENT);

//         WriteOutputText(string.Format("[{0}]번 알람을 취소 했습니다.", id));
//     }

//     /// <summary>
//     /// 모든 로컬 푸시 메세지 취소 
//     /// </summary>
//     private void OnUnregisterAllLocalNotification()
//     {
//         SundaytozNativeExtension.Instance.CancelLocalNotificationAll();

//         WriteOutputText("모든 알람을 취소 했습니다!");
//     }

//     /// <summary>
//     /// 시스템 클립보드로 텍스트 복사
//     /// </summary>
//     private void OnCopyToClipboard()
//     {
//         string text = GetParamAsString(0);
//         CopyToClipboard(text);
//     }
// 	/// <summary>
// 	/// 권한 확인 요청
// 	/// 가능한 인자 확인 : https://developer.android.com/guide/topics/security/permissions.html?hl=ko#perm-groups
// 	/// </summary>
// 	private void OnCheckInstalled()
// 	{
// 		Debug.Log("OnCheckInstalled");
// 		string packageName = GetParamAsString(0);
		
// 		Debug.LogFormat("OnCheckInstalled()-> packageName : {0}", packageName);

// 		if (string.IsNullOrEmpty(packageName))
// 		{
// #if UNITY_ANDROID
// 			packageName = "com.sundaytoz.kakao.diffgame.dev";
// #else
// 			packageName = "fb245310342622069://";//백야드
// #endif
// 		}
// 		bool isInstalled = SundaytozNativeExtension.Instance.IsInstalled(packageName);

// 		WriteOutputText(string.Format("{0} is {1}", packageName, isInstalled));
// 	}
	
// 	/// <summary>
// 	/// 전체 권한 확인 요청
// 	/// </summary>
// 	private void OnCheckAllPermission()
// 	{
// 		Debug.Log("OnCheckPermission(1)->");

// 		string permissionAll = SundaytozNativeExtension.Instance.CheckAllPermission();

// 		Debug.LogFormat("OnCheckPermission(2)->{0}", permissionAll);
		
// 		RequestPermission(permissionAll);
// 	}

// 	/// <summary>
// 	/// 권한 확인 요청
// 	/// 가능한 인자 확인 : https://developer.android.com/guide/topics/security/permissions.html?hl=ko#perm-groups
// 	/// </summary>
// 	private void OnCheckPermission()
// 	{
// 		Debug.Log("OnCheckPermission");

// 		string permission = "android.permission.WRITE_EXTERNAL_STORAGE";

// 		EPermissionGrantType permissionGrantType = SundaytozNativeExtension.Instance.CheckPermission ( permission );

// 		Debug.Log (permission + " state is " + permissionGrantType);

// 		switch (permissionGrantType) {
// 			case EPermissionGrantType.ALREADY_GRANTED:
// 				break;
// 			case EPermissionGrantType.NEED_EXPLANATION:
// 				//이 경우는 게임상의 팝업으로 권한이 필요한 이유를 설명한 후에 퍼미션을 요청하시기 바랍니다.
// 				RequestPermission (permission);
// 				break;
// 			case EPermissionGrantType.NOT_NEED_EXPLANATION:
// 				RequestPermission (permission);
// 				break;
// 		}
// 	}

// 	/// <summary>
// 	/// 여러개의 권한 확인 요청
// 	/// 여러개의 권한을 한 번에 요청 할 경우 각각을 '|'로 묶어서 요청한다
// 	/// </summary>
// 	private void OnCheckNumberOfPermissions()
// 	{
// 		Debug.Log("OnCheckNumberOfPermissions");

// 		string permissionRead = "android.permission.READ_EXTERNAL_STORAGE";
// 		string permissionWrite = "android.permission.WRITE_EXTERNAL_STORAGE";

// 		EPermissionGrantType permissionReadGrantType = SundaytozNativeExtension.Instance.CheckPermission ( permissionRead );
// 		EPermissionGrantType permissionWriteGrantType = SundaytozNativeExtension.Instance.CheckPermission ( permissionWrite );

// 		Debug.Log (permissionRead + " state is " + permissionReadGrantType);
// 		Debug.Log (permissionWrite + " state is " + permissionWriteGrantType);

// 		string requestString = string.Format("{0}|{1}", permissionRead, permissionWrite);
// 		if (permissionReadGrantType == EPermissionGrantType.ALREADY_GRANTED &&
// 		    permissionWriteGrantType == EPermissionGrantType.ALREADY_GRANTED)
// 		{
// 			Debug.Log("이미 두 퍼미션을 모두 획득했어요");
// 		}
// 		else
// 		{
// 			RequestPermission (requestString);
// 		}
			
// 	}

// 	/// <summary>
// 	/// 권한 획득 요청
// 	/// </summary>
// 	private void RequestPermission( string permission )
// 	{
// 		SundaytozNativeExtension.Instance.RequestPermission( permission, (delegate(int inStatus, JSONNode inResult){
// 			WriteOutputText("inStatus : " + inStatus + ", inresult : " + inResult.ToString());
// 			switch((EPermissionResponseType)inStatus)
// 			{
// 			case EPermissionResponseType.GRANTED:
// 				Debug.Log ("EPermissionResponseType.GRANTED");
// 				//승인 될 경우 계속 진행.
// 				//만일 여러개의 권한을 요청한 경우 모두 승인해야만 이쪽으로 온다.
// 				break;
// 			case EPermissionResponseType.DENIED:
// 				//거부 될 경우 안내 팝업후 설정으로 가도록 해준다
// 				//만일 여러개의 권한을 요청한 경우 하나라도 거부하면 이쪽으로 온다.
// 				Debug.Log ("EPermissionResponseType.DENIED");
// 				showAppDetail();
// 				break;
// 			}
// 		}),
// 		(delegate(int inStatus, string error)
// 		{
// 			WriteOutputText("inStatus : " + inStatus + ", error : " + error);
// 		}));
// 	}

// 	private void showAppDetail()
// 	{
// 		SundaytozNativeExtension.Instance.showAppDetail ();
// 	}


// 	/// <summary>
//     /// 시스템 팝업 출력 
//     /// </summary>
//     private void OnShowAlert()
//     {
//         string title = GetParamAsString(0);
//         string message = GetParamAsString(1);
//         string okay = GetParamAsString(2);
//         string cancel = GetParamAsString(3, null, false);

//         ShowAlert(title, message, (okay != null && okay.Length > 0 ? okay : null), (cancel != null && cancel.Length > 0 ? cancel : null));
//     }

// 	private void OnPushIdGet()
// 	{
// 		SundaytozNativeExtension.Instance.GetPushId((delegate(int inStatus, JSONNode inResult)
// 			{
// 				WriteOutputText("PushId get : "+inStatus+ ", inResult : " + inResult.ToString());
// 			}), 
// 			(delegate(int inStatus, string error)
// 			{
// 				WriteOutputText("PushId get : "+inStatus+ ", inError : " + error);
// 			}));
// 	}

// 	private void OnIntentDataGet()
// 	{
// 		WriteOutputText("OnIntentDataGet");
// 		SundaytozNativeExtension.Instance.GetIntentData((delegate(int inStatus, JSONNode inResult)
// 			{
// 				WriteOutputText("IntentData : "+inStatus+ ", inResult : " + inResult.ToString());
// 			}), 
// 			(delegate(int inStatus, string error)
// 			{
// 				WriteOutputText("IntentData : "+inStatus+ ", inError : " + error);
// 			}));
// 	}
// 	private void OnIntentDataClear()
// 	{
// 		WriteOutputText("OnIntentDataClear");
// 		SundaytozNativeExtension.Instance.ClearIntentData();
// 	}

// 	private void OnStorageSpaceGet()
// 	{
// 		WriteOutputText("OnStorageSpaceGet");
// 		SundaytozNativeExtension.Instance.GetStorageSpace((delegate(int inStatus, JSONNode inResult)
// 			{
// 				WriteOutputText("StorageSpace : "+inStatus+ ", inResult : " + inResult.ToString());
// 			}), 
// 			(delegate(int inStatus, string error)
// 			{
// 				WriteOutputText("StorageSpace : "+inStatus+ ", inError : " + error);
// 			}));
// 	}


// 	private void OnSignatureGet()
// 	{
// 		string signature = SundaytozNativeExtension.Instance.GetSignature();
//         WriteOutputText("Keystore signature : "+signature);
// 	}

//     /// <summary>
//     /// 시스템 클립보드로 텍스트 복사
//     /// <param name="inText"></param>
//     /// </summary>
//     private void CopyToClipboard(string inText)
//     {
//         SundaytozNativeExtension.Instance.SetTextToSystemClipboard(inText);
//     }

//     /// <summary>
//     /// 시스템 팝업 출력 
//     /// </summary>
//     /// <param name="inTitle"></param>
//     /// <param name="inMessage"></param>
//     /// <param name="inOkay"></param>
//     /// <param name="inCancel"></param>
//     private void ShowAlert(string inTitle, string inMessage, string inOkay = null, string inCancel = null)
//     {
//         SundaytozNativeExtension.Instance.ShowAlert(inTitle, inMessage
//             , inOkay        // 생략 가능, null
//             , inCancel      // 생략 가능, null
//             , delegate() { WriteOutputText(string.Format("[{0}] 선택됨!", inOkay)); }   // 생략 가능, null
//             , delegate () { WriteOutputText(string.Format("[{0}] 선택됨!", inCancel)); }  // 생략 가능, null
//             );
//     }

//     /// <summary>
//     /// 로컬 푸시 메세지(들) 등록 
//     /// </summary>
//     /// <returns>등록된 알람 고유코드</returns>
//     private void RegisterLocalNotifications(int inId, int inTime, string inTitle, string inMessage, string inIconName)
//     {
// 		int alarmType = ELocalNotificationType.UPDATE_CURRENT;
// 		int counter = 0;

// 		string title = string.Format ("[{0}]{1}", inId, inTitle);

//         SundaytozNativeExtension.Instance.AddLocalNotification (inId, inTime, title, inMessage, counter, alarmType,"notif_background","#000000","#000000", 30, 15);
// 	}
// #if UNITY_IOS
//     private void AppleCheckAccount()
// 	{
// 		WriteOutputText("Apple CheckAccount");
// 		SundaytozNativeExtension.Instance.AppleCheckAccount(delegate(int inStatus, JSONNode inResult)
// 		{
// 			WriteOutputText("Apple Check Account : " + inStatus + ", inResult : " + inResult.ToString());
// 		}, delegate(int inStatus, string error)
// 		{
// 			WriteOutputText("Apple Check Account : " + inStatus+ ", inError : " + error);
// 		});
// 	}

//     private void AppleLogin(string profileScope = "")
// 	{
// 		WriteOutputText("Apple Login()->profileScope : " + profileScope);
// 		SundaytozNativeExtension.Instance.AppleLogin(profileScope, delegate(int inStatus, JSONNode inResult)
// 		{
// 			WriteOutputText("Apple Login : " + inStatus + ", inResult : " + inResult.ToString());
// 		}, delegate(int inStatus, string error)
// 		{
// 			WriteOutputText("Apple Login : " + inStatus+ ", inError : " + error);
// 		});
// 	}
// 	private void AppleLogout()
// 	{
// 		WriteOutputText("Apple Logout");
// 		SundaytozNativeExtension.Instance.AppleLogout();
// 	}
// 	private void OnUUIDWithTeamID()
// 	{
// 		WriteOutputText("OnUUIDWithTeamID");
		
// 		SundaytozNativeExtension.Instance.GetUUIDWithKeychain("UUID", "9ARF7XAE3E.", (delegate(int inStatus, JSONNode inResult)
// 			{
// 				WriteOutputText("UUIDWithKeychain : "+inStatus+ ", inResult : " + inResult.ToString());
// 			}), 
// 			(delegate(int inStatus, string error)
// 			{
// 				WriteOutputText("UUIDWithKeychain : "+inStatus+ ", inError : " + error);
// 			}));
// 	}

// 	private void OnUUIDWithTeamBundleID()
// 	{
// 		WriteOutputText("OnUUIDWithTeamBundleID");

// 		SundaytozNativeExtension.Instance.GetUUIDWithKeychain("UUID", "9ARF7XAE3E.com.sundaytoz.line.joy", (delegate(int inStatus, JSONNode inResult)
// 			{
// 				WriteOutputText("OnUUIDWithTeamBundleID : "+inStatus+ ", inResult : " + inResult.ToString());
// 			}), 
// 			(delegate(int inStatus, string error)
// 			{
// 				WriteOutputText("OnUUIDWithTeamBundleID : "+inStatus+ ", inError : " + error);
// 			}));
// 	}
// #endif
// 	private void StartDeviceLoading()
// 	{
// #if UNITY_IOS
// 		SundaytozNativeExtension.Instance.StartLoading ();
// 		WriteOutputText("startLoading");
// #endif
// 	}

// 	private void StopDeviceLoading()
// 	{
// #if UNITY_IOS
// 		SundaytozNativeExtension.Instance.StopLoading ();
// 		WriteOutputText("stopLoading");
// #endif
// 	}

// 	private void EnableSecureDisplay(bool isEnable)
// 	{
// 		SundaytozNativeExtension.Instance.EnableSecure(isEnable);
// 		WriteOutputText(string.Format("EnableSecureDisplay : {0}", isEnable));
// 	}

// 	private void GetScheme()
// 	{
// 		SundaytozNativeExtension.Instance.GetScheme(delegate(int status, JSONNode inResult)
// 		{
			
// 			string statusFormat = string.Format("GetScheme()complete->status:{0}", status);
// 			Debug.Log(statusFormat);
// 			WriteOutputText(statusFormat);

// 			if (inResult != null && inResult.ContainsKey("GET_SCHEME"))
// 			{
// 				string intentData = inResult["GET_SCHEME"];
// 				WriteOutputText(string.Format("GetScheme()->status:{0}, result:{1}", status, intentData));
// 			}
// 		}, delegate(int status, string message)
// 		{
// 			Debug.LogFormat("error()->status:{0}", status);
// 			WriteOutputText("error");
// 		});
// 	}
// 	private void ClearScheme()
// 	{
// 		SundaytozNativeExtension.Instance.ClearScheme();
// 	}

// 	private void GetIntentData()
// 	{
// 		SundaytozNativeExtension.Instance.GetIntentData(
// 			delegate (int inStatus, JSONNode inResult)
// 			{
// 				Debug.Log($"SocialController::inStatus:{inStatus}, inResult:{inResult}");

// 				if (inResult!= null && inResult.ContainsKey("intent_data"))
// 				{
// 					string intentData = inResult["intent_data"];
					
// 					Debug.Log(intentData);
// 				}
// 			},
// 			delegate (int inStatus, string inMessage)
// 			{
// 				return;
// 			});
// 	}
}