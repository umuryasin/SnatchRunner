using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void InputManagerDelegate();
    public static event InputManagerDelegate OnShowAds;


    private void OnEnable()
    {
        AdsManager.OnInterstitialClosed += OnInterstitialClosed;
        AdsManager.OnRewardedClosed += OnInterstitialClosed;
    }
    private void OnDisable()
    {
        AdsManager.OnInterstitialClosed -= OnInterstitialClosed;
        AdsManager.OnRewardedClosed -= OnInterstitialClosed;
    }
    private void OnInterstitialClosed()
    {
        GameManager.Instance.RestartGame();
    }
    public void StartGame()
    {
        Debug.Log("starttttt");
        GameManager.Instance.StartGame();
    }

    public void ResetGame()
    {
        OnShowAds?.Invoke();
    }

    public void NextLevel()
    {

        Debug.Log(" input next level");
        GameManager.Instance.NextLevel();
    }
}
