using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BrainCheck;

public class TextScanner : MonoBehaviour
{
    public void ScanTextButton()
    {
#if UNITY_ANDROID
        OcrBridge.SetUnityGameObjectNameAndMethodName("ScannedText_InputField", "ApplyCapturedText");
        ScannedTextLoader.Instance.ClearText();
        bool permission = BrainCheck.OcrBridge.checkPermissions();
        if (!permission) {
            OcrBridge.requestPermissions();
        }
        OcrBridge.captureImageAndReadCharacters();
#endif
    }


}
