#import "UnityAppController.h"
#import "StzCommonUtil.h"
#import <UserNotifications/UserNotifications.h>

@interface StzcommonAppController : UnityAppController<UNUserNotificationCenterDelegate>
- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions;
@property(nonatomic, retain) UIImageView *securityImage;
@end
IMPL_APP_CONTROLLER_SUBCLASS(StzcommonAppController)