//
//  StzCommonUtil.m
//  StzUnityCommonPlugin
//
//  Created by test user on 2015. 8. 14..
//  Copyright (c) 2015년 Sundaytoz. All rights reserved.
//

#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <CoreTelephony/CTCarrier.h>
#import <Foundation/Foundation.h>
#import <AppTrackingTransparency/ATTrackingManager.h>
#import <FBSDKCoreKit/FBSDKSettings.h>
#import <FBAudienceNetwork/FBAdSettings.h>
#import <AdSupport/ASIdentifierManager.h>

#import "CommonStringKey.h"
#import "StzCommonUtil.h"
#import "KeychainItemWrapper.h"

extern "C" void sundaytozUnityExtension(char* action, ResponseCallback callback)
{
    NSData * param = [NSData dataWithBytes:action length:strlen(action)];
    NSError *error = nil;
    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:param
                                                         options:0
                                                           error:&error];

    NSTimeZone *timeZones = [NSTimeZone localTimeZone];
    NSLog(@"timezone : %@", [timeZones name]);

    NSLog(@"ios Common Extension start 0218_1");
    [StzCommonUtil sharedInstance].callback = callback;
    [[StzCommonUtil sharedInstance] processWithData:dict];
    
}


@implementation StzCommonUtil
@synthesize callback, bgView, indicator, launchURL, isEnableSecureDisplay;

static StzCommonUtil *instance;
+(StzCommonUtil *)sharedInstance
{
    if(instance == nil)
    {
        instance = [[StzCommonUtil alloc] init];

        NSString *bundleIdentifier = [[NSBundle mainBundle] bundleIdentifier];
        
        instance.accessGroupBundleId = [NSString stringWithFormat:@"%@%@", [[NSBundle mainBundle] objectForInfoDictionaryKey:@"AppIdentifierPrefix"], bundleIdentifier];
        //NSLog(@"app prefix : %@", appIdentifierPrefix);
        
        instance.keychainItemBundleId = [[KeychainItemWrapper alloc] initWithIdentifier:bundleIdentifier accessGroup:nil];
        instance.keychainItemStzCommon = [[KeychainItemWrapper alloc] initWithIdentifier:[NSString stringWithFormat:@"%@.stzcommon", bundleIdentifier] accessGroup:nil];
        instance.keychainItemAppleLogin = [[KeychainItemWrapper alloc] initWithIdentifier:[NSString stringWithFormat:@"%@.applelogin", bundleIdentifier] accessGroup:instance.accessGroupBundleId];
    }
    return instance;
}


