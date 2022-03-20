using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yodo1.MAS;


public class AdsManager : MonoBehaviour
{
    public delegate void AdsDelegate();
    public static AdsDelegate OnRewardedAdWatched;
    public static AdsDelegate OnInterstitialClosed;
    public static AdsDelegate OnRewardedClosed;
    public int InterstitialAdShowRatio = 3;

    private Yodo1U3dBannerAdView BannerAdView;
    private bool IsAdsInitialized = false;
    private bool isInterstitialInitialized = false;
    private bool isRewardedAdInitialized = false;

    private int InterstitialAdsShowCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsAdsInitialized)
        {
            // Initialize the MAS SDK.
            Yodo1U3dMas.InitializeSdk();
            IsAdsInitialized = true;
            Debug.Log(DateTime.Now + " : doesluck[Yodo1 Mas] Initializing Ads");
        }
    }

    private void OnEnable()
    {
        MenuManager.OnMenuLoaded += MenuManager_OnMenuLoaded;
        InputManager.OnShowAds += InputManager_OnShowAds;
        GameManager.onGameStateChanged += GameManager_onGameStateChanged;
        InitializeAds();

    }
    private void OnDisable()
    {
        MenuManager.OnMenuLoaded -= MenuManager_OnMenuLoaded;
        InputManager.OnShowAds -= InputManager_OnShowAds;
        GameManager.onGameStateChanged -= GameManager_onGameStateChanged;
    }
    private void GameManager_onGameStateChanged(GameStates obj)
    {

    }

    private void InputManager_OnShowAds()
    {
        InterstitialAdsShowCounter++;
        if(InterstitialAdsShowCounter>=InterstitialAdShowRatio)
        {
            InterstitialAdsShowCounter = 0;
            ShowInterstitialAd();
        }
        else
            OnInterstitialClosed?.Invoke();
    }

    private void MenuManager_OnMenuLoaded()
    {
        RequestBanner();
    }




    private void InitializeAds()
    {
       
        /// Initialize ad units
        //this.RequestBanner();
        this.InitializeInterstitialAds();
        this.InitializeRewardedAds();
    }
    #region Banner Ads

    private void RequestBanner()
    {
        // Clean up banner before reusing
        if (BannerAdView != null)
        {
            BannerAdView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        BannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.SmartBanner, Yodo1U3dBannerAdPosition.BannerBottom);
        // Ad Events
        BannerAdView.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        BannerAdView.OnAdFailedToLoadEvent += OnBannerAdFailedToLoadEvent;
        BannerAdView.OnAdOpenedEvent += OnBannerAdOpenedEvent;
        BannerAdView.OnAdFailedToOpenEvent += OnBannerAdFailedToOpenEvent;
        BannerAdView.OnAdClosedEvent += OnBannerAdClosedEvent;

        // Load banner ads, the banner ad will be displayed automatically after loaded
        BannerAdView.LoadAd();
    }
    private void DestroyBanner()
    {
        BannerAdView.Destroy();
        BannerAdView = null;
    }

    private void OnBannerAdLoadedEvent(Yodo1U3dBannerAdView adView)
    {
        // Banner ad is ready to be shown.
        Debug.Log(DateTime.Now + " : doesluck[Yodo1 Mas] OnBannerAdLoadedEvent event received");
    }

    private void OnBannerAdFailedToLoadEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] OnBannerAdFailedToLoadEvent event received with error: " + adError.ToString());
    }

    private void OnBannerAdOpenedEvent(Yodo1U3dBannerAdView adView)
    {
        Debug.Log(DateTime.Now + " : doesluck [Yodo1 Mas] OnBannerAdOpenedEvent event received");

    }

    private void OnBannerAdFailedToOpenEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] OnBannerAdFailedToOpenEvent event received with error: " + adError.ToString());
    }

    private void OnBannerAdClosedEvent(Yodo1U3dBannerAdView adView)
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] OnBannerAdClosedEvent event received");
    }

    #endregion

    #region Interstitial Ads

    private void InitializeInterstitialAds()
    {
        if (isInterstitialInitialized)
            return;
        Yodo1U3dMasCallback.Interstitial.OnAdOpenedEvent +=
        OnInterstitialAdOpenedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdClosedEvent +=
        OnInterstitialAdClosedEvent;
        Yodo1U3dMasCallback.Interstitial.OnAdErrorEvent +=
        OnInterstitialAdErorEvent;
        isInterstitialInitialized = true;
    }

    public void ShowInterstitialAd()
    {
        bool isLoaded = Yodo1U3dMas.IsInterstitialAdLoaded();
        Debug.Log("is inter loaded: " + isLoaded);
        if (isLoaded)
            Yodo1U3dMas.ShowInterstitialAd();
        else
            OnInterstitialClosed?.Invoke();
    }

    private void OnInterstitialAdOpenedEvent()
    {
        Debug.Log(DateTime.Now + " : doesluck[Yodo1 Mas] Interstitial ad opened");
    }

    private void OnInterstitialAdClosedEvent()
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] Interstitial ad closed");
        Debug.Log("ads restart game");
        GameManager.Instance.RestartGame();

    }

    private void OnInterstitialAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] Interstitial ad error - " + adError.ToString());
    }

    #endregion

    #region Rewarded Video Ads

    private void InitializeRewardedAds()
    {
        if (isRewardedAdInitialized)
            return;
        // Add Events
        Yodo1U3dMasCallback.Rewarded.OnAdOpenedEvent += OnRewardedAdOpenedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdClosedEvent += OnRewardedAdClosedEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdReceivedRewardEvent += OnAdReceivedRewardEvent;
        Yodo1U3dMasCallback.Rewarded.OnAdErrorEvent += OnRewardedAdErorEvent;
        isRewardedAdInitialized = true;
    }

    public void ShowRewardedVideo()
    {
        bool isLoaded = Yodo1U3dMas.IsRewardedAdLoaded();
        if (isLoaded)
            Yodo1U3dMas.ShowRewardedAd();
    }

    private void OnRewardedAdOpenedEvent()
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] Rewarded ad opened");
    }

    private void OnRewardedAdClosedEvent()
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] Rewarded ad closed");
        //GameManager.Instance.AdsContinue();
    }

    private void OnAdReceivedRewardEvent()
    {
        Debug.Log(DateTime.Now + " : doesluck[Yodo1 Mas] Rewarded ad received reward");
        OnRewardedAdWatched?.Invoke();
    }

    private void OnRewardedAdErorEvent(Yodo1U3dAdError adError)
    {
        Debug.Log(DateTime.Now + " :doesluck [Yodo1 Mas] Rewarded ad error - " + adError.ToString());
    }


    #endregion
}
