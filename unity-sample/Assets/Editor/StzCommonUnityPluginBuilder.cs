using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;
using PlistCS;
using Debug = UnityEngine.Debug;

public class StzCommonUnityPluginBuilder : MonoBehaviour
{
	[PostProcessBuild]
	static void onPostProcessBuild (BuildTarget buildTarget, string path)
	{
		UnityEngine.Debug.Log ("PostProcessBuild StzCommon Unity SDK Start 12.05");

#if UNITY_5
		if( buildTarget != BuildTarget.iOS ) {
#else
		if ( buildTarget != BuildTarget.iOS ) {
#endif
			return;
		}

#if UNITY_IOS
        string projectPath = Path.Combine (path, "Unity-iPhone.xcodeproj/project.pbxproj");

		PBXProject proj = new PBXProject ();
		proj.ReadFromString (File.ReadAllText (projectPath));

		string target = PBXProject.GetUnityTargetName ();
		target = proj.TargetGuidByName (target);

		proj.AddFrameworkToProject(target,"CoreTelephony.framework",false);
		proj.AddFrameworkToProject(target,"Security.framework",false);
		proj.AddFrameworkToProject(target,"AuthenticationServices.framework",false);
		proj.AddFrameworkToProject(target,"iAd.framework",false);
		proj.AddFrameworkToProject(target,"AdSupport.framework",false);
		proj.AddFrameworkToProject(target,"AppTrackingTransparency.framework",false);
		proj.AddFrameworkToProject(target,"MessageUI.framework",false);
		proj.AddFrameworkToProject(target,"UserNotifications.framework",false);

		//proj.AddCapability(target, PBXCapabilityType.AppleSignin);
		string entitlementFilePath = Path.Combine(Application.dataPath, "Plugins", "iOS", "dev.entitlements");
		if(File.Exists(entitlementFilePath))
			proj.AddCapability(target, PBXCapabilityType.KeychainSharing, entitlementFilePath);
		proj.SetBuildProperty(target, "GCC_ENABLE_OBJC_EXCEPTIONS", "YES");
		proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "Apple Development: dongha shin (RV45P6KWC2)");
		proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY[sdk=iphoneos*]", "Apple Development: dongha shin (RV45P6KWC2)");
		proj.SetBuildProperty(target, "SWIFT_VERSION", "4.0");
		
		File.WriteAllText (projectPath, proj.WriteToString ());
		string appController = Path.Combine(path, "Libraries/Plugins/iOS/Classes/StzcommonAppController.mm");
		if (File.Exists(appController))
		{
			File.WriteAllText(appController, File.ReadAllText(appController).Replace(@"[super application:app openURL:url options:options];", "//[super application:app openURL:url options:options];"));
		}
		
		string infoPlistPath = Path.Combine(path , "Info.plist");

		PlistDocument plistDoc = new PlistDocument();
		plistDoc.ReadFromFile(infoPlistPath);
		if (plistDoc.root != null)
		{
			plistDoc.root.CreateArray("LSApplicationQueriesSchemes").AddString("stzkrjoydev");
			plistDoc.root.CreateArray("CFBundleURLTypes").AddDict().CreateArray("CFBundleURLSchemes").AddString("stzkrjoydev");
			plistDoc.root.SetString("AppIdentifierPrefix", "$(AppIdentifierPrefix)");
			plistDoc.WriteToFile(infoPlistPath);
		}
		else {
			UnityEngine.Debug.LogError("ERROR: Can't open " + infoPlistPath);
		}

		UnityEngine.Debug.Log ("PostProcessBuild StzCommon Unity SDK Done");
#endif
	}
}