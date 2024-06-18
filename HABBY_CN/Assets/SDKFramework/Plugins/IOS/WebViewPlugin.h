#import <UIKit/UIKit.h>

@interface WebViewPlugin : NSObject
- (instancetype)initWithFrame:(CGRect)frame;
- (void)loadURL:(NSString *)urlString;
- (void)closeWebView;
@end