-(void) processWithData:(NSDictionary *)data
{
    NSLog(@"[Sundaytoz Common]processwithData, action : %@", data[COMMON_ACTION]);
    if([COMMON_INITIALIZE isEqualToString:data[COMMON_ACTION]])
    {
        ///iOS14 토스트 경고를 피하기 위해..
        [UIPasteboard generalPasteboard].string = @"";
        
        [StzCommonUtil sharedInstance];
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_INITIALIZE withParam:[[StzCommonUtil sharedInstance] getInitParam]];
    }
    else if ([COMMON_SET_TEXT_TO_SYSTEM_CLIPBOARD isEqualToString:data[COMMON_ACTION]])
    {
        NSString *text = data[COMMON_TEXT];
        [[StzCommonUtil sharedInstance] setTextToSystemClipboard:text];
    }
    else if ([ENABLE_SECURE_DISPLAY isEqualToString:data[COMMON_ACTION]])
    {
        Boolean isEnable = [data[COMMON_IS_ENABLE] boolValue];
        NSString *imageFileName = data[COMMON_IMAGE_FILE_NAME];
        self.isEnableSecureDisplay = isEnable;
        self.securityImageFileName = imageFileName;
    }
    else if([COMMON_LOCAL_NOTIFICATION_ADD isEqualToString:data[COMMON_ACTION]])
    {
        NSString *alarmId = [data[COMMON_NOTI_ID] stringValue];
        NSUInteger sec = [data[COMMON_TIME] integerValue];
        NSString *msg = data[COMMON_MESSAGE];
        NSUInteger badgeNum = [data[COMMON_COUNTER] integerValue];
        [[StzCommonUtil sharedInstance] addLocalNotificationWithSec:alarmId sec:sec AndMessage:msg AndBadge:badgeNum];
    }
    else if([COMMON_LOCAL_NOTIFICATION_CANCEL isEqualToString:data[COMMON_ACTION]])
    {
        NSString *alarmId = [data[COMMON_NOTI_ID] stringValue];
        [[StzCommonUtil sharedInstance] cancelNotification:alarmId];
    }
    else if([COMMON_APNS_NOTIFICATION_REGIST isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] registerNotifications];
    }
    else if([COMMON_LOCAL_NOTIFICATION_ALL_CANCEL isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] cancelAllNotification];
    }
    else if([COMMON_GET_PUSH_ID isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getPushId];
    }
    else if([COMMON_START_DEVICE_LOADING isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] startDeviceLoading];
    }
    else if([COMMON_STOP_DEVICE_LOADING isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] stopDeviceLoading];
    }
    else if([COMMON_INIT_APPLE_ACCOUNT isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] appleCheckAccount];
    }
    else if([COMMON_APPLE_LOGIN isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] appleLogin:data[COMMON_PROFILE_SCOPE]];
    }
    else if([COMMON_APPLE_LOGOUT isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] appleLogout];
    }
    else if([COMMON_GET_KEYCHAIN_VALUE isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getKeychainValue];
    }
    else if([COMMON_SET_KEYCHAIN_VALUE isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] setKeychainValue:data[COMMON_VALUE]];
    }
    else if([COMMON_GET_SHARED_VALUE isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getSharedValue:data[COMMON_KEY]];
    }
    else if([COMMON_SET_SHARED_VALUE isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] setSharedValue:data[COMMON_KEY] keychainValue:data[COMMON_VALUE]];
    }
    else if([COMMON_GET_LAUNCH_URL isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getLaunchUrlForUri];
    }
    else if([COMMON_GET_LANGUAGE isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getLanguage];
    }
    else if([COMMON_GET_SCHEME_DATA isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_SCHEME_DATA withParam:@{COMMON_SCHEME_DATA : [[StzCommonUtil sharedInstance] getScheme]}];
    }
    else if([COMMON_CLEAR_SCHEME_DATA isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] clearScheme];
    }
    else if([COMMON_GET_FREE_SPACE_MB isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_FREE_SPACE_MB withParam:@{COMMON_SIZE : [NSNumber numberWithInteger:[self totalFreeSpaceMB]]}];
    }
    else if([COMMON_UUID_WITH_KEYCHAIN isEqualToString:data[COMMON_ACTION]])
    {
        NSString *initIdentifier = data[COMMON_INT_WITH_IDENTIFIER];
        NSString *accessGroup = data[COMMON_ACCESS_GROUP];
        
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_UUID_WITH_KEYCHAIN withParam:@{COMMON_UUID_WITH_KEYCHAIN : [[StzCommonUtil sharedInstance] getUUIDWithKeychain:initIdentifier accessGroup:accessGroup]}];
    }
    else if([COMMON_IS_INSTALLED isEqualToString:data[COMMON_ACTION]])
    {
        NSString *schemeName = data[COMMON_PACKAGE_NAME];
        
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_IS_INSTALLED withParam:@{COMMON_INSTALLED : @([[StzCommonUtil sharedInstance] isInstalled:schemeName])}];
    }
    else if([COMMON_RISE_CRASH isEqualToString:data[COMMON_ACTION]])
    {
        NSLog(@"ios RiseCrash");
        @[][1];
    }
    else if([COMMON_SET_RESERVED_WORD isEqualToString:data[COMMON_ACTION]])
    {
        NSString *reservedKey = [data[COMMON_WORD_KEY] lowercaseString];
        NSString *reservedValue = data[COMMON_WORD_VALUE];
        NSString *groupId = data[COMMON_GROUP_ID];

        [[StzCommonUtil sharedInstance] setReservedWord:reservedKey value:reservedValue groupId:groupId];
    }
    else if([COMMON_GET_AND_CLEAR_DEEPLINK isEqualToString:data[COMMON_ACTION]])
    {
        NSString *deepLink = [[StzCommonUtil sharedInstance] getAndClearDeeplink];
        NSLog(@"StzCommonUtil()->deepLink:%@", deepLink);
        if( deepLink == nil )
            deepLink = @"";
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_AND_CLEAR_DEEPLINK withParam:@{COMMON_DEEPLINK : deepLink}];
    }
    else if([COMMON_OPEN_APP_DETAIL isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] openSettings];
    }
    else if([COMMON_GET_ATT_STATUS isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getATTStatus];
    }
    else if([COMMON_REQUEST_ATT isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] requestATT];
    }
    else if([COMMON_GET_IDFA isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] getIDFA];
    }
    else if([COMMON_SET_UUID isEqualToString:data[COMMON_ACTION]])
    {
        [[StzCommonUtil sharedInstance] setUUID];
    }
}

-(void)setReservedWord:(NSString*)key value:(NSString*)value groupId:(NSString*)groupId
{
    if( key == nil || value == nil )
    {
        [[StzCommonUtil sharedInstance] sendError:COMMON_SET_RESERVED_WORD withError:nil withParam:@{COMMON_STATUS_MESSAGE:@"key or value is nil"}];
        return;
    }

    NSUserDefaults *userDefaults = [[NSUserDefaults alloc] initWithSuiteName:@"group.com.sundaytoz"];
    NSArray<NSString*> *keys = [userDefaults stringArrayForKey:@"ReservedWordKeys"];
    if ( keys == nil )
    {
        keys = [NSArray arrayWithObject:key];
    }
    else
    {
        if( [keys indexOfObjectIdenticalTo:key] == NSNotFound )
        {
            [keys arrayByAddingObject:key];
        }
    }

    [userDefaults setObject:keys forKey:@"ReservedWordKeys"];
    
    [userDefaults setObject:value forKey:key];
    
    [userDefaults synchronize];
    
    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_SET_RESERVED_WORD withParam:@{COMMON_WORD_KEY : key}];
}
-(void)setTextToSystemClipboard:(NSString*)text
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    
    if (pasteboard != nil && text != nil)
    {
        pasteboard.string = text;
    }
}

-(void)addLocalNotificationWithSec:(NSString *)alarmId sec:(NSUInteger)sec AndMessage:(NSString *) msg AndBadge: (NSUInteger)badgeNum
{
    UILocalNotification *localNotification = [[UILocalNotification alloc] init];
    NSInteger currentBadgeNum = [[UIApplication sharedApplication] applicationIconBadgeNumber];
    
    NSDate *notificationDate = [NSDate dateWithTimeIntervalSinceNow:sec];
    
    localNotification.fireDate = notificationDate;
    localNotification.alertBody = msg;
    localNotification.timeZone  = [NSTimeZone systemTimeZone];
    localNotification.applicationIconBadgeNumber = currentBadgeNum + badgeNum;
    localNotification.userInfo = @{COMMON_NOTI_ID : alarmId};
    [[UIApplication sharedApplication] scheduleLocalNotification:localNotification];
}

