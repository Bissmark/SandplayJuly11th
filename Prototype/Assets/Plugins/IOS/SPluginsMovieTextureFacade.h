#import <Foundation/Foundation.h>
#import "NTEPLog.h"
#import "Player/NTEPMovieTexturePlayer.h"


typedef void (*DelegateOnLoadCompleted)(int nativeTextureID_, int resultType_, const char*);
typedef void (*DelegateOnPlayCompleted)(int nativeTextureID_, int completedType_);

@interface NTEPMovieTextureFacade : NSObject <DelegateOnEventListener>
{
@private
	NSMutableDictionary* _playerDic;
	DelegateOnLoadCompleted _delegateOnLoadCompleted;
	DelegateOnPlayCompleted _delegateOnPlayCompleted;
}

- (void)onLoadCompleted:(int)nativeTextureID_ resultType:(RESULT_TYPE)resultType_ description:(NSString*)description_;
- (void)onPlayCompleted:(int)nativeTextureID_ completedType:(PLAY_COMPLETED_TYPE)completedType_;
@end


extern void UORegisterLogDelegates(DelegateOnLog delegateLog_,
								 DelegateOnLog delegateWarning_,
								 DelegateOnLog delegateError_);
extern void UOEnableLog(bool enable_);

extern void UOLoadAsync(int nativeTextureID_, const char* relativeFilePathEFromStreamingAssets_, DelegateOnLoadCompleted delegateOnLoadCompleted_);
extern void UOBindTexture(int nativeTextureID_);
extern void UORenderObject(int nativeTextureID_);
extern void UOPlay(int nativeTextureID_, DelegateOnPlayCompleted delegateOnPlayCompleted_);
extern void UOStop(int nativeTextureID_);
extern void UOPause(int nativeTextureID_);
extern void UOResume(int nativeTextureID_);
extern void UOSeekTo(int nativeTextureID_, int timeMS_);
extern int UOGetCurrentPosition(int nativeTextureID_);
extern int UOGetDuration(int nativeTextureID_);
extern bool UOIsPaused(int nativeTextureID_);
extern bool UOIsPlaying(int nativeTextureID_);
