//
//  StzCommonUtil.h
//  StzUnityCommonPlugin
//
//  Created by test user on 2015. 8. 14..
//  Copyright (c) 2015년 Sundaytoz. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AuthenticationServices/AuthenticationServices.h>
#import "KeychainItemWrapper.h"
#import <MessageUI/MessageUI.h>

typedef void (*ResponseCallback)(const char* response);

@interface StzCommonUtil : UIViewController<ASAuthorizationControllerDelegate, MFMailComposeViewControllerDelegate>
{}

@property (nonatomic,assign) bool isEnableSecureDisplay;
@property (nonatomic,strong) NSString *securityImageFileName;
@property (nonatomic,strong) NSString *accessGroupBundleId;
@property (nonatomic,assign) ResponseCallback callback;
@property (nonatomic,strong) UIActivityIndicatorView *indicator;
@property (nonatomic,strong) UIView *bgView;
@property (nonatomic,retain) NSURL *launchURL;
@property (nonatomic,retain) KeychainItemWrapper *keychainItemBundleId;
@property (nonatomic,retain) KeychainItemWrapper *keychainItemStzCommon;
@property (nonatomic,retain) KeychainItemWrapper *keychainItemAppleLogin;


+(StzCommonUtil *)sharedInstance;
-(void)setTextToSystemClipboard:(NSString*)text;
-(void)addLocalNotificationWithSec:(NSString *)alarmId sec:(NSUInteger)sec AndMessage:(NSString *) msg AndBadge: (NSUInteger)badgeNum;
-(void)cancelAllNotification;
-(void)registerNotifications;

-(void)startDeviceLoading;
-(void)stopDeviceLoading;

-(void) processWithData:(NSDictionary *)data;

-(void) getPushId;
-(void) setUUID;
-(void) sendEmail:(NSString*)receiverAddress title:(NSString*)title body:(NSString*)body;
-(NSDictionary *)getUUIDWithKeychain: (NSString *)identifier accessGroup:(NSString *) accessGroup;

@end
