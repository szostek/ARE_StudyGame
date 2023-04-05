using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScannedTextLoader : MonoBehaviour
{
    public static ScannedTextLoader Instance;
    private TMP_InputField inputField;

    private void Awake() 
    {
        Instance = this;
        inputField = GetComponent<TMP_InputField>();    
    }

    public void ApplyCapturedText(string text)
    {
        inputField.text = text;
    }

    public void ClearText()
    {
        inputField.text = "";
    }

}
