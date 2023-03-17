using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StruggleButton : MonoBehaviour
{
    public int qIndex;
    public StruggleList struggleList;
    public TMP_Text buttonText;

    public void Select()
    {
        struggleList.GetStruggleQuestion(qIndex);
    }
}
