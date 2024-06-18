#import "WebViewPlugin.h"

@interface WebViewPlugin ()
@property (nonatomic, strong) UIWebView *webView;
@end

@implementation WebViewPlugin

- (instancetype)initWithFrame:(CGRect)frame {
    self = [super init];
    if (self) {
        self.webView = [[UIWebView alloc] initWithFrame:frame];
        self.webView.scalesPageToFit = YES;
        [[UIApplication sharedApplication].keyWindow addSubview:self.webView];
    }
    return self;
}

- (void)loadURL:(NSString *)urlString {
    NSURL *url = [NSURL URLWithString:urlString];
    NSURLRequest *request = [NSURLRequest requestWithURL:url];
    [self.webView loadRequest:request];
}

- (void)closeWebView {
    [self.webView removeFromSuperview];
    self.webView = nil;
}