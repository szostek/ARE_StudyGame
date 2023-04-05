using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BrainCheck {

	public enum OcrOptions 
	{
	  checkCameraPermission,
	  requestCameraPermission,
	  takeScreenshotAndReadCharacters,
	  captureImageAndReadCharacters,
	  selectImageFromGalleryAndReadCharacters,
	  liveFeedAndReadCharacters,
	  stopLiveFeed
	}

	public class DemoScript : MonoBehaviour
	{
		public OcrOptions myOption;
		string gameObjectName = "UnityReceiveMessage";
		string methodName = "statusMessage";
		string textToDisplay = "Capture Image From Camera";

		void OnMouseUp() {
	    	StartCoroutine(BtnAnimation());
	 	}

	 	private IEnumerator BtnAnimation()
	    {
	    	Vector3 originalScale = gameObject.transform.localScale;
	        gameObject.transform.localScale = 0.9f * gameObject.transform.localScale;
	        yield return new WaitForSeconds(0.2f);
	        gameObject.transform.localScale = originalScale;
	        ButtonAction();
	    }

	    private void ButtonAction() {
	    	BrainCheck.OcrBridge.SetUnityGameObjectNameAndMethodName(gameObjectName, methodName);
	    	UnityReceiveMessages.Instance.clearStatus();
			switch(myOption) 
			{
				case OcrOptions.checkCameraPermission:
				  bool permission = BrainCheck.OcrBridge.checkPermissions();
				  if (permission) {
				  	UnityReceiveMessages.Instance.statusMessage("App has Permission");
				  } else {
				  	UnityReceiveMessages.Instance.statusMessage("App does not have Permission");
				  }
			      break;
				case OcrOptions.requestCameraPermission:
				  BrainCheck.OcrBridge.requestPermissions();
			      break;
			    case OcrOptions.takeScreenshotAndReadCharacters:
			      StartCoroutine(BrainCheck.OcrBridge.takeScreenshotAndReadCharacters());
			      break;
			    case OcrOptions.captureImageAndReadCharacters:
				  BrainCheck.OcrBridge.captureImageAndReadCharacters();
			      break;
			    case OcrOptions.selectImageFromGalleryAndReadCharacters:
				  BrainCheck.OcrBridge.selectImageFromGalleryAndReadCharacters();
			      break;
			    case OcrOptions.liveFeedAndReadCharacters:
				  BrainCheck.OcrBridge.liveFeedAndReadCharacters(textToDisplay);
			      break;
			    case OcrOptions.stopLiveFeed:
				  BrainCheck.OcrBridge.stopLiveFeed();
			      break;
			}
	    }
	}
}
