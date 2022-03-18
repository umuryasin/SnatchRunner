#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yodo1.MAS;

public class Yodo1EditorAds : MonoBehaviour
{
    public static GameObject AdHolder;
    private static Dictionary<string, GameObject> BannerSampleAdEditor;
    private static Dictionary<string, GameObject> NativeSampleAdEditor;
    private static GameObject InterstitialSampleAdEditor;
    private static GameObject RewardedVideoSampleAdEditor;

    public static bool DisableGUI = false;
    public static bool GrantReward = false;
    public static int TotalBannerCount = 0;

    private static GameObject BannerSampleAdEditorTemp;
    private static GameObject NativeSampleAdEditorTemp;


    public static void InitializeAds()
    {
        BannerSampleAdEditor = new Dictionary<string, GameObject>();
        NativeSampleAdEditor = new Dictionary<string, GameObject>();
        EventSystem sceneEventSystem = FindObjectOfType<EventSystem>();
        if (sceneEventSystem == null)
        {
            AdHolder = Instantiate(Resources.Load("SampleAds/AdHolder") as GameObject);
        }
        Transform highestOrderCanvas = HighestOrderCanvas();
        if (InterstitialSampleAdEditor == null)
        {
            InterstitialSampleAdEditor = Instantiate(Resources.Load("SampleAds/InterstitialSampleAdPanel") as GameObject, highestOrderCanvas);
            InterstitialSampleAdEditor.transform.SetAsLastSibling();
        }
        if (RewardedVideoSampleAdEditor == null)
        {
            RewardedVideoSampleAdEditor = Instantiate(Resources.Load("SampleAds/RewardedVideoSampleAdPanel") as GameObject, highestOrderCanvas);
            RewardedVideoSampleAdEditor.transform.SetAsLastSibling();
        }
    }

