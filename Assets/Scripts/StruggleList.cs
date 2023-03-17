using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleList : MonoBehaviour
{
    [SerializeField] StruggleButton struggleButtonPrefab;
    [SerializeField] RectTransform content;

    private void Start() 
    {

        for (int i = 0; i < 12; i++) {
            StruggleButton button = Instantiate(struggleButtonPrefab, content);
            button.struggleList = this;
            button.qIndex = i;
            button.buttonText.text = i.ToString();
        }
    }

    public void GetStruggleQuestion(int qIndex)
    {
        Debug.Log(qIndex);
    }
}
