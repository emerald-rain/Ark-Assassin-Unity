using System;
using UnityEngine;

public class AppsFlyerManager
{
	public static string devKey = "Gio8zFFVzAZJGr9sbHXoVb";

	public static string androidAppID = "os.falcon.archer.stick.war";

	public static string iosAppID = "IDFromItunesConnect";

	public static void InitAppsFlyer()
	{
		try
		{
#if ENABLE_APPSFLYER
            AppsFlyer.setAppID(androidAppID);
			AppsFlyer.init(devKey);
			AppsFlyer.loadConversionData("StartUp");
			AppsFlyer.getConversionData();
			AppsFlyer.trackAppLaunch();
#endif
			UnityEngine.Debug.Log("init AppsFlyer complete");
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("AppsFlyer: " + ex.Message + " at " + ex.StackTrace);
		}
	}
}