    private static Transform HighestOrderCanvas()
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        int length = canvases.Length;
        Transform highestOrderCanvas = canvases[0].transform;
        int highestOrder = canvases[0].sortingOrder;
        for (int i = 1; i < length; i++)
        {
            if (highestOrder < canvases[i].sortingOrder)
            {
                highestOrder = canvases[i].sortingOrder;
                highestOrderCanvas = canvases[i].transform;
            }
        }
        return highestOrderCanvas;
    }
    public static void ShowStamdardBannerAdsInEditor(string IndexId)
    {
        GameObject BannerAd;
        if (!BannerSampleAdEditor.TryGetValue(IndexId, out BannerAd))
        {
            Transform highestOrderTransform = HighestOrderCanvas();
            BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/StandardBannerSampleAdPanel") as GameObject, highestOrderTransform);

            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1f);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1f);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            BannerSampleAdEditorTemp.SetActive(true);

            BannerSampleAdEditor.Add(IndexId, BannerSampleAdEditorTemp);
        }
        else
        {
            BannerAd.SetActive(true);
        }
        Debug.Log(Yodo1U3dMas.TAG + "Editor Banner ad opened");


    }
    private static float anchorMinX, anchorMinY, anchorMaxX, anchorMaxY, pivotX, pivotY, anchoredPositionX, anchoredPositionY;
    public static int tempAlign = 0;


    private static void CalculateAnchoringForStandardBanner(int align, out float anchorMinX, out float anchorMinY, out float anchorMaxX, out float anchorMaxY, out float pivotX, out float pivotY, out float anchoredPositionX, out float anchoredPositionY)
    {
        if (align == 0)
        {
            align = tempAlign;
        }
        tempAlign = align;
        anchorMinX = 0.5f;
        anchorMinY = 1f;
        anchorMaxX = 0.5f;
        anchorMaxY = 1f;
        pivotX = 0.5f;
        pivotY = 1f;
        anchoredPositionX = 0f;
        anchoredPositionY = 0f;
        if ((align & (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter) == (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter)
        {
            anchorMinX = 0.5f;
            anchorMaxX = 0.5f;
            pivotX = 0.5f;
            anchoredPositionX = 0f;

        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerRight) == (int)Yodo1U3dBannerAdPosition.BannerRight)
        {
            anchorMinX = 1f;
            anchorMaxX = 1f;
            pivotX = 0.5f;
            anchoredPositionX = -320f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerLeft) == (int)Yodo1U3dBannerAdPosition.BannerLeft)
        {
            anchorMinX = 0f;
            anchorMaxX = 0f;
            pivotX = 0.5f;
            anchoredPositionX = 320f;
        }

        if ((align & (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter) == (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter)
        {
            anchorMinY = 0.5f;
            anchorMaxY = 0.5f;
            pivotY = 0f;
            anchoredPositionY = -60f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerBottom) == (int)Yodo1U3dBannerAdPosition.BannerBottom)
        {
            anchorMinY = 0f;
            anchorMaxY = 0f;
            pivotY = 1f;
            anchoredPositionY = 120f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerTop) == (int)Yodo1U3dBannerAdPosition.BannerTop)
        {
            anchorMinY = 1f;
            anchorMaxY = 1f;
            pivotY = 1f;
            anchoredPositionY = 0f;
        }
    }
    private static void CalculateAnchoringForLargeBanner(int align, out float anchorMinX, out float anchorMinY, out float anchorMaxX, out float anchorMaxY, out float pivotX, out float pivotY, out float anchoredPositionX, out float anchoredPositionY)
    {
        if (align == 0)
        {
            align = tempAlign;
        }
        tempAlign = align;
        anchorMinX = 0.5f;
        anchorMinY = 1f;
        anchorMaxX = 0.5f;
        anchorMaxY = 1f;
        pivotX = 0.5f;
        pivotY = 0.5f;
        anchoredPositionX = 0f;
        anchoredPositionY = -100f;
        if ((align & (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter) == (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter)
        {
            anchorMinX = 0.5f;
            anchorMaxX = 0.5f;
            pivotX = 0.5f;
            anchoredPositionX = 0f;

        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerRight) == (int)Yodo1U3dBannerAdPosition.BannerRight)
        {
            anchorMinX = 1f;
            anchorMaxX = 1f;
            pivotX = 0.5f;
            anchoredPositionX = -320f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerLeft) == (int)Yodo1U3dBannerAdPosition.BannerLeft)
        {
            anchorMinX = 0f;
            anchorMaxX = 0f;
            pivotX = 0.5f;
            anchoredPositionX = 320f;
        }

        if ((align & (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter) == (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter)
        {
            anchorMinY = 0.5f;
            anchorMaxY = 0.5f;
            pivotY = 0.5f;
            anchoredPositionY = 0f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerBottom) == (int)Yodo1U3dBannerAdPosition.BannerBottom)
        {
            anchorMinY = 0f;
            anchorMaxY = 0f;
            pivotY = 0.5f;
            anchoredPositionY = 100f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerTop) == (int)Yodo1U3dBannerAdPosition.BannerTop)
        {
            anchorMinY = 1f;
            anchorMaxY = 1f;
            pivotY = 0.5f;
            anchoredPositionY = -100f;
        }
    }
    private static void CalculateAnchoringForIABBanner(int align, out float anchorMinX, out float anchorMinY, out float anchorMaxX, out float anchorMaxY, out float pivotX, out float pivotY, out float anchoredPositionX, out float anchoredPositionY)
    {
        if (align == 0)
        {
            align = tempAlign;
        }
        tempAlign = align;
        anchorMinX = 0.5f;
        anchorMinY = 1f;
        anchorMaxX = 0.5f;
        anchorMaxY = 1f;
        pivotX = 0.5f;
        pivotY = 0.5f;
        anchoredPositionX = 0f;
        anchoredPositionY = -250f;
        if ((align & (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter) == (int)Yodo1U3dBannerAdPosition.BannerHorizontalCenter)
        {
            anchorMinX = 0.5f;
            anchorMaxX = 0.5f;
            pivotX = 0.5f;
            anchoredPositionX = 0f;

        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerRight) == (int)Yodo1U3dBannerAdPosition.BannerRight)
        {
            anchorMinX = 1f;
            anchorMaxX = 1f;
            pivotX = 0.5f;
            anchoredPositionX = -320f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerLeft) == (int)Yodo1U3dBannerAdPosition.BannerLeft)
        {
            anchorMinX = 0f;
            anchorMaxX = 0f;
            pivotX = 0.5f;
            anchoredPositionX = 320f;
        }

        if ((align & (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter) == (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter)
        {
            anchorMinY = 0.5f;
            anchorMaxY = 0.5f;
            pivotY = 0.5f;
            anchoredPositionY = 0f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerBottom) == (int)Yodo1U3dBannerAdPosition.BannerBottom)
        {
            anchorMinY = 0f;
            anchorMaxY = 0f;
            pivotY = 0.5f;
            anchoredPositionY = 250f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerTop) == (int)Yodo1U3dBannerAdPosition.BannerTop)
        {
            anchorMinY = 1f;
            anchorMaxY = 1f;
            pivotY = 0.5f;
            anchoredPositionY = -250f;
        }
    }
    private static void CalculateAnchoringForAdaptiveBanner(int align, out float anchorMinX, out float anchorMinY, out float anchorMaxX, out float anchorMaxY, out float pivotX, out float pivotY, out float anchoredPositionX, out float anchoredPositionY)
    {
        if (align == 0)
        {
            align = tempAlign;
        }
        tempAlign = align;
        anchorMinX = 0f;
        anchorMinY = 1f;
        anchorMaxX = 1f;
        anchorMaxY = 1f;
        pivotX = 0.5f;
        pivotY = 0.5f;
        anchoredPositionX = 0f;
        anchoredPositionY = -60f;

        if ((align & (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter) == (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter)
        {
            anchorMinY = 0.5f;
            anchorMaxY = 0.5f;
            pivotY = 0.5f;
            anchoredPositionY = 0f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerBottom) == (int)Yodo1U3dBannerAdPosition.BannerBottom)
        {
            anchorMinY = 0f;
            anchorMaxY = 0f;
            pivotY = 0.5f;
            anchoredPositionY = 60f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerTop) == (int)Yodo1U3dBannerAdPosition.BannerTop)
        {
            anchorMinY = 1f;
            anchorMaxY = 1f;
            pivotY = 0.5f;
            anchoredPositionY = -60f;
        }
    }
    private static void CalculateAnchoringForSmartBanner(int align, out float anchorMinX, out float anchorMinY, out float anchorMaxX, out float anchorMaxY, out float pivotX, out float pivotY, out float anchoredPositionX, out float anchoredPositionY)
    {
        if (align == 0)
        {
            align = tempAlign;
        }
        tempAlign = align;
        anchorMinX = 0f;
        anchorMinY = 1f;
        anchorMaxX = 1f;
        anchorMaxY = 1f;
        pivotX = 0.5f;
        pivotY = 0.5f;
        anchoredPositionX = 0f;
        anchoredPositionY = -40f;

        if ((align & (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter) == (int)Yodo1U3dBannerAdPosition.BannerVerticalCenter)
        {
            anchorMinY = 0.5f;
            anchorMaxY = 0.5f;
            pivotY = 0.5f;
            anchoredPositionY = 0f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerBottom) == (int)Yodo1U3dBannerAdPosition.BannerBottom)
        {
            anchorMinY = 0f;
            anchorMaxY = 0f;
            pivotY = 0.5f;
            anchoredPositionY = 40f;
        }
        else if ((align & (int)Yodo1U3dBannerAdPosition.BannerTop) == (int)Yodo1U3dBannerAdPosition.BannerTop)
        {
            anchorMinY = 1f;
            anchorMaxY = 1f;
            pivotY = 0.5f;
            anchoredPositionY = -40f;
        }
    }
    public static void ShowBannerAdsInEditor(string IndexId, int align, int size, int offsetX, int offsetY)
    {
        GameObject BannerAd;
        if (!BannerSampleAdEditor.TryGetValue(IndexId, out BannerAd))
        {
            Transform highestOrderTransform = HighestOrderCanvas();
            if (size == 0)
            {
                
                BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/StandardBannerSampleAdPanel") as GameObject, highestOrderTransform);
                CalculateAnchoringForStandardBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

            }
            else if (size == 1)
            {
                BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/LargeBanner") as GameObject, highestOrderTransform);
                CalculateAnchoringForLargeBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

            }
            else if (size == 2)
            {
                BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/IABMediumRectangleBanner") as GameObject, highestOrderTransform);
                CalculateAnchoringForIABBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

            }
            else if (size == 3)
            {

                string[] res = UnityStats.screenRes.Split('x');
                if (int.Parse(res[1]) > int.Parse(res[0]))
                {
                    BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/SmartBannerPortrait") as GameObject, highestOrderTransform);
                    CalculateAnchoringForAdaptiveBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

                }
                else
                {
                    BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/SmartBannerLandscape") as GameObject, highestOrderTransform);
                    CalculateAnchoringForSmartBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

                }
            }
            else if (size == 4)
            {
                BannerSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/AdaptiveBanner") as GameObject, highestOrderTransform);
                CalculateAnchoringForAdaptiveBanner(align, out anchorMinX, out anchorMinY, out anchorMaxX, out anchorMaxY, out pivotX, out pivotY, out anchoredPositionX, out anchoredPositionY);

            }
            //BannerSampleAdEditorTemp.transform.SetAsFirstSibling();
            BannerSampleAdEditorTemp.transform.SetSiblingIndex(BannerSampleAdEditorTemp.transform.parent.childCount - 3);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchorMin = new Vector2(anchorMinX, anchorMinY);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchorMax = new Vector2(anchorMaxX, anchorMaxY);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
            BannerSampleAdEditorTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(anchoredPositionX, anchoredPositionY);
            BannerSampleAdEditorTemp.SetActive(true);
            BannerSampleAdEditor.Add(IndexId, BannerSampleAdEditorTemp);
        }
        else
        {
            BannerAd.SetActive(true);
        }
        Debug.Log(Yodo1U3dMas.TAG + "Editor Banner ad opened");
    }

    public static void ShowNativeAdsInEditor(string IndexId, int width, int height, int offsetX, int offsetY,Color colorVal)
    {
        GameObject NativeAd;
        if (!NativeSampleAdEditor.TryGetValue(IndexId, out NativeAd))
        {
            string[] res = UnityStats.screenRes.Split('x');
            if(width > int.Parse(res[0]))
            {
                width = int.Parse(res[0]);
            }
            if (height > int.Parse(res[1]))
            {
                height = int.Parse(res[1]);
            }
            int xVal = (width / 2) + offsetX;
            int yVal = -(height / 2) - offsetY;
            Transform highestOrderTransform = HighestOrderCanvas();
            if ((width / height) > 3.5)
            {
                NativeSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/TemplateNativeAdSmall") as GameObject, highestOrderTransform);
                if (height > 60 && width > 360)
                {
                    NativeSampleAdEditorTemp.GetComponent<RectTransform>().localScale = new Vector2((float)width / 360, (float)height / 60);
                    
                }
                else
                {
                    NativeSampleAdEditorTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

                }

            }
            else
            {
                NativeSampleAdEditorTemp = Instantiate(Resources.Load("SampleAds/TemplateNativeAdMedium") as GameObject, highestOrderTransform);
                if (height > 300 && width > 360)
                {
                    NativeSampleAdEditorTemp.GetComponent<RectTransform>().localScale = new Vector2((float)width / 360, (float)height / 300);
                    
                }
                else
                {
                    NativeSampleAdEditorTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                }
                if(height < 200)
                {
                    NativeSampleAdEditorTemp.transform.GetChild(0).gameObject.SetActive(false);
                    NativeSampleAdEditorTemp.transform.GetChild(2).gameObject.SetActive(false);
                    NativeSampleAdEditorTemp.transform.GetChild(4).gameObject.SetActive(false);
                }
            }
            
            //NativeSampleAdEditorTemp.transform.SetAsFirstSibling();
            NativeSampleAdEditorTemp.transform.SetSiblingIndex(NativeSampleAdEditorTemp.transform.parent.childCount - 3);

            NativeSampleAdEditorTemp.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            NativeSampleAdEditorTemp.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            NativeSampleAdEditorTemp.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            
            NativeSampleAdEditorTemp.GetComponent<Image>().color = colorVal;

            
            if (!((width / height) > 3.5))
            {
                if (xVal < 85)
                {
                    xVal = 85;
                }
                if (yVal > -70)
                {
                    yVal = -70;
                }
            }
            NativeSampleAdEditorTemp.GetComponent<RectTransform>().anchoredPosition = new Vector2(xVal,yVal);
            NativeSampleAdEditorTemp.SetActive(true);
            NativeSampleAdEditor.Add(IndexId, NativeSampleAdEditorTemp);
        }
        else
        {
            NativeAd.SetActive(true);
        }
        Debug.Log(Yodo1U3dMas.TAG + "Editor Native ad opened");
    }

    public static void ShowInterstitialAdsInEditor()
    {
        if (InterstitialSampleAdEditor != null)
        {
            Debug.Log("Showing inters ads");
            DisableGUI = true;
            InterstitialSampleAdEditor.SetActive(true);
            Yodo1U3dMasCallback.ForwardEvent("onInterstitialAdOpenedEvent");
        }
        else
        {
            Debug.Log("InterstitialSampleAdEditor is null");

        }
    }
    public static void ShowRewardedVideodsInEditor()
    {
        if (RewardedVideoSampleAdEditor != null)
        {
            DisableGUI = true;
            RewardedVideoSampleAdEditor.SetActive(true);
            Yodo1U3dMasCallback.ForwardEvent("onRewardedAdOpenedEvent");
        }
    }
    public static void HideBannerAdsInEditor(string IndexId)
    {
        if (BannerSampleAdEditor != null)
        {
            GameObject BannerAd;
            if (BannerSampleAdEditor.TryGetValue(IndexId, out BannerAd))
            {

                Debug.Log(Yodo1U3dMas.TAG + "Editor Banner ad closed");
                if (BannerSampleAdEditor[IndexId] != null)
                {
                    BannerSampleAdEditor[IndexId].SetActive(false);
                }

            }

        }
    }
    public static void DestroyBannerAdsInEditor(string IndexId)
    {
        if (BannerSampleAdEditor != null)
        {
            GameObject BannerAd;
            if (BannerSampleAdEditor.TryGetValue(IndexId, out BannerAd))
            {
                Debug.Log(Yodo1U3dMas.TAG + "Editor Banner ad destroyed");
                Destroy(BannerSampleAdEditor[IndexId]);
                BannerSampleAdEditor.Remove(IndexId);

            }


        }
    }
    public static void CloseInterstitialAdsInEditor()
    {
        if (InterstitialSampleAdEditor != null)
        {
            DisableGUI = false;
            InterstitialSampleAdEditor.SetActive(false);
            Debug.Log("inter close event: InterstitialSampleAdEditor");
            Yodo1U3dMasCallback.ForwardEvent("onInterstitialAdClosedEvent");
        }
        else
            Debug.Log("cant catch inter close event: InterstitialSampleAdEditor=null");
    }
    public static void CloseRewardedVideodsInEditor()
    {
        if (RewardedVideoSampleAdEditor != null)
        {
            DisableGUI = false;
            RewardedVideoSampleAdEditor.SetActive(false);
            if (GrantReward)
            {
                Yodo1U3dMasCallback.ForwardEvent("onRewardedAdReceivedRewardEvent");
                GrantReward = false;
            }
            Yodo1U3dMasCallback.ForwardEvent("onRewardedAdClosedEvent");
        }
    }
    public static void GetRewardsInEditor()
    {
        GrantReward = true;
    }

    public static void HideNativeAdsInEditor(string IndexId)
    {
        if (NativeSampleAdEditor != null)
        {
            GameObject NativeAd;
            if (NativeSampleAdEditor.TryGetValue(IndexId, out NativeAd))
            {

                Debug.Log(Yodo1U3dMas.TAG + "Editor Native ad hidden");
                if (NativeSampleAdEditor[IndexId] != null)
                {
                    NativeSampleAdEditor[IndexId].SetActive(false);
                }

            }

        }
    }
    public static void DestroyNativeAdsInEditor(string IndexId)
    {
        if (NativeSampleAdEditor != null)
        {
            GameObject NativeAd;
            if (NativeSampleAdEditor.TryGetValue(IndexId, out NativeAd))
            {
                Debug.Log(Yodo1U3dMas.TAG + "Editor Native ad destroyed");
                Destroy(NativeSampleAdEditor[IndexId]);
                NativeSampleAdEditor.Remove(IndexId);

            }


        }
    }
}
#endif