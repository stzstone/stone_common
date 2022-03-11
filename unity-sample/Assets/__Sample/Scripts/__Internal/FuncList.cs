using System;
using System.Collections.Generic;
using System.Net;
using StzEnums;
using UnityEngine;
using Sundaytoz;

public class FuncList
{
	public static void MakeUI()
    {
        Debug.Log("Main::MakeUI()->1");
        List<string> permissions = new List<string>();
        permissions.Add("android.permission.WRITE_EXTERNAL_STORAGE");

        ApiTestEditor _ate = ApiTestEditor.Begin("NativeExtension");
        
        unityFunction(_ate);
        iosFunction();
        aosFunction();
        
        _ate.UpdateUI();
    }
    private static void unityFunction(ApiTestEditor _ate)
    {
	    _ate.SetDataAction("Send Email", () =>
			    SundaytozNativeExtension.Instance.SendEmail(
				    (isSuccess) => { Debug.Log($"Main::SendEmail()->isSuccess:{isSuccess}"); },
				    "jongwoo.park@sundaytoz.com", "title", "content"))

		    .SetDataAction("Cancel Local Notification", () =>
			    SundaytozNativeExtension.Instance.CancelLocalNotification(null, 1, 1))
		    .SetDataAction("Cancel Local Notification All", () =>
			    SundaytozNativeExtension.Instance.CancelLocalNotificationAll(null))
		    .SetDataAction("Set Text To System Clipboard", () =>
			    SundaytozNativeExtension.Instance.SetTextToSystemClipboard(null, "hello world"));


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

    private static void aosFunction()
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
}
