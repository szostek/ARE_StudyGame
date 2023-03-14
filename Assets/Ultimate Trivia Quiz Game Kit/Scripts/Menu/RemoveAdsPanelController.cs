using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RemoveAdsPanelController : MonoBehaviour
{
    [SerializeField] Text text;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.Hide(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        this.canvasGroup.alpha = 1f;
    }

    public void ShowText(Action callback)
    {
        this.text.DOText("Removed ads.", 2f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                this.Hide(true);
                this.text.text = "";
                callback.Invoke();
            });
        });
    }

    public void Hide(bool animated = false)
    {
        if (animated)
        {
            float duration = 2f;
            this.canvasGroup.DOFade(0f, duration).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
        else
        {
            this.canvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);
        }
    }
}
