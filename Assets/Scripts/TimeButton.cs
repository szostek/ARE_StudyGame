using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeButton : MonoBehaviour
{
    public TMP_Text intervalNameText;
    public TMP_Text intervalDetailsText;

    [HideInInspector] public int buttonIndex;
    [HideInInspector] public TimeButtonList timeButtonList;

    public void Select()
    {
        timeButtonList.SetIntervalIndex(buttonIndex);
    }
}
