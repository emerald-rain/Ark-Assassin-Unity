#if ENABLE_FIREBASE
using Firebase;
using Firebase.Analytics;
#endif
using System.Threading.Tasks;
using UnityEngine;

public class FireBaseAnalyticControl : MonoBehaviour
{
	public static FireBaseAnalyticControl ins;

	private void Awake()
	{
		ins = this;
	}

	private void Start()
	{
		Init();
	}

	private void Init()
	{
#if ENABLE_FIREBASE
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
		{
			DependencyStatus result = task.Result;
			if (result == DependencyStatus.Available)
			{
				analyticLogin();
			}
			else
			{
				UnityEngine.Debug.LogError($"Could not resolve all Firebase dependencies: {result}");
			}
		});
#endif
	}

	private void analyticLogin()
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
#endif
	}

	public void AnalyticLevelPassed(int level)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("level_passed", new Parameter("level_id", level));
#endif
	}

	public void AnalyticLevelFailed(int level)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("level_failed", new Parameter("level_id", level));
#endif
	}

	public void AnalyticClickBuyIAP(string id)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("click_to_iap", FirebaseAnalytics.ParameterItemId, id);
#endif
	}

	public void AnalyticCancelBuyIAP(string id)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchaseRefund, FirebaseAnalytics.ParameterItemId, id);
#endif
	}

	public void AnalyticWatchVideoReward(string _name)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("watch_video_play : " + _name);
#endif
	}

	public void AnalyticWatchVideoReward_notReady()
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("Video Not Ready");
#endif
	}

	public void AnalyticPlaySpin()
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("Play spin");
#endif
	}

	public void AnalyticUpgradeHero(string nameHero, int level)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("Upgrade Hero :", new Parameter("name hero : ", nameHero), new Parameter("level : ", level));
#endif
	}

	public void AnalyticBuyArchery(int id)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("Buy Archery id : " + id);
#endif
	}

	public void AnalyticLogSpin(string content)
	{
#if ENABLE_FIREBASE
		FirebaseAnalytics.LogEvent("content");
#endif
	}
}
