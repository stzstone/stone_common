using System;
using System.Collections.Generic;
using System.Net;
using StzEnums;
using UnityEngine;
using Sundaytoz;

public class Main : MonoBehaviour
{
    private void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        EPlatformType platform = EPlatformType.Unknown;

#if UNITY_ANDROID && !UNITY_EDITOR
		platform = EPlatformType.Android;
#elif UNITY_IOS && !UNITY_EDITOR
		platform = EPlatformType.iOS;
#elif UNITY_EDITOR
        platform = EPlatformType.UnityEditor;
#endif
        Debug.Log($"Main::Initialize()->version:{Application.version}");

        SundaytozNativeExtension.Instance.Initialize((success) =>
        {			
            FuncList.MakeUI();
#if UNITY_ANDROID && !UNITY_EDITOR
			SundaytozNativeExtension.OnChangedNetworkStatus += InstanceOnOnChangedNetworkStatus;
#endif

        }, "com.sundaytoz.kakao.joy.SundaytozNativeActivity", platform);
    }
    
}