-(void) cancelNotification:(NSString *)inId
{
    for(UILocalNotification *currentNoti in [[UIApplication sharedApplication] scheduledLocalNotifications])
    {
        if(currentNoti.userInfo!=nil && currentNoti.userInfo[COMMON_NOTI_ID]!=nil)
        {
            if([inId isEqualToString:currentNoti.userInfo[COMMON_NOTI_ID]])
            {
                [[UIApplication sharedApplication] cancelLocalNotification:currentNoti];
                break;
            }
        }
    }
}

-(void)registerNotifications
{
    //Registering for remote notifications
    if ([[UIApplication sharedApplication] respondsToSelector:@selector(isRegisteredForRemoteNotifications)])
    {
        // iOS 8 Notifications
        [[UIApplication sharedApplication] registerUserNotificationSettings:[UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeSound | UIUserNotificationTypeAlert | UIUserNotificationTypeBadge) categories:nil]];
        [[UIApplication sharedApplication] registerForRemoteNotifications];
    }
    else if ([[UIApplication sharedApplication] respondsToSelector:@selector(registerUserNotificationSettings:)])
    {
        
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeBadge|UIUserNotificationTypeAlert|UIUserNotificationTypeSound) categories:nil];
        [[UIApplication sharedApplication] registerUserNotificationSettings:settings];
    }
}

-(void) cancelAllNotification
{
    [[UIApplication sharedApplication] cancelAllLocalNotifications];
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
}

-(void) getPushId
{
    NSString *pushId = [[NSUserDefaults standardUserDefaults] objectForKey:@"push_id"];
    if(pushId == nil || (pushId != nil && [pushId isEqualToString:@""]))
    {
        NSLog(@"GET PUSH FAILED");
        [[StzCommonUtil sharedInstance] sendError:COMMON_GET_PUSH_ID withError:nil withParam:@{COMMON_STATUS_MESSAGE:@"push id is not stored in nsuserDefaults"}];
    }
    else
    {
        NSLog(@"GET PUSH SUCCESS");
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_PUSH_ID withParam:@{COMMON_REGISTRATION_ID : pushId}];
    }
}

-(Boolean)isInstalled:(NSString*)appScheme
{
    NSLog(@"StzCommonUtil::isIntalled()");
    bool ret = false;
    ret = [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString: appScheme]];
    return ret;
}

//extern "C" bool isIntalled(char* _appScheme)
//{
//    NSLog(@"StzCommonUtil::isIntalled()");
//    NSString* appScheme = [NSString stringWithUTF8String:_appScheme];
//    bool ret = false;
//    ret = [[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString: appScheme]];
//    return ret;
//}

-(void)sendSuccess:(NSString*)action withParam:(NSDictionary*)param
{
    dispatch_async(dispatch_get_main_queue(), ^{
        NSMutableDictionary* dictionary = [NSMutableDictionary dictionary];
        [dictionary setValue:action forKey:COMMON_ACTION];

        for (NSString *key in param) {
            id value = param[key];
            NSLog(@"%@ : %@", key, value);
        }
        if( param!=nil )
        {
            [dictionary setValue:param forKey:COMMON_DATA];
        }
        
        if( [StzCommonUtil sharedInstance].callback!=nil ) {
            NSError *err;
            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:&err];
            NSString* result = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            [StzCommonUtil sharedInstance].callback(result.UTF8String);
        }
    });
    
}
-(void)sendError:(NSString*)action withError:(NSError*)error withParam:(NSDictionary*)param;
{
    dispatch_async(dispatch_get_main_queue(), ^{
        NSMutableDictionary* dictionary = [NSMutableDictionary dictionary];
        [dictionary setValue:action forKey:COMMON_ACTION];
        [dictionary setObject:@"-1" forKey:COMMON_ERROR_CODE];
        if( error!=nil )
        {
            NSMutableDictionary* errorDictionary = [NSMutableDictionary dictionary];
            
//            [errorDictionary setObject:[NSString stringWithFormat:@"%d",(int)error.code]
//                                forKey:COMMON_STATUS_CODE];
//
//            [errorDictionary setObject:error.localizedDescription
//                                forKey:COMMON_STATUS_MESSAGE];
            
//            [dictionary setValue:errorDictionary forKey:COMMON_ERROR];
//            [dictionary setValue:param forKey:COMMON_RESULT];
            
            [dictionary setObject:[NSString stringWithFormat:@"%d",(int)error.code] forKey:COMMON_ERROR_CODE];
            [dictionary setObject:error.localizedDescription forKey:COMMON_ERROR_MSG];
            NSLog(@"sendError()-> error.code : %@, COMMON_ERROR_MSG : %@", [NSString stringWithFormat:@"%d",(int)error.code], error.localizedDescription);
        }
        
        if( [StzCommonUtil sharedInstance].callback!=nil ) {
            NSError *err;
            NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary options:0 error:&err];
            NSString* result = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
            [StzCommonUtil sharedInstance].callback(result.UTF8String);
        }
    });
}


