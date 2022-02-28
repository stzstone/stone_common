# stz-plugins-stzcommon

##폴더 설명
- sdk : 빌드 서버에 설치될 파일
- unity-plugin : 유니티 플러그인
- unity-sample : 유니티 셈플 프로젝트
- unity-sample/native-project : 안드로이드 소스 코드들을 jar로 만들기 위한 프로젝트 ( 유니티에서 바로 빌드를 하기위해서는 소스 코드들이 jar로 묶여야 함 )
- DLL_StzCommon : dll 생성을 위한 프로젝트.

## 환경
- v1.0.11 부터 2018.3.x 로 실행해야합니다. ( openUrl 함수 변경 )
- stz-build v2.0.1 필요 
  - v1.0.5 부터 2.0.1에 수정된 ios plist 처리과정이 필요합니다.
  - v1.0.8 부터 compat-support-library 26.0.1 이상 버전이 필요합니다. [참조1](https://academy.realm.io/kr/posts/android-o-preview-and-new-features/) [참조2](https://developer.android.com/reference/android/app/Notification.Builder.html#setChannelId(java.lang.String)) 이를 위해서 build.gradle 에 구글 레포지터리를 추가해야합니다. [참조](https://developer.android.com/topic/libraries/support-library/revisions.html#26-0-0)

## 1.0.x -> 1.0.8 마이그레이션
- 유니티 패키지를 임포트해서 cs 파일을 갱신하신 후에, 아래의 플랫폼별로 추가 작업을 해주세요.
- android
  - target sdk 26, compileSdk 26, buildTools 26 이상을 사용해야합니다.
  build.gradle, AndroidManifest.xml 을 확인해주세요.
  - support-library 26 이상을 사용하기 위해 구글의 메이븐 레포지터리를 통해야합니다.[참조](https://developer.android.com/topic/libraries/support-library/revisions.html#26-0-0)
  build.gradle 파일을 확인해주세요.
- ios
  - stz-build를 최신으로 업데이트해주세요. 스키마, 앱네임, 디퍼드 제스쳐 등이 추가되었습니다.

## 업데이트

###2.0.0
- dll 적용.

###1.1.2
- ios
 - 키체인 공용 조회 함수 추가. SundaytozNativeExtension.Instance.GetSharedValue("UUID", "com.sundaytoz.istovekr.joy.dev", onSuccess, onError);
 - 키체인 공용 저장 함수 추가. SundaytozNativeExtension.Instance.SetSharedValue("UUID", "com.sundaytoz.istovekr.joy.dev", "custom_data_disney_stove_1646");
   타앱에 저장한 공용 데이터를 접근할 수 있으며, 읽기만을 허용합니다. 저장할 때는 본인의 번들 아이디를 넣어주세요. 
 - 조회할 타앱과 팀아이디가 같아야 하며 팀아이디는 아래를 참조해주세요.
```
com.sundaytoz.mobile.anipang.ios.dev ZVL2TQ5RF3
com.sundaytoz.mobile.anipang.ios.service GXQE5BYCVJ
com.sundaytoz.mobile.anipang2.ios.kakao.dev VHNN2RBQ2Y
com.sundaytoz.mobile.anipang2.ios.kakao.service VHNN2RBQ2Y
com.sundaytoz.kakao.anipang3.service.dev XR2PBHV9ZX
com.sundaytoz.kakao.anipang3.service GXQE5BYCVJ
com.sundaytoz.kakao.anipangtouch.dev XR2PBHV9ZX
com.sundaytoz.kakao.anipangtouch XR2PBHV9ZX
com.sundaytoz.kakao.shanghai.dev 9ARF7XAE3E
com.sundaytoz.kakao.shanghai 9ARF7XAE3E
com.sundaytoz.istovekr.joy.dev XR2PBHV9ZX
com.sundaytoz.istovekr.joy XR2PBHV9ZX
com.sundaytoz.line.joy.dev 9ARF7XAE3E
com.sundaytoz.line.joy 9ARF7XAE3E
com.sundaytoz.kakao.wbb.dev XR2PBHV9ZX
com.sundaytoz.kakao.wbb XR2PBHV9ZX
com.sundaytoz.istove.wbb.dev XR2PBHV9ZX
com.sundaytoz.istove.wbb XR2PBHV9ZX
```
키체인 에러코드가 발생할 경우 아래 헤더 파일을 참조해주세요.
https://opensource.apple.com/source/Security/Security-55471/sec/Security/SecBasePriv.h

###1.1.1
- ios
  - 키체인 저장 함수 추가. SundaytozNativeExtension.Instance.SetKeychainValue("value");
  - 키체인 조회 함수 추가. SundaytozNativeExtension.Instance.GetKeychainValue(onSuccess, onError);
  - UUID 조회 함수 추가. SundaytozNativeExtension.Instance.GetUUIDWithKeychain("UUID", TeamID.BundleID", onSuccess, onError);

###1.1.0
- ios
  - openUrl 함수 변경으로 인해 더 이상 유니티 5.6.x 버전은 지원되지 않습니다.
- aos
  - 이 버전부터는 android-x 를 사용합니다.

###1.0.11
- ios
  - 1.0.11 은 openUrl 함수가 변경되었습니다. ( 유니티 2018.3.x 부터 지원합니다. )
  - 애플 로그인 함수 추가. ( SundaytozNativeExtension.Instance.AppleLogin("ASAuthorizationScopeFullName|ASAuthorizationScopeEmail", onSuccess, onError);//모든 정보 조회 )
  - 애플 로그아웃 함수 추가.(SundaytozNativeExtension.Instance.AppleLogout();)
  - 애플 로그인 상태 함수 추가. (SundaytozNativeExtension.Instance.AppleCheckAccount(onSuccess, onError);)
- android
  - simState 함수 추가 ( )

###1.0.10
- ios
  - apns 등록하는 함수 추가. ( SundaytozNativeExtension.Instance.RegistAPNSNotification )
  - 스키마 정보 가져오는 함수 추가. ( SundaytozNativeExtension.Instance.GetScheme, 테스트 url : stzkrjoydev://sundaytoz?action=add&token=hello&tag=ho )
- android
  - 인텐트 정보 가져오는 함수 추가. ( SundaytozNativeExtension.Instance.GetIntentData )

###1.0.9
- android
  - keystore signature 기능 추가.
  - 초기화시 build name 가져오도록 수정.

###1.0.8
- 초기화 타이밍에 디바이스 남은 용량도 전달하도록 수정.

###1.0.7
- ios 스키마 변경 ( 기존의 카카오나 페이스북에서 사용하는 스키마 삭제 )
- 안드로이드 푸시 아이콘 정리 [안드로이드 리소스](https://developer.android.com/guide/topics/resources/providing-resources.html)
  - 리소스의 크기는 다음 비율을 맞춰주세요. ( ldpi:mdpi:hdpi:xhdpi:xxhdpi:xxxhdpi = 3:4:6:8:12:16 )
  - ic_notif : 알림바 상단에 나오는 컬러 아이콘 ( xxxhdpi기준 96x96 ), SDK 21미만을 위한 아이콘
  - ic_notif_white : 알림바 상단에 나오는 흰색 아이콘 ( xxxhdpi기준 96x96 ), SDK 21이상부터 머티리얼 디자인이 적용됩니다.
  - ic_notif_large : 노티피케이션 뷰 좌측에 나오는 커다란 아이콘입니다. ( xxxhdpi기준 256x256 )
  - notif_background : 노티피케이션 뷰 배경 이미지입니다. ( xxxhdpi기준 2600x256 )
  - 킷캣 이하에서(api 20) 알림바 상단에 컬러값이 들어있는 아이콘을 사용하거나, 다른 문의가 있는 경우 플랫폼팀에 문의해주세요.
  - 로컬 푸쉬에서 html 태그가 사용가능합니다. ( gcm 은 8.4.3부터 가능합니다. )
  ```c#
  message = "설정한 <font color=#FF0000><b>알람</b>이</font> 발생했어요";
  ```
  - 이모지를 보내기 위해서는 "\U0001F610" 이런식으로 값을 넘겨주면 됩니다. 이모지는 예전 버전도 가능합니다. [사용 가능한 이모티콘](https://apps.timwhitlock.info/emoji/tables/unicode#block-2-dingbats) GCM 의 경우 이모티콘 자체를 넣으면 정상 동작합니다.

###1.0.6
- ios 백그라운드 이미지 제공
  build.config.d/ios/DEPLOY/secure_display.png 파일을 앱이 백그라운드에 들어갔을 시 보여줍니다.
```c#
SundaytozNativeExtension.Instance.EnableSecure(활성화 여부, 파일명);// 파일명을 입력하지 않을 시 "secure_display.png" 값을 기본으로 합니다.
```
- 스키마 네이밍 추가 ( stz-build )
build.config.d/config.json 에 STZ_SCHEME 값 추가( __ex : stzkakaosnoopy__ )


###1.0.5
- init에서 time zone, time zone offset 정보를 제공
- IsInstalled 함수가 추가되었습니다. 안드로이드의 경우는 패키지명으로 조회하며, ios 에서는 스키라로 조회합니다. 특히 ios 의 경우 canOpenURL을 이용하므로, ios9 부터 LSApplicationQueriesSchemes 에 등록되지 않은 스키마에 대해서는 false 만을 반환합니다. 정해진 스키마로만 호출해야합니다. [참고](https://developer.apple.com/documentation/uikit/uiapplication/1622952-canopenurl)
- CheckAllPermission() 함수가 추가되었습니다. 권한 부여가 필요한 모든 퍼미션을 '|'로 묶어서 반환합니다.
- 안드로이드 권한을 요청하는 함수가 추가되었습니다. [참고](http://gun0912.tistory.com/55)
```c#
SundaytozNativeExtension.Instance.RequestPermission( permission, (delegate(int inStatus, JSONNode inResult){
			WriteOutputText("inStatus : " + inStatus + ", inresult : " + inResult.ToString());
			switch((EPermissionResponseType)inStatus)
			{
			case EPermissionResponseType.GRANTED:
				Debug.Log ("EPermissionResponseType.GRANTED");
				//승인 될 경우 계속 진행.
				//만일 여러개의 권한을 요청한 경우 모두 승인해야만 이쪽으로 온다.
				break;
			case EPermissionResponseType.DENIED:
				//거부 될 경우 안내 팝업후 설정으로 가도록 해준다
				//만일 여러개의 권한을 요청한 경우 하나라도 거부하면 이쪽으로 온다.
				Debug.Log ("EPermissionResponseType.DENIED");
				showAppDetail();
				break;
			}
		}),
		(delegate(int inStatus, string error)
		{
			WriteOutputText("inStatus : " + inStatus + ", error : " + error);
		}));
```
- 노티피케이션의 배경이 추가되었습니다. 기본값으로 notif_background.png 파일을 사용합니다. bg_image_name 에 파일명을 전달해서 바꿀 수 있습니다.
  적용시 build.config.d/android/common/res 하위에 있는 ic_notif_large.png, ic_notif.png, notif_background.png 의 파일이 반드시 있어야 합니다. ( [이미지 크기](https://developer.android.com/guide/practices/screens_support.html?hl=ko#DesigningResources) )
  오른쪽 정렬로 되어있는 레이아웃을 바꾸고 싶으시면 layout-v15/notification_default.xml, layout-v17/notification_default.xml 의 레이아웃을 변경해주세요.

###1.0.4
- 안드로이드 런타임에 퍼미션을 확인해서 승인을 받거나, 또는 거절 되었을 경우의 호출 할 수 있는 함수들이 추가되었습니다. [사용법](https://github.com/sundaytoz/stz-plugins-stzcommon/wiki/stzcommon)
- __v1.0.5에 추가된 IsInstalled, Init time zone, time zone offset 기능이 1.0.4 에 병합하였습니다.(17.11.21)__

###1.0.3
- 시스템 클립보드에 텍스트 복사 기능 추가
- 안드로이드 6.0.1 / iOS 10.0.2 에서 동작 확인했습니다.
```c#
SundaytozNativeExtension.Instance.SetTextToSystemClipboard("복사할 텍스트");
```

###1.0.2
