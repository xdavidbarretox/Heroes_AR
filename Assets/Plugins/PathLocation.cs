using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class PathLocation
{
#if UNITY_ANDROID
	public static string getExternalDirectory()
	{
		//AndroidJavaClass InternalPath = new AndroidJavaClass("com.inmersys.pathlocation.Bridge");
		//return InternalPath.CallStatic<String>("getExternalDirectory");

		using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				using (AndroidJavaClass storage = new AndroidJavaClass("com.inmersys.pathlocation.Bridge"))
				{
					return storage.CallStatic<string>("getExternalDirectory", activity);
				}
			}
		}
	}

	public static string getInternalDirectory()
	{
		using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				using (AndroidJavaClass storage = new AndroidJavaClass("com.inmersys.pathlocation.Bridge"))
				{
					return storage.CallStatic<string>("getInternalDirectory", activity);
				}
			}
		}
	}

#endif
}