-(void)startDeviceLoading
{
    if(self.indicator == nil)
    {
       self.indicator = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleWhiteLarge];
    }
    if(self.bgView == nil)
    {
        self.bgView = [[UIView alloc] init];
        [bgView setFrame:[[[[UIApplication sharedApplication] keyWindow] rootViewController] view].frame];
        [bgView setBackgroundColor:[UIColor blackColor]];
        [bgView setAlpha:0.7];
        
    }
    
    [self.indicator setFrame:
     CGRectMake([[[[UIApplication sharedApplication] keyWindow] rootViewController] view].frame.size.width/2 - self.indicator.frame.size.width/2,
                [[[[UIApplication sharedApplication] keyWindow] rootViewController] view].frame.size.height/2 - self.indicator.frame.size.height/2,
                self.indicator.frame.size.width,
                self.indicator.frame.size.height)];
    
    
    if([[[[UIApplication sharedApplication] keyWindow] rootViewController] view] != [self.bgView superview])
    {
        [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] addSubview:self.bgView];
    }
    if([[[[UIApplication sharedApplication] keyWindow] rootViewController] view] != [self.indicator superview])
    {
        [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] addSubview:self.indicator];
    }

    [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] bringSubviewToFront:self.bgView];
    [[[[[UIApplication sharedApplication] keyWindow] rootViewController] view] bringSubviewToFront:self.indicator];
    [self.indicator startAnimating];
}
-(void)stopDeviceLoading
{
    if(self.indicator!=nil)
    {
        [self.indicator removeFromSuperview];
        [self.indicator stopAnimating];
    }
    if(self.bgView!=nil)
    {
        [self.bgView removeFromSuperview];
    }
}
-(void)saveUserAppleId:(NSString *)userId
{
    //NSLog(@"saveUserAppleId()-> user: %@", userId);
    try {
        [instance.keychainItemAppleLogin resetKeychainItem];

        instance.keychainItemAppleLogin = [[KeychainItemWrapper alloc] initWithIdentifier:[NSString stringWithFormat:@"%@.applelogin", [[NSBundle mainBundle] bundleIdentifier]] accessGroup:instance.accessGroupBundleId];
        
        [instance.keychainItemAppleLogin setObject:userId forKey:(__bridge id)kSecAttrAccount];
    }
    catch (NSException *e)
    {
        NSLog(@"%@", e.reason);
    }
}
-(void)appleCheckAccount
{
    NSLog(@"[Sundaytoz Common] appleCheckAccount");
    if (@available(iOS 13.0, *))
    {
        // A mechanism for generating requests to authenticate users based on their Apple ID.
        ASAuthorizationAppleIDProvider *appleIDProvider = [ASAuthorizationAppleIDProvider new];
        
        NSString *userIdentifier = [instance.keychainItemAppleLogin objectForKey:(__bridge id)kSecAttrAccount];
        if( userIdentifier == nil || [userIdentifier isEqual: @""] )
        {
            NSString * userIdentifierValueData = [instance.keychainItemBundleId objectForKey:(__bridge id)kSecValueData];
            if( userIdentifierValueData != nil && userIdentifierValueData.length > 0)
            {
                NSLog(@"Save Previos apple id");
                userIdentifier = userIdentifierValueData;
                [[StzCommonUtil sharedInstance] saveUserAppleId:userIdentifier];
            }
            else
            {
                NSString * userIdentifierAttrAccount = [instance.keychainItemBundleId objectForKey:(__bridge id)kSecAttrAccount];

                if( userIdentifierAttrAccount != nil && userIdentifierAttrAccount.length > 0)
                {
                    NSLog(@"Save Previos apple id");
                    userIdentifier = userIdentifierAttrAccount;
                    [[StzCommonUtil sharedInstance] saveUserAppleId:userIdentifier];
                }
            }
            
        }
        
        //NSLog(@"user() : %@", userIdentifier);
        
        if (userIdentifier == nil || [userIdentifier isEqual: @""]) {
            NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
            
            [dict setValue:@(ASAuthorizationAppleIDProviderCredentialNotFound) forKey:COMMON_STATUS_CODE];
            
            [dict setValue:@"empty" forKey:COMMON_ID];
            [dict setValue:@"empty" forKey:COMMON_EMAIL];
            [dict setValue:@"empty" forKey:COMMON_USER_FULL_NAME];
            [dict setValue:@"CredentialNotFound" forKey:COMMON_DETAIL_MSG];

            [[StzCommonUtil sharedInstance] sendSuccess:COMMON_INIT_APPLE_ACCOUNT withParam:dict];
        }
        else
        {
            NSString* __block detailMsg = nil;
            //Returns the credential state for the given user in a completion handler.
            [appleIDProvider getCredentialStateForUserID:userIdentifier completion:^(ASAuthorizationAppleIDProviderCredentialState credentialState, NSError * _Nullable error) {
                switch (credentialState) {
                    case ASAuthorizationAppleIDProviderCredentialRevoked:
                        detailMsg = @"CredentialRevoked";//0
                        break;
                    case ASAuthorizationAppleIDProviderCredentialAuthorized:
                        detailMsg = @"CredentialAuthorized";//1
                        break;
                    case ASAuthorizationAppleIDProviderCredentialNotFound:
                        detailMsg = @"CredentialNotFound";//2
                        break;
                    case ASAuthorizationAppleIDProviderCredentialTransferred:
                        detailMsg = @"CredentialTransferred";//3
                        break;
                }
                if( credentialState != ASAuthorizationAppleIDProviderCredentialAuthorized)
                    [[StzCommonUtil sharedInstance] appleLogout];;
                dispatch_async(dispatch_get_main_queue(), ^{

                    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
                    
                    [dict setValue:@(credentialState) forKey:COMMON_STATUS_CODE];
                    
                    if( credentialState == ASAuthorizationAppleIDProviderCredentialAuthorized)
                        [dict setValue:userIdentifier forKey:COMMON_ID];
                    else
                    {
                        [dict setValue:@"empty" forKey:COMMON_ID];
                    }
                    [dict setValue:@"empty" forKey:COMMON_EMAIL];
                    [dict setValue:@"empty" forKey:COMMON_USER_FULL_NAME];
                    [dict setValue:detailMsg forKey:COMMON_DETAIL_MSG];

                    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_INIT_APPLE_ACCOUNT withParam:dict];
                });
            }];
        }
    }
}

