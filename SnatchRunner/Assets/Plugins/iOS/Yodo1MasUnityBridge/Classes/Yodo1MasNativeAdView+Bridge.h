//
//  Yodo1MasNativeAdView+Bridge.h
//  UnityFramework
//
//  Created by 周玉震 on 2021/11/28.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "Yodo1MasNativeAdView.h"

NS_ASSUME_NONNULL_BEGIN

@interface Yodo1MasBridgeNativeAdConfig : NSObject

+ (Yodo1MasBridgeNativeAdConfig *)parse:(id)json;

@property (nonatomic, assign) CGFloat x;
@property (nonatomic, assign) CGFloat y;
@property (nonatomic, assign) CGFloat width;
@property (nonatomic, assign) CGFloat height;
@property (nonatomic, copy) NSString *adPlacement;
@property (nonatomic, copy) NSString *indexId;
@property (nonatomic, copy) NSString *backgroundColor;

@end

@interface Yodo1MasNativeAdView(Bridge)

@property (nonatomic, strong) Yodo1MasBridgeNativeAdConfig *yodo1_config;

@end

NS_ASSUME_NONNULL_END
