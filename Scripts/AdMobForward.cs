using GoogleMobileAds.Api;
using UnityEngine;

public class AdMobForward : MonoBehaviour
{
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-9996067309099965/5150343499";//���� ���� id:ca-app-pub-9996067309099965/5150343499, �׽�Ʈ�� : ca-app-pub-3940256099942544/1033173712
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif

    private InterstitialAd _interstitialAd;

    //�� ����� �ѹ��� �ʱ�ȭ �ϸ��
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        //�� ���۵Ǹ� �̸� �ε�. ������ 1�ð�
        LoadInterstitialAd();
    }

    //Loads the interstitial ad
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    //Shows the interstitial ad.
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            _interstitialAd.Show();
        }
    }
}