-(void)appleLogin:(NSString *)profileScope
{
    if (@available(iOS 13.0, *))
    {
        ASAuthorizationAppleIDProvider * provider = [[ ASAuthorizationAppleIDProvider alloc ] init] ;
        
        ASAuthorizationAppleIDRequest * request = [ provider createRequest];
        //NSLog(@"appleLogin()-> profileScope : %@", profileScope);

        NSMutableArray<ASAuthorizationScope> * authorizationScope = [[NSMutableArray alloc] init];
        if( [profileScope rangeOfString:@"ASAuthorizationScopeFullName"].location != NSNotFound)
        {
            [authorizationScope addObject:ASAuthorizationScopeFullName];
        }
        if( [profileScope rangeOfString:@"ASAuthorizationScopeEmail"].location != NSNotFound)
        {
            [authorizationScope addObject:ASAuthorizationScopeEmail];
        }
        [request setRequestedScopes:authorizationScope];
                
        ASAuthorizationController * controller = [[ ASAuthorizationController alloc ] initWithAuthorizationRequests:@[ request]];
        
        controller.delegate = self;
        
        [controller performRequests];
    }
    else
    {
        NSLog(@"[Sundaytoz Common] apple sign in need ios 13");
        [[StzCommonUtil sharedInstance] sendError:COMMON_APPLE_LOGIN withError:nil withParam:@{COMMON_STATUS_MESSAGE:@"apple sign in need ios 13"}];
    }
}

-(void)authorizationController:(ASAuthorizationController *)controller didCompleteWithAuthorization:(nonnull ASAuthorization *)authorization API_AVAILABLE(ios(13.0))
{
    if (@available(iOS 13.0, *))
    {
        ASAuthorizationAppleIDCredential * appleIDCrendential = [authorization credential ];
        NSLog(@"authorizationController()->state : %@",[appleIDCrendential state]);
        NSString * user = [NSString stringWithFormat:@"%@", [appleIDCrendential user ]];
        NSString * fullName = [ NSString stringWithFormat : @"%@ %@", [[appleIDCrendential fullName ] familyName], [[appleIDCrendential fullName] givenName] ];
        NSString * email = [NSString stringWithFormat : @"%@", [appleIDCrendential email ]];

        if( [appleIDCrendential fullName ].familyName.length < 1 && [appleIDCrendential fullName ].givenName.length < 1)
            fullName = @"empty";
        if( [appleIDCrendential email ].length < 1 )
            email = @"empty";
        
        if( user.length < 1 )
        {
            user = @"empty";
            NSError* error = [NSError errorWithDomain:@"apple_login_failed" code:1000 userInfo:@{
                NSLocalizedDescriptionKey:@"apple login failed. user id is empty."
            }];
            [[StzCommonUtil sharedInstance] sendError:COMMON_APPLE_LOGIN withError:error withParam:nil];
        }
        else
        {
            NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
            
            [dict setValue:@1 forKey:COMMON_STATUS_CODE];
            [dict setValue:@"Login Success" forKey:COMMON_DETAIL_MSG];
            [dict setValue:user forKey:COMMON_ID];
            [dict setValue:fullName forKey:COMMON_USER_FULL_NAME];
            [dict setValue:email forKey:COMMON_EMAIL];

            [[StzCommonUtil sharedInstance] sendSuccess:COMMON_APPLE_LOGIN withParam:dict];

            //NSLog(@"user: %@, fullName : %@, email : %@", user, fullName, email);
            
            [[StzCommonUtil sharedInstance] saveUserAppleId:user];
        }
    }
    else
    {
        [[StzCommonUtil sharedInstance] sendError:COMMON_APPLE_LOGIN withError:nil withParam:@{COMMON_STATUS_MESSAGE:@"apple sign in need ios 13"}];
    }
}
-(void) authorizationController:(ASAuthorizationController * ) controller didCompleteWithError:(nonnull NSError *)error API_AVAILABLE(ios(13.0))
{
    NSLog(@"apple login failed()-> %@", error);
    
    [[StzCommonUtil sharedInstance] sendError:COMMON_APPLE_LOGIN withError:error withParam:nil];
}
-(void)appleLogout
{
    NSLog(@"apple logout");
    try {
        [instance.keychainItemBundleId resetKeychainItem];
        [instance.keychainItemAppleLogin resetKeychainItem];

        instance.keychainItemAppleLogin = [[KeychainItemWrapper alloc] initWithIdentifier:[NSString stringWithFormat:@"%@.applelogin", [[NSBundle mainBundle] bundleIdentifier]] accessGroup:instance.accessGroupBundleId];
    }
    catch (NSException *e)
    {
        NSLog(@"%@", e.reason);
    }
}
// Prompts the user if an existing iCloud Keychain credential or Apple ID credential is found.
-(void)appleExistingAccountSetupFlows
{
    NSLog(@"[Sundaytoz Common] appleExistingAccountSetupFlows");
    if (@available(iOS 13.0, *)) {
        ASAuthorizationAppleIDProvider * idProvider = [[ ASAuthorizationAppleIDProvider alloc ] init] ;
        ASAuthorizationAppleIDRequest * idRequest = [ idProvider createRequest];
        ASAuthorizationPasswordProvider * passwordProvider = [[ ASAuthorizationPasswordProvider alloc ] init] ;
        ASAuthorizationPasswordRequest * passwordRequest = [ passwordProvider createRequest];
        ASAuthorizationController * controller = [[ASAuthorizationController alloc ] initWithAuthorizationRequests:@[ idRequest, passwordRequest]];
        controller.delegate = self;
        [controller performRequests];
    } else {
        NSLog(@"[Sundaytoz Common] appleExistingAccountSetupFlows apple sign in need ios 13 !");
    }
}
-(void)getKeychainValue
{
    NSLog(@"getKeychainValue");
    
    NSString *keychainValue = [instance.keychainItemStzCommon objectForKey:(__bridge id)kSecAttrAccount];
    
    NSLog(@"getKeychainValue%@", keychainValue);
    
    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_KEYCHAIN_VALUE withParam:@{COMMON_APP_PARAM : keychainValue}];
}
-(void)setKeychainValue:(NSString *)keychainValue
{
    NSLog(@"setKeychainValue");
    try {
        [instance.keychainItemStzCommon setObject:keychainValue forKey:(__bridge id)kSecAttrAccount];
    }
    catch (NSException *e)
    {
        NSLog(@"%@", e.reason);
    }
}
-(void)getSharedValue:(NSString *)keychainKey
{
    NSString *appIdentifierPrefix = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"AppIdentifierPrefix"];
    
    NSLog(@"getSharedValue()->keychainKey:%@, appIdentifierPrefix1:%@", keychainKey, appIdentifierPrefix);
    
    KeychainItemWrapper* keychainItemStzCommonShare = [[KeychainItemWrapper alloc] initWithIdentifier:keychainKey accessGroup:[NSString stringWithFormat:@"%@stzcommon", appIdentifierPrefix]];
    
    NSString *keychainValue = [keychainItemStzCommonShare objectForKey:(__bridge id)kSecAttrAccount];

    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    [dict setValue:keychainValue forKey:COMMON_VALUE];
    
    NSLog(@"getSharedValue:%@", keychainValue);

    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_SHARED_VALUE withParam:dict];
    
    //[[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_SHARED_VALUE withParam:@{COMMON_APP_PARAM : keychainValue}];
}
-(void)setSharedValue:(NSString *)keychainKey keychainValue:(NSString *)keychainValue
{
    
    NSLog(@"setSharedValue()->key:%@, value:%@", keychainKey, keychainValue);
    
    try {
        NSString *appIdentifierPrefix = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"AppIdentifierPrefix"];

        KeychainItemWrapper* keychainItemStzCommonShare = [[KeychainItemWrapper alloc] initWithIdentifier:keychainKey accessGroup:[NSString stringWithFormat:@"%@stzcommon", appIdentifierPrefix]];

        //[keychainItemStzCommonShare resetKeychainItem];
        [keychainItemStzCommonShare setObject:keychainValue forKey:(__bridge id)kSecAttrAccount];
    }
    catch (NSException *e)
    {
        NSLog(@"%@", e.reason);
    }
}
-(void)getLaunchUrlForUri
{
    NSString *launchUrl = [[NSUserDefaults standardUserDefaults] objectForKey:@"launch_url"];
    if(launchUrl == nil || (launchUrl != nil && [launchUrl isEqualToString:@""]))
    {
        launchUrl = @"";
    }
    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_LAUNCH_URL withParam:@{COMMON_APP_PARAM : launchUrl}];

}

