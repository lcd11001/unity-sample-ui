// iOS plugin: https://www.youtube.com/watch?v=zP5mjHzJR-o
// plist.info: https://www.youtube.com/watch?v=VAQriQlR95A

#import <Foundation/Foundation.h>

@interface SnapShot: NSObject
{
    
}
@end

@implementation SnapShot

static SnapShot* _instance = nullptr;
+(SnapShot*) getInstance
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        NSLog(@"Creating SnapShot plugin instance");
        _instance = [[SnapShot alloc] init];
    });
    
    return _instance;
}

-(id) init
{
    self = [super init];
    if (self)
    {
        [self initHelper];
    }
    
    return self;
}

-(void) initHelper
{
    NSLog(@"initHelper called");
}

-(void) SaveSnapShot: (NSData*) data
{
    UIImage* image = [UIImage imageWithData:data];
    UIImageWriteToSavedPhotosAlbum(image, self, @selector(image: didFinishSavingWithError: contextInfo:), nil);
}

- (void) image:(UIImage *)image
didFinishSavingWithError:(NSError *)error
   contextInfo:(void *)contextInfo
{
    if (error != nil)
    {
        NSLog(@"Save snap shot error %@", error.localizedDescription);
        return;
    }
    NSLog(@"Save snap shot successful");
}

@end

extern "C"
{
    void _SaveSnapShot(Byte bytes[], int len)
    {
        NSData* data = [NSData dataWithBytesNoCopy:bytes length:len freeWhenDone:FALSE];
        [[SnapShot getInstance] SaveSnapShot:data];
    }
}
