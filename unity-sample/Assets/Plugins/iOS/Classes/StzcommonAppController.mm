#import "UnityInterface.h"
#import "StzcommonAppController.h"
#import "StzCommonUtil.h"

#import <AppTrackingTransparency/ATTrackingManager.h>
#import <FBSDKCoreKit/FBSDKSettings.h>
#import <FBAudienceNetwork/FBAdSettings.h>
#import <UserNotifications/UserNotifications.h>

@implementation StzcommonAppController
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
{
    NSDictionary *userInfo = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
    if (userInfo) {
        for (id key in userInfo) {
            if([key isEqual: @"deep_link"])
            {
                [[NSUserDefaults standardUserDefaults] setObject:[userInfo objectForKey:key] forKey:@"deep_link"];
            }
        }
    } else {
        NSLog(@"StzcommonAppController::didFinishLaunchingWithOptions()->app start without notification");
    }
    
    [[NSUserDefaults standardUserDefaults] setObject:@"empty" forKey:@"launch_url_schema"];
    [[NSUserDefaults standardUserDefaults] setObject:@"empty" forKey:@"open_url_schema"];

    BOOL result = [super application:application didFinishLaunchingWithOptions:launchOptions];
    if( launchOptions )
    {
        NSURL * launchURL = [launchOptions objectForKey:UIApplicationLaunchOptionsURLKey];
        [[NSUserDefaults standardUserDefaults] setObject:[launchURL description] forKey:@"launch_url_schema"];
    }
    
    [UNUserNotificationCenter currentNotificationCenter].delegate = self;
    
    if (@available(iOS 14.0, *)) {
        switch ([ATTrackingManager trackingAuthorizationStatus]) {
            case ATTrackingManagerAuthorizationStatusNotDetermined:
            case ATTrackingManagerAuthorizationStatusAuthorized:
                [FBSDKSettings setAdvertiserTrackingEnabled:YES];
                [FBAdSettings setAdvertiserTrackingEnabled:YES];
                break;
            case ATTrackingManagerAuthorizationStatusRestricted:
            case ATTrackingManagerAuthorizationStatusDenied:
                [FBSDKSettings setAdvertiserTrackingEnabled:NO];
                [FBAdSettings setAdvertiserTrackingEnabled:NO];
                break;
        }
    }
    
    return result;
}

- (NSString *)stringFromDeviceToken:(NSData *)deviceToken {
    NSUInteger length = deviceToken.length;
    if (length == 0) {
        return nil;
    }
    const unsigned char *buffer = (unsigned char*)deviceToken.bytes;
    NSMutableString *hexString  = [NSMutableString stringWithCapacity:(length * 2)];
    for (int i = 0; i < length; ++i) {
        [hexString appendFormat:@"%02x", buffer[i]];
    }
    return [hexString copy];
}

- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken
{
    try {
        [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    } catch (NSException *e) {}
    
    NSString *pushToken = [self stringFromDeviceToken:deviceToken];
    NSLog(@"pushToken : %@",pushToken);
    
    [[NSUserDefaults standardUserDefaults] setObject:pushToken forKey:@"push_id"];
}

- (void)applicationWillResignActive:(UIApplication *)application
{
    try {
        [super applicationWillResignActive:[UIApplication sharedApplication]];
    } catch (NSException *e) {}
    
    if( [StzCommonUtil sharedInstance].isEnableSecureDisplay
       &&  [StzCommonUtil sharedInstance].securityImageFileName != nil)
    {
        self.securityImage = [[UIImageView alloc]initWithImage:[UIImage imageNamed:[StzCommonUtil sharedInstance].securityImageFileName]];
        self.securityImage.frame = _rootView.frame;

        [_rootView addSubview:self.securityImage];
        [_rootView bringSubviewToFront:self.securityImage];
    }
}

- (void)applicationDidBecomeActive:(UIApplication *)application
{
    try {
        [super applicationDidBecomeActive:[UIApplication sharedApplication]];
    } catch (NSException *e) {}
    
    if(self.securityImage != nil) {
        [self.securityImage removeFromSuperview];
        self.securityImage = nil;
    }
}

- (BOOL)application:(UIApplication*)app openURL:(NSURL*)url options:(NSDictionary<NSString*, id>*)options
{
    [[NSUserDefaults standardUserDefaults] setObject:[url description] forKey:@"open_url_schema"];
    
    try {
        [super application:app openURL:url options:options];
    } catch (NSException *e) {}
    
    return YES;
}

//foreground push
- (void) userNotificationCenter:(UNUserNotificationCenter *)center
        willPresentNotification:(UNNotification *)notification
          withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler {
    
    completionHandler(UNNotificationPresentationOptionNone);
}

//background push
- (void) userNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)(void))completionHandler {

    if ([response.actionIdentifier isEqual:UNNotificationDefaultActionIdentifier])//push message click
    {
        NSDictionary *userInfo = response.notification.request.content.userInfo;
        for (id key in userInfo) {
            if([key isEqual: @"deep_link"])
            {
                [[NSUserDefaults standardUserDefaults] setObject:[userInfo objectForKey:key] forKey:@"deep_link"];
            }
        }
    }
    else if ([response.actionIdentifier isEqual:UNNotificationDismissActionIdentifier])//push message close
    {
    }
    completionHandler();
    NSLog(@"StzcommonAppController::userNotificationCenter()->end");
}

@end