-(void)getLanguage
{
    NSString *language = [[NSLocale preferredLanguages] objectAtIndex:0];
    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_LANGUAGE withParam:@{COMMON_LANGUAGE : language}];
}

-(void)clearScheme
{
    [[NSUserDefaults standardUserDefaults] setObject:@"empty" forKey:@"launch_url_schema"];
    [[NSUserDefaults standardUserDefaults] setObject:@"empty" forKey:@"open_url_schema"];
}

- (uint64_t)totalFreeSpaceMB
{
    NSLog(@"[Sundaytoz Common]freeDiskspace");
    uint64_t totalSpace = 0;
    uint64_t totalFreeSpace = 0;
    uint64_t totalFreeSpaceMB = 0;
    __autoreleasing NSError *error = nil;
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSDictionary *dictionary = [[NSFileManager defaultManager] attributesOfFileSystemForPath:[paths lastObject] error: &error];
    
    if (dictionary) {
        NSNumber *fileSystemSizeInBytes = [dictionary objectForKey: NSFileSystemSize];
        NSNumber *freeFileSystemSizeInBytes = [dictionary objectForKey:NSFileSystemFreeSize];
        totalSpace = [fileSystemSizeInBytes unsignedLongLongValue];
        totalFreeSpace = [freeFileSystemSizeInBytes unsignedLongLongValue];
        totalFreeSpaceMB = ((totalFreeSpace/1024ll)/1024ll);
        NSLog(@"Memory Capacity of %llu MiB with %llu MiB Free memory available.", ((totalSpace/1024ll)/1024ll), ((totalFreeSpace/1024ll)/1024ll));
    } else {
        NSLog(@"Error Obtaining System Memory Info: Domain = %@, Code = %ld", [error domain], (long)[error code]);
    }
    
    return totalFreeSpaceMB;
}
-(NSDictionary*)getStorageSpace
{
    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    NSNumber *freeSpaceMB = [NSNumber numberWithInteger:[self totalFreeSpaceMB]];
    
    [dict setValue:freeSpaceMB forKey:COMMON_FREE_SPACE_MB];
    
    return dict;
}

