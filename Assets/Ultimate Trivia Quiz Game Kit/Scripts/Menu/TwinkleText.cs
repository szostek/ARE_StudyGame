using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TwinkleText : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        text.DOFade(0.5f, 1f).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
    }
}
