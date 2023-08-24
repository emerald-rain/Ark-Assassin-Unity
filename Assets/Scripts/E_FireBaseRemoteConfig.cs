#if ENABLE_FIREBASE
using Firebase;
using Firebase.RemoteConfig;
#endif
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class E_FireBaseRemoteConfig : MonoBehaviour
{
#if ENABLE_FIREBASE
	private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif
    private void Start()
    {
#if ENABLE_FIREBASE
		FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(delegate(Task<DependencyStatus> task)
		{
			dependencyStatus = task.Result;
			if (dependencyStatus == DependencyStatus.Available)
			{
				InitializeFirebase();
			}
			else
			{
				UnityEngine.Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
			}
		});
#endif
    }

    private void InitializeFirebase()
    {
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        dictionary.Add("config_test_string", "default local string");
        dictionary.Add("config_test_int", 1);
        dictionary.Add("config_test_float", 1.0);
        dictionary.Add("config_test_bool", false);
#if ENABLE_FIREBASE
		FirebaseRemoteConfig.SetDefaults(dictionary);
		UnityEngine.Debug.Log("Remote config ready!");
		FetchFireBase();
		UnityEngine.Debug.Log("_________________ " + FirebaseRemoteConfig.GetValue("Num_Watch_Video_To_Spin").StringValue);
#endif
    }

    public void FetchFireBase()
    {
#if ENABLE_FIREBASE
        FetchDataAsync();
#endif
    }

    private void Update()
    {
        if (UnityEngine.Input.GetKey(KeyCode.A))
        {
            ShowData();
        }
    }

    public void ShowData()
    {
#if ENABLE_FIREBASE
		UnityEngine.Debug.Log("1111111111111: " + FirebaseRemoteConfig.GetValue("Num_Watch_Video_To_Spin").StringValue);
		UnityEngine.Debug.Log("2222222222222: " + FirebaseRemoteConfig.GetValue("Num_Watch_Video_To_Spin").DoubleValue);
		UnityEngine.Debug.Log("33333333333333: " + FirebaseRemoteConfig.GetValue("Num_Watch_Video_To_Spin").Source);
		UnityEngine.Debug.Log("44444444444444: " + FirebaseRemoteConfig.GetValue("Num_Watch_Video_To_Spin").LongValue);
#endif
    }

#if ENABLE_FIREBASE
	public Task FetchDataAsync()
	{
		UnityEngine.Debug.Log("Fetching data...");
		Task task = FirebaseRemoteConfig.FetchAsync(TimeSpan.Zero);
		return task.ContinueWith(FetchComplete);
	}
#endif

#if ENABLE_FIREBASE
    private void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            UnityEngine.Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            UnityEngine.Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            UnityEngine.Debug.Log("Fetch completed successfully!");
        }
        ConfigInfo info = FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.ActivateFetched();
                UnityEngine.Debug.Log($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        UnityEngine.Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        UnityEngine.Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                UnityEngine.Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }
#endif
}
