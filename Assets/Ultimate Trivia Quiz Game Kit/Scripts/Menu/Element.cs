using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class Element : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected RectTransform rectTransform;

    public delegate void DidFinishedAction();

    virtual protected void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.rectTransform = GetComponent<RectTransform>();

        Hide(0);
    }

    virtual public void Show(float duration, Ease ease = Ease.Linear, DidFinishedAction callback = null)
    {
        if (!gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);

            if (duration <= 0)
            {
                this.canvasGroup.alpha = 1f;
                callback?.Invoke();
            }
            else
            {
                this.canvasGroup.DOFade(1f, duration).SetEase(ease).OnComplete(() =>
                {
                    callback?.Invoke();
                });
            }
        }
    }

    virtual public void Hide(float duration, DidFinishedAction callback = null)
    {
        if (this.gameObject.activeSelf)
        {
            if (duration <= 0)
            {
                this.canvasGroup.alpha = 0;
                this.gameObject.SetActive(false);
                callback?.Invoke();
            }
            else
            {
                this.canvasGroup.DOFade(0f, duration).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    callback?.Invoke();
                });
            }
        }
    }
}