-(NSDictionary*)getUUIDWithKeychain: (NSString *)identifier accessGroup:(NSString *) accessGroup;
{
    NSLog(@"getUUIDWithKeychain Called");

    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    id uuid;
    try
    {
        KeychainItemWrapper *wrapper = [[KeychainItemWrapper alloc] initWithIdentifier:identifier accessGroup:accessGroup];
        
        uuid = [wrapper objectForKey:(__bridge id)(kSecAttrAccount)];
    }
    catch (NSException *e)
    {
        NSLog(@"%@", e.reason);
        [[StzCommonUtil sharedInstance] sendError:COMMON_UUID_WITH_KEYCHAIN withError:nil withParam:@{COMMON_STATUS_MESSAGE:e.reason}];
    }

    if( uuid == nil )
        uuid = @"";
    
    //NSLog(@"A uuid : %@", uuid);

    [dict setValue:uuid forKey:COMMON_APP_UUID];
    
    return dict;
}
-(NSDictionary*)getInitParam
{
    NSMutableDictionary* dict = [[NSMutableDictionary alloc] init];
    
    [dict setValue:[[UIDevice currentDevice] model] forKey:COMMON_DEVICE_NAME];
    
    CTTelephonyNetworkInfo *netinfo = [[CTTelephonyNetworkInfo alloc] init];
    CTCarrier *carrier = [netinfo subscriberCellularProvider];
    NSLocale *currentLocale = [NSLocale currentLocale];
    NSString *countryCode = [currentLocale objectForKey:NSLocaleCountryCode];
    
    [dict setValue:countryCode forKey:COMMON_LOCAL_COUNTRY];
    
    
    [dict setValue:@"empty" forKey:COMMON_INSTALL_REFERRER];//aos 와 규약 맞춤
    [dict setValue:@"empty" forKey:COMMON_SIM_STATE];//aos 와 규약 맞춤
    
    if(carrier!=nil){
        [dict setValue:[carrier carrierName] forKey:COMMON_CARRIER];
        [dict setValue:[carrier isoCountryCode] forKey:COMMON_COUNTRY];
        [dict setValue:[carrier isoCountryCode] forKey:COMMON_NETWORK_COUNTRY];
    }else{
        [dict setValue:countryCode forKey:COMMON_COUNTRY];
    }
    
    if( [dict valueForKey:COMMON_COUNTRY] == nil)
        [dict setValue:countryCode forKey:COMMON_COUNTRY];
    if( [dict valueForKey:COMMON_CARRIER] == nil)
        [dict setValue:@"empty" forKey:COMMON_CARRIER];
    if( [dict valueForKey:COMMON_NETWORK_COUNTRY] == nil)
        [dict setValue:@"empty" forKey:COMMON_NETWORK_COUNTRY];
    
    NSTimeZone *timeZones = [NSTimeZone localTimeZone];
    [dict setValue:[timeZones name] forKey:COMMON_TIME_ZONE];

    NSInteger offset = [timeZones secondsFromGMT]/3600.0;
    [dict setValue:[@(offset) stringValue] forKey:COMMON_TIME_OFFSET];
    
    NSNumber *freeSpaceMB = [NSNumber numberWithInteger:[self totalFreeSpaceMB]];
    [dict setValue:freeSpaceMB forKey:COMMON_FREE_SPACE_MB];
    
    [dict setValue:@"empty" forKey:COMMON_VERSION_NAME];

    [dict setValue:[[UIDevice currentDevice] systemVersion] forKey:COMMON_OS_VERSION];
    
    NSString *launchUrl = [[NSUserDefaults standardUserDefaults] objectForKey:@"launch_url"];
    if(launchUrl == nil || (launchUrl != nil && [launchUrl isEqualToString:@""])){
         [dict setValue:@"empty" forKey:COMMON_APP_PARAM];
    }else{
        [dict setValue:launchUrl forKey:COMMON_APP_PARAM];
    }
    
    [dict setValue:@"empty" forKey:COMMON_UPDATE_AVAILABLE];
    [dict setValue:@"empty" forKey:COMMON_UPDATE_FLEXIBLE];
    [dict setValue:@"empty" forKey:COMMON_UPDATE_IMMEDIATE];
    
    [dict setValue:@(-1) forKey:COMMON_API_LEVEL];
    
    [dict setValue:[self getUUID] forKey:COMMON_APP_UUID];
    return dict;
    
}

//old version
//- (NSString*) getUUID
//{
//    // initialize keychaing item for saving UUID.
//    KeychainItemWrapper *wrapper = [[KeychainItemWrapper alloc] initWithIdentifier:@"UUID" accessGroup:nil];
//
//    NSString *uuid = [wrapper objectForKey:(__bridge id)(kSecAttrAccount)];
//
//    if( uuid == nil || uuid.length == 0)
//    {
//
//        // if there is not UUID in keychain, make UUID and save it.
//        CFUUIDRef uuidRef = CFUUIDCreate(NULL);
//        CFStringRef uuidStringRef = CFUUIDCreateString(NULL, uuidRef);
//
//        CFRelease(uuidRef);
//        uuid = [NSString stringWithString:(__bridge NSString *) uuidStringRef];
//        CFRelease(uuidStringRef);
//        try {
//            // save UUID in keychain
//            [wrapper setObject:uuid forKey:(__bridge id)(kSecAttrAccount)];
//        }
//        catch (NSException *e)
//        {
//            NSLog(@"%@", e.reason);
//        }
//
//    }
//
//    return uuid;
//}

static NSString *UUID_IDENTIFIER = @"UUID";

