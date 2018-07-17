// https://www.youtube.com/watch?v=zP5mjHzJR-o

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

-(int) add: (int) x
       with: (int) y
{
    return x + y;
}

@end

extern "C"
{
    int _add(int x, int y)
    {
        // Just a simple example of returning an int value
        return [[SnapShot getInstance] add:x with:y];
    }
}
