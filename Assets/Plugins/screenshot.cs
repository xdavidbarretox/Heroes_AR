using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class screenshot : MonoBehaviour
{
	public Texture2D TextureButtonScreenShootOut;
	public Texture2D texCaptureScreen = null;
	public Texture2D TextureButtonShare;
	public Texture2D TextureButtonClose;

	public Texture2D Case0;
	public Texture2D Case1;
	public Texture2D Case2;

	private bool isShot = false;
	private string originalText = "";
	private byte[] utf8Bytes;
	private string text = "";

	public GameObject Obj0;
	public GameObject Obj1;
	public GameObject Obj2;
	
	public GUIStyle EstiloGui;
	float nScaleFactor = 1.25f;
	int nCloseSize = (int)(Screen.width * 0.075);
	int nIconSize = (int)(Screen.width / 11.377f);

	#if UNITY_ANDROID
	public static string screenshotFilename = "Screenshot.jpg";

	#endif
	void Start ()
	{
		utf8Bytes = System.Text.Encoding.UTF8.GetBytes(originalText);
		text = System.Text.Encoding.UTF8.GetString (utf8Bytes);
		#if UNITY_ANDROID && !UNITY_EDITOR
		setup_screenshot();
		#endif
	}

	void setup_screenshot()
	{
		ClearScreeshot();
	}
	
	IEnumerator take_screenshot()
	{
		yield return new WaitForEndOfFrame();
		texCaptureScreen = ScreenCapture.Capture();
	}

	private void convertToFile()
	{
		screenshotFilename = "Marvel_" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".jpg";
		//string path = System.IO.Path.Combine(Application.persistentDataPath, screenshotFilename);
		Debug.Log("screenshotFilename " + screenshotFilename);
		//Debug.Log("path " + path);
		byte[] bytes = texCaptureScreen.EncodeToJPG();
		//File.WriteAllBytes(path, bytes);

		string path2 = System.IO.Path.Combine(PathLocation.getExternalDirectory(), screenshotFilename);
		File.WriteAllBytes(path2, bytes);

		AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
		AndroidJavaClass uriClass = new AndroidJavaClass ("android.net.Uri");
		
		AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + path2);
		
		intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_VIEW"));
		intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "image/*");
		
		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		
		currentActivity.Call ("startActivity", intentObject);

/*
		AndroidJavaClass intentClass = new AndroidJavaClass ("org.androidprinting.intent.action.PRINT");
		AndroidJavaClass uriClass = new AndroidJavaClass ("android.net.Uri");
		
		AndroidJavaObject intentObject = new AndroidJavaObject ("org.androidprinting.intent.action.PRINT");
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", path);
		
		//intentObject.Call<AndroidJavaObject> ("setAction", intentClass.GetStatic<string> ("ACTION_VIEW"));
		intentObject.Call<AndroidJavaObject>("addCategory", intentClass.GetStatic<string> ("CATEGORY_DEFAULT"));
		intentObject.Call<AndroidJavaObject>("setDataAndType", uriObject, "image/*");
		
		//AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		//AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");

		AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
		
		currentActivity.Call ("startActivity", intentObject, 0);
*/
	}

	private void ClearScreeshot ()
	{
		Debug.Log ("[ClearScreeshot]...................................................");
		texCaptureScreen = null;
		isShot = false;
	}
	
		void OnGUI ()
		{
			if (texCaptureScreen)
			{
				//Background
				GUI.Box (new Rect (0, 0, Screen.width, Screen.height), "");

				//Shown the screenshot in the screen.
				GUI.DrawTexture (new Rect ((Screen.width / 2) - (texCaptureScreen.width / (nScaleFactor * 2)), (Screen.height / 2) - (texCaptureScreen.height / (nScaleFactor * 2)), Screen.width / nScaleFactor, Screen.height / nScaleFactor), texCaptureScreen);

				//Close button
			if (GUI.Button (new Rect(((float)(Screen.width) / 2) - ((float)(Screen.width) / (nScaleFactor * 2)), 5f,((float)(Screen.width) / nScaleFactor),  ((float)(Screen.width) / nScaleFactor) / 1.33f), TextureButtonClose, EstiloGui))
				{
						ClearScreeshot ();
						isShot = false;
				}
		
				#if UNITY_ANDROID
				if (GUI.Button(new Rect (0, Screen.height - nIconSize, Screen.width, nIconSize), TextureButtonShare, EstiloGui))
				{
					Debug.Log ("Share for android..................................................");
					convertToFile();
				}

				#endif
			}

			//Button to take Screenshot.
			if (!isShot)
			{
				if (GUI.Button (new Rect ((float)(Screen.width) - ((float)(Screen.width) * 0.125f), ((float)Screen.height / 2) - (((float)(Screen.height) * 0.213f) / 2), ((float)(Screen.width) * 0.125f), ((float)(Screen.height) * 0.213f)), TextureButtonScreenShootOut, EstiloGui))
				{
					Debug.Log ("[Button Take screenshot]");
				
					isShot = true;
			
					#if UNITY_ANDROID
					StartCoroutine("take_screenshot");
					#endif
				}

				if (GUI.Button (new Rect(0f,(float)(Screen.height) / 6,(float)(Screen.height) / 6, (float)(Screen.height) / 6), Case0, EstiloGui))
				{
					Obj0.SetActive(true);
					Obj1.SetActive(false);
					Obj2.SetActive(false);
				}

				if (GUI.Button (new Rect(0f,(float)(Screen.height) /2.375f ,(float)(Screen.height) / 6, (float)(Screen.height) / 6), Case1, EstiloGui))
				{
					Obj0.SetActive(false);
					Obj1.SetActive(true);
					Obj2.SetActive(false);
				}

				if (GUI.Button (new Rect(0f,(float)(Screen.height) / 1.5f,(float)(Screen.height) / 6, (float)(Screen.height) / 6), Case2, EstiloGui))
				{
					Obj0.SetActive(false);
					Obj1.SetActive(false);
					Obj2.SetActive(true);
				}
		}
	}
}