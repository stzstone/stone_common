//
//  CommonStringKey.h
//  StzUnityCommonPlugin
//
//  Created by test user on 2015. 8. 14..
//  Copyright (c) 2015년 Sundaytoz. All rights reserved.
//

#import <Foundation/Foundation.h>

#ifndef StzUnityCommonPlugin_CommonStringKey_h
#define StzUnityCommonPlugin_CommonStringKey_h

//Action
static NSString *ENABLE_SECURE_DISPLAY   = @"ENABLE_SECURE_DISPLAY";
static NSString *COMMON_SET_TEXT_TO_SYSTEM_CLIPBOARD   = @"SET_TEXT_TO_SYSTEM_CLIPBOARD";
static NSString *COMMON_LOCAL_NOTIFICATION_ADD         = @"LOCAL_NOTIFICATION_ADD";
static NSString *COMMON_LOCAL_NOTIFICATION_CANCEL      = @"LOCAL_NOTIFICATION_CANCEL";
static NSString *COMMON_LOCAL_NOTIFICATION_ALL_CANCEL  = @"LOCAL_NOTIFICATION_ALL_CANCEL";
static NSString *COMMON_APNS_NOTIFICATION_REGIST  = @"APNS_NOTIFICATION_REGIST";
static NSString *COMMON_GET_PUSH_ID  = @"GET_PUSH_ID";
static NSString *COMMON_GET_SCHEME_DATA = @"GET_SCHEME_DATA";
static NSString *COMMON_CLEAR_SCHEME_DATA = @"CLEAR_SCHEME_DATA";
static NSString *COMMON_GET_FREE_SPACE_MB = @"GET_FREE_SPACE_MB";
static NSString *COMMON_UUID_WITH_KEYCHAIN = @"UUID_WITH_KEYCHAIN";

static NSString *COMMON_IS_INSTALLED = @"IS_INSTALLED";

static NSString *COMMON_GET_LAUNCH_URL  = @"GET_LAUNCH_URL";

static NSString *COMMON_GET_LANGUAGE = @"GET_LANGUAGE";
static NSString *COMMON_GET_COUNTRY = @"GET_COUNTRY";

static NSString *COMMON_START_DEVICE_LOADING  = @"START_DEVICE_LOADING";
static NSString *COMMON_STOP_DEVICE_LOADING  = @"STOP_DEVICE_LOADING";

static NSString *COMMON_INIT_APPLE_ACCOUNT  = @"INIT_APPLE_ACCOUNT";
static NSString *COMMON_APPLE_LOGIN  = @"LOGIN_APPLE_ACCOUNT";
static NSString *COMMON_APPLE_LOGOUT  = @"LOGOUT_APPLE_ACCOUNT";
static NSString *COMMON_GET_KEYCHAIN_VALUE  = @"GET_KEYCHAIN_VALUE";
static NSString *COMMON_SET_KEYCHAIN_VALUE  = @"SET_KEYCHAIN_VALUE";
static NSString *COMMON_GET_SHARED_VALUE  = @"GET_SHARED_VALUE";
static NSString *COMMON_SET_SHARED_VALUE  = @"SET_SHARED_VALUE";
static NSString *COMMON_RISE_CRASH  = @"RISE_CRASH";
static NSString *COMMON_SET_RESERVED_WORD  = @"SET_RESERVED_WORD";
static NSString *COMMON_GET_AND_CLEAR_DEEPLINK  = @"GET_AND_CLEAR_DEEPLINK";
static NSString *COMMON_OPEN_APP_DETAIL  = @"OPEN_APP_DETAIL";

static NSString *COMMON_ACTION                    = @"action";
static NSString *COMMON_RESULT                    = @"result";
static NSString *COMMON_DATA                      = @"data";

static NSString *COMMON_STATUS_CODE               = @"status";
static NSString *COMMON_ERROR                     = @"error";
static NSString *COMMON_ERROR_CODE                = @"error_code";
static NSString *COMMON_ERROR_MSG                 = @"error_msg";

static NSString *COMMON_STATUS_MESSAGE            = @"message";

static NSString *COMMON_INITIALIZE                = @"INITIALIZE";