//new version
- (NSString*) getUUID
{
    //old 저장소의 UUID
    KeychainItemWrapper *oldWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:UUID_IDENTIFIER accessGroup:nil];
    NSString *olduuid = [oldWrapper objectForKey:(__bridge id)kSecAttrAccount];
    
    //new 저장소의 UUID
    NSString *appIdentifierPrefix = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"AppIdentifierPrefix"];
    NSString *bundleIdentifier = [[NSBundle mainBundle] bundleIdentifier];
    NSString *newAccessGroup = [NSString stringWithFormat:@"%@%@", appIdentifierPrefix, bundleIdentifier];
    KeychainItemWrapper *newWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:UUID_IDENTIFIER accessGroup:newAccessGroup];
    NSString *newuuid = [newWrapper objectForKey:(__bridge id)kSecAttrAccount];
    
    if ([olduuid length] != 0) { //old UUID 값이 있다면 그 값을 사용하고 신규 저장소에 같이 저장한다.
        
        if(![olduuid isEqualToString:newuuid]) {
            //신규 저장소에 저장하려면 기존에 저장된 값을 삭제해야 한다.
            if ([newuuid length] != 0) {
                [newWrapper resetKeychainItem];
                newWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:UUID_IDENTIFIER accessGroup:newAccessGroup];
            }
            
            [newWrapper setObject:olduuid forKey:(__bridge id)(kSecAttrAccount)];
        }
        
        newuuid = olduuid;
    }
    else if ([newuuid length] != 0) { //new UUID 값이 있다면 그 값을 사용한다.
    }
    else { ///아무 값이 없다면 신규 생성 후 new 저장소에 저장한다.
        
        // if there is not UUID in keychain, make UUID and save it.
        CFUUIDRef uuidRef = CFUUIDCreate(NULL);
        CFStringRef uuidStringRef = CFUUIDCreateString(NULL, uuidRef);
        
        CFRelease(uuidRef);
        newuuid = [NSString stringWithString:(__bridge NSString *) uuidStringRef];
        CFRelease(uuidStringRef);
        
        try {
            // save UUID in keychain
            [newWrapper setObject:newuuid forKey:(__bridge id)(kSecAttrAccount)];
        }
        catch (NSException *e)
        {
            NSLog(@"%@", e.reason);
        }
    }
    
    return newuuid;
}

- (NSString*) getScheme
{
    NSString *ret = [NSString stringWithUTF8String:"empty"];
    NSString *launchUrlScheme = [[NSUserDefaults standardUserDefaults] objectForKey:@"launch_url_schema"];
    
    if( launchUrlScheme != nil && [launchUrlScheme isEqualToString:@"empty"] == false )
    {
        ret = launchUrlScheme;
    }
    else
    {
        NSString *openUrlScheme = [[NSUserDefaults standardUserDefaults] objectForKey:@"open_url_schema"];
        if( openUrlScheme != nil && [openUrlScheme isEqualToString:@"empty"] == false )
            ret = openUrlScheme;
    }
    
    return ret;
}

- (NSString*) getAndClearDeeplink
{
    NSLog(@"NotificationService::getDeeplinkFromPush(1)->");
    
    NSString *ret = @"";
    
    ret = [[NSUserDefaults standardUserDefaults] stringForKey:@"deep_link"];
    
    NSLog(@"NotificationService::getDeeplinkFromPush(3)->deepLink:%@", ret);
    
    [[NSUserDefaults standardUserDefaults] setObject:@"" forKey:@"deep_link"];

    return ret;
}

- (void) openSettings
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
}

- (void) getATTStatus
{
    if (@available(iOS 14.5, *)) {
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_ATT_STATUS withParam:@{COMMON_ATT_STATUS : [NSNumber numberWithInteger:[ATTrackingManager trackingAuthorizationStatus]]}];
    }
    else {
        //ATTrackingManagerAuthorizationStatusAuthorized
        [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_ATT_STATUS withParam:@{COMMON_ATT_STATUS : [NSNumber numberWithInteger:3]}];
    }
}

- (void) requestATT
{
    if (@available(iOS 14.5, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            
            switch (status) {
                case ATTrackingManagerAuthorizationStatusAuthorized:
                    [FBSDKSettings setAdvertiserTrackingEnabled:YES];
                    [FBAdSettings setAdvertiserTrackingEnabled:YES];
                    break;
                default:
                    [FBSDKSettings setAdvertiserTrackingEnabled:NO];
                    [FBAdSettings setAdvertiserTrackingEnabled:NO];
                    break;
            }
            
            [[StzCommonUtil sharedInstance] sendSuccess:COMMON_REQUEST_ATT withParam:@{COMMON_ATT_STATUS : [NSNumber numberWithInteger:status]}];
        }];
    }
    else {
        [[StzCommonUtil sharedInstance] sendError:COMMON_REQUEST_ATT withError:nil withParam:@{COMMON_STATUS_MESSAGE:@"requestATT is only iOS 14.5 higher is supported."}];
    }
}

- (void) getIDFA
{
    NSString *idfaString = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    [[StzCommonUtil sharedInstance] sendSuccess:COMMON_GET_IDFA withParam:@{COMMON_IDFA : idfaString}];
}

- (void) setUUID: (NSString *)uuid
{    
    NSLog(@"StzCommonUtil::setUUID()->%@", uuid);
    
    NSString* oldUUID = [[StzCommonUtil sharedInstance] getUUID];
    if( [oldUUID isEqualToString:uuid] )
    {
        NSLog(@"StzCommonUtil::setUUID()->Already same value");
    }
    else
    {
        NSLog(@"StzCommonUtil::setUUID()->Overwrite begin");
        
        NSString *appIdentifierPrefix = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"AppIdentifierPrefix"];
        NSString *bundleIdentifier = [[NSBundle mainBundle] bundleIdentifier];
        NSString *newAccessGroup = [NSString stringWithFormat:@"%@%@", appIdentifierPrefix, bundleIdentifier];
        
        KeychainItemWrapper *newWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:UUID_IDENTIFIER accessGroup:newAccessGroup];

        [newWrapper resetKeychainItem];

        newWrapper = [[KeychainItemWrapper alloc] initWithIdentifier:UUID_IDENTIFIER accessGroup:newAccessGroup];
        
        [newWrapper setObject:uuid forKey:(__bridge id)(kSecAttrAccount)];
        
        NSLog(@"StzCommonUtil::setUUID()->Overwrite end");
    }
}

@end
