using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public enum WatchAdResult { Finished, Failed, Skipped}
public class UnityAds : MonoBehaviour , IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    //delegate   ()
    public delegate void RewardedAdResult(WatchAdResult result);

    //event  
    public static event RewardedAdResult AdResult;

    public static UnityAds Instance;

    [Header("UNITY AD SETUP")]
    public string UNITY_ANDROID_ID = "1486550";
    public string UNITY_IOS_ID = "1486551";
    public bool isTestMode = true;
    public bool checkAds = false;

    private void Awake()
    {
        if (UnityAds.Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        string gameId = "";
#if UNITY_IOS
		gameId = UNITY_IOS_ID;
#elif UNITY_ANDROID
        gameId = UNITY_ANDROID_ID;
#endif
        if (Advertisement.isSupported)
        {
            //Advertisement.Initialize(gameId, isTestMode);
        }
    }



    #region NORMAL AD
    public void ShowNormalAd()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(UNITY_ANDROID_ID);
        }
    }

    public bool ForceShowNormalAd()
    {
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(UNITY_ANDROID_ID);
            return true;
        }
        else
            return false;
    }

    #endregion

    #region REWARD AD
    public bool isRewardedAdReady()
    {
        return checkAds;
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        checkAds = true;
    }

    public void ShowRewardVideo()
    {
        ShowRewardedAd();
    }

    private void ShowRewardedAd()
    {
        if (!allowWatch)
            return;

        if (checkAds)
        {
                //var options = new ShowOptions {  gamerSid = HandleShowResult };
                //if (!Advertisement.isShowing)
                //    Advertisement.Show("rewardedVideo", options);

                allowWatch = false;
            
        }
    }

    bool allowWatch = true;
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                AdResult(WatchAdResult.Finished);
                ; break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                AdResult(WatchAdResult.Skipped);
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                AdResult(WatchAdResult.Failed);
                break;
        }

        allowWatch = true;
    }

    public void OnInitializationComplete()
    {

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
