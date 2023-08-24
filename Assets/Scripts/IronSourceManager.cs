using com.F4A.MobileThird;
using System;
using UnityEngine;

public class IronSourceManager : MonoBehaviour
{
	private bool isRewarded;

	private Action success;

	public static IronSourceManager ins;

	private void Awake()
	{
		ins = this;
	}

	public void onShowVideoReward()
	{
        AdsManager.Instance.ShowRewardAds();
	}

	public void onShowInterstitiald()
	{
        AdsManager.Instance.ShowInterstitialAds();
	}

	public void onShowVideoReward(Action success, Action fail)
	{
		if (videoRewardIsReady())
		{
			this.success = success;
			onShowVideoReward();
		}
		else
		{
			fail();
		}
	}

	public bool videoRewardIsReady()
	{
        return AdsManager.Instance.IsRewardAdsReady();
	}
}
