using com.F4A.MobileThird;
using System;
using UnityEngine;

public class VideoRewardController : MonoBehaviour
{
    private static event Action onSuccess = delegate { };
    private static event Action onFail = delegate { };
	public static void onShowVideoReward(Action success, Action fail)
	{
        onSuccess = success;
        onFail = fail;

        AdsManager.Instance.ShowRewardAds();
	}

    private void OnEnable()
    {
        AdsManager.OnRewardedAdCompleted += AdsManager_OnRewardedAdCompleted;
        AdsManager.OnRewardedAdFailed += AdsManager_OnRewardedAdFailed;
    }

    private void AdsManager_OnRewardedAdFailed(ERewardedAdNetwork adNetwork)
    {
        onFail?.Invoke();
    }

    private void AdsManager_OnRewardedAdCompleted(ERewardedAdNetwork adNetwork)
    {
        onSuccess?.Invoke();
    }

    private void OnDisable()
    {
        AdsManager.OnRewardedAdCompleted -= AdsManager_OnRewardedAdCompleted;
        AdsManager.OnRewardedAdFailed -= AdsManager_OnRewardedAdFailed;
    }
}