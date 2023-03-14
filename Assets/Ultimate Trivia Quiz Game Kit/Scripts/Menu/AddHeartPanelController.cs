using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class AddHeartPanelController : MonoBehaviour
{
    [SerializeField] Heart heart;

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

    public void AddHeart(int heartAmount, Heart.AfterAddHeart callback)
    {
        this.heart.Show(0f);

        DOVirtual.DelayedCall(1f, () =>
        {
            heart.AddHeart(heartAmount, () =>
            {
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    this.Hide(true);
                    callback.Invoke();
                });
            });
        });
    }

    public void Hide(bool animated = false)
    {
        if (animated)
        {
            float duration = 2f;
            this.canvasGroup.DOFade(0f, duration).OnComplete(()=>
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