static NSString *COMMON_SUCCESS_MSG                = @"OK";
static NSString *COMMON_FAIL_MSG                   = @"FAIL";

//noti check
static NSString *COMMON_NOTI_ID                     = @"alarm_id";
static NSString *COMMON_TIME                        = @"time";
static NSString *COMMON_TEXT                        = @"text";
static NSString *COMMON_MESSAGE                     = @"message";
static NSString *COMMON_COUNTER                     = @"counter";

static NSString *COMMON_REGISTRATION_ID             = @"registration_id";

static NSString *COMMON_LANGUAGE                    = @"language";

static NSString *COMMON_DEVICE_NAME                 = @"device_name";
static NSString *COMMON_CARRIER                     = @"carrier";
static NSString *COMMON_OS_VERSION                  = @"os_version";
static NSString *COMMON_COUNTRY                     = @"country";
static NSString *COMMON_APP_PARAM                   = @"app_params";
static NSString *COMMON_API_LEVEL                   = @"api_level";

static NSString *COMMON_TIME_ZONE                   = @"timezone";
static NSString *COMMON_TIME_OFFSET                 = @"timeoffset";
static NSString *COMMON_FREE_SPACE_MB               = @"free_space_mb";

static NSString *COMMON_IS_ENABLE                   = @"is_enable";
static NSString *COMMON_IMAGE_FILE_NAME             = @"image_file_name";
static NSString *COMMON_APP_UUID                    = @"uuid";

static NSString *COMMON_ID                          = @"id";
static NSString *COMMON_EMAIL                       = @"email";
static NSString *COMMON_USER_FULL_NAME              = @"user_full_name";
static NSString *COMMON_PROFILE_SCOPE               = @"profile_scope";
//static NSString *COMMON_CREDENTIAL_VALUE            = @"credential_value";
static NSString *COMMON_DETAIL_MSG                  = @"detail_msg";
static NSString *COMMON_VALUE                       = @"value";
static NSString *COMMON_KEY                         = @"key";

static NSString *COMMON_INT_WITH_IDENTIFIER         = @"init_with_identifier";
static NSString *COMMON_ACCESS_GROUP                = @"access_group";

static NSString *COMMON_LOCAL_COUNTRY               = @"local_country";
static NSString *COMMON_NETWORK_COUNTRY             = @"network_country";
static NSString *COMMON_INSTALL_REFERRER            = @"install_referrer";
static NSString *COMMON_VERSION_NAME                = @"version_name";
static NSString *COMMON_SIM_STATE                   = @"sim_state";
static NSString *COMMON_SCHEME_DATA                 = @"scheme_data";
static NSString *COMMON_PACKAGE_NAME                = @"package_name";
static NSString *COMMON_SIZE                        = @"size";
static NSString *COMMON_INSTALLED 		            = @"installed";
static NSString *COMMON_DEEPLINK                    = @"deeplink";

static NSString *COMMON_UPDATE_AVAILABLE 		    = @"update_available";
static NSString *COMMON_UPDATE_FLEXIBLE 		    = @"update_flexible";
static NSString *COMMON_UPDATE_IMMEDIATE            = @"update_immediate";
static NSString *COMMON_WORD_KEY                    = @"word_key";
static NSString *COMMON_WORD_VALUE                  = @"word_value";
static NSString *COMMON_GROUP_ID                    = @"group_id";

static NSString *COMMON_RECEIVER_ADDRESS            = @"receiver_address";
static NSString *COMMON_TITLE                       = @"title";
static NSString *COMMON_BODY                        = @"body";

//2.1.0 update
static NSString *COMMON_GET_ATT_STATUS              = @"GET_ATT_STATUS";
static NSString *COMMON_REQUEST_ATT                 = @"REQUEST_ATT";
static NSString *COMMON_ATT_STATUS                  = @"att_status";

//2.1.1 update
static NSString *COMMON_GET_IDFA                    = @"GET_IDFA";
static NSString *COMMON_IDFA                        = @"idfa";

//2.1.3 update
static NSString *COMMON_SET_UUID                    = @"SET_UUID";

//2.1.4 update
static NSString *COMMON_SEND_EMAIL                  = @"SEND_EMAIL";
static NSString *COMMON_SUCCESS                     = @"success";
#endif
