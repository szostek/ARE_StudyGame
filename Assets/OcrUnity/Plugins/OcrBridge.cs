using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BrainCheck {

	public class OcrBridge {
	    static AndroidJavaClass _class;
		static AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

		private static void SetupPlugin () {
			if (_class == null) {
				_class = new AndroidJavaClass ("mayankgupta.com.audioPlugin.OcrManager");
				_class.CallStatic ("_initiateFragment");
			}
		}

		public static void liveFeedAndReadCharacters(string textToDisplay)
		{
			SetupPlugin ();
			instance.Call("startTextRecognitionScanning", textToDisplay);	
		}

		public static void stopLiveFeed()
		{
			SetupPlugin ();
			instance.Call("stopScanning");	
		}

		public static void selectImageFromGalleryAndReadCharacters()
		{
			SetupPlugin ();
			instance.Call("captureImageFromGallery");	
		}

		public static void captureImageAndReadCharacters() {
			SetupPlugin ();
			instance.Call("captureImageFromCamera");
		}

		public static IEnumerator takeScreenshotAndReadCharacters() {
			yield return new WaitForEndOfFrame ();
			string fileName = System.DateTime.Now.ToString ("yyyyMMddHHmmssfff") + ".png";
			ScreenCapture.CaptureScreenshot (fileName);
			if (Application.platform == RuntimePlatform.Android) {
				yield return new WaitForSeconds(1);
				string origin = System.IO.Path.Combine (Application.persistentDataPath, fileName);
				SetupPlugin ();
		   		instance.Call("readScreenshot", origin);
			}
		}

		public static bool checkPermissions() {
			SetupPlugin ();
			return instance.Call<bool>("checkPermissions");
		}

		public static void requestPermissions() {
			SetupPlugin ();
			instance.Call("requestPermissions");
		}

		public static void SetUnityGameObjectNameAndMethodName(string gameObject, string methodName){
			SetupPlugin ();
		   	instance.Call("_setUnityGameObjectNameAndMethodName", gameObject, methodName);
		}
	}

}