using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.AppLovin;
using System;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
    public static InterstitialAd interstitialAd;
    private string interstitialAd_ID;

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_EDITOR
        interstitialAd_ID = "ca-app-pub-3940256099942544/1033173712"; //testID
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
        interstitialAd_ID = "";
#endif
        AppLovin.SetHasUserConsent(true);
        AppLovin.Initialize();
        MobileAds.Initialize(initStatus => { });

        RequestInterstitialAd();
    }

    private void RequestInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(interstitialAd_ID);
        interstitialAd.OnAdLoaded += HandleInterstitialLoaded;
        interstitialAd.OnAdOpening += HandleInterstitialOpening;
        interstitialAd.OnAdClosed += HandleInterstitialClosed;
        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);

    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            RequestInterstitialAd();
            SceneManager.LoadScene("Game");
        }
    }

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {

    }
    public void HandleInterstitialOpening(object sender, EventArgs args)
    {
        
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        RequestInterstitialAd();
        PlayerPrefs.SetInt("loadCount", 0);
        SceneManager.LoadScene("Game");
    }
}
