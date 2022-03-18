using UnityEngine;
using Yodo1.MAS;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TemplateNativeAd : MonoBehaviour
{
    
    [Header("Native Ad X position")]
    [Tooltip("Please Select X-coordinate in pixels. The origin is the top-left of the screen.")]
    public int XPosition = 0;
    [Header("Native Ad Y position")]
    [Tooltip("Please Select Y-coordinate in pixels. The origin is the top-left of the screen.")]
    public int YPosition = 0;
    [Header("Native Ad width")]
    [Tooltip("If native ad type is small, the ratio should be 3:1, if medium, it should be 6:5.")]
    public int width = 950;
    [Header("Native Ad height")]
    [Tooltip("If native ad type is small, the ratio should be 3:1, if medium, it should be 6:5.")]
    public int height = 600;

    [Header("Show Native ad on all scenes")]
    [Tooltip("Check this if you want to show this native ad on all scenes.")]
    public bool nativeAdOnAllScenes = true;

    [Header("Native ad Placement (Optional) ")]
    public string PlacementID = string.Empty;
    [Header("Native ad background color (Optional) ")]
    public Color backGroundColor = Color.black;
    [Space(10)]
    [Header("Native AD Events (optional) ")]
    [SerializeField] UnityEvent OnNativeAdLoaded;
    [SerializeField] UnityEvent OnNativeAdFailedToLoad;

    Yodo1U3dNativeAdView nativeAdView = null;


    [Header("Load Native Ad manually or use buttons in Unity editor")]
    public bool LoadManually = false;
    [ConditionalHide("LoadManually", true)]
    public Button ShowNativeAdButton;
    [ConditionalHide("LoadManually", true)]
    public Button HideNativeAdButton;
    [ConditionalHide("LoadManually", true)]
    public Button DestroyNativeAdButton;

    private void Start()
    {
        if (!LoadManually)
        {
            Invoke("LoadNativeAd", 2f);
        }
        else
        {
            if (ShowNativeAdButton != null)
            {
                ShowNativeAdButton.onClick.AddListener(() => { LoadNativeAd(); });
            }
            if (HideNativeAdButton != null)
            {
                HideNativeAdButton.onClick.AddListener(() => {
                    if (nativeAdView != null)
                    {
                        nativeAdView.Hide();
                    }
                });
            }
            if (DestroyNativeAdButton != null)
            {
                DestroyNativeAdButton.onClick.AddListener(() => {
                    if (nativeAdView != null)
                    {
                        nativeAdView.Destroy();
                    }
                });
            }
        }
#if UNITY_EDITOR
        if (nativeAdOnAllScenes)
        {
            if (transform.parent != null)
            {
                DontDestroyOnLoad(transform.parent);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

#endif
    }
    private void OnEnable()
    {
#if UNITY_EDITOR
        SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        if (nativeAdView != null)
        {
            nativeAdView.OnAdLoadedEvent += OnNativeAdLoadedEvent;
            nativeAdView.OnAdFailedToLoadEvent += OnNativeAdFailedToLoadEvent;

            nativeAdView.Show();
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
        if (nativeAdView != null)
        {
            nativeAdView.OnAdLoadedEvent -= OnNativeAdLoadedEvent;
            nativeAdView.OnAdFailedToLoadEvent -= OnNativeAdFailedToLoadEvent;

            nativeAdView.Hide();
        }
    }

    private void OnDestroy()
    {
        if (!nativeAdOnAllScenes)
        {
            if (nativeAdView != null)
            {
                nativeAdView.Destroy();
                nativeAdView = null;
            }
        }
    }
    private void LoadNativeAd()
    {
        // Clean up native before reusing
        if (nativeAdView != null)
        {
            nativeAdView.Destroy();
            nativeAdView = null;
        }

        nativeAdView = new Yodo1U3dNativeAdView(XPosition, YPosition, width, height);
        if (!string.IsNullOrEmpty(PlacementID))
        {
            nativeAdView.SetAdPlacement(PlacementID);
        }
        nativeAdView.SetBackgroundColor(backGroundColor);
        // Add Events
        nativeAdView.OnAdLoadedEvent += OnNativeAdLoadedEvent;
        nativeAdView.OnAdFailedToLoadEvent += OnNativeAdFailedToLoadEvent;

        nativeAdView.LoadAd();
    }
#if UNITY_EDITOR
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (nativeAdView != null)
        {
            if (nativeAdOnAllScenes)
            {
                int sceneNumber = SceneManager.GetActiveScene().buildIndex;
                if (sceneNumber != 0)
                {
                    if (!LoadManually)
                    {
                        LoadNativeAd();
                    }
                }
            }
        }
    }
#endif
    private void OnNativeAdLoadedEvent(Yodo1U3dNativeAdView adView)
    {
        // Banner ad is ready to be shown.
        Debug.Log(Yodo1U3dMas.TAG + "Native ad loaded");
        OnNativeAdLoaded.Invoke();
    }

    private void OnNativeAdFailedToLoadEvent(Yodo1U3dNativeAdView adView, Yodo1U3dAdError adError)
    {
        Debug.Log(Yodo1U3dMas.TAG + "Native ad failed to load with error code: " + adError.ToString());
        OnNativeAdFailedToLoad.Invoke();
    }
}
