using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]

public class PopupController : MonoBehaviour
{
    [SerializeField] AudioClip openFX;
    [SerializeField] AudioClip closeFX;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    protected AudioSource audioSource;

    protected virtual void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        this.canvasGroup = GetComponent<CanvasGroup>();

        audioSource = GetComponent<AudioSource>();

        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        this.Hide(false);
    }

    public virtual PopupController Show()
    {
        float duration = 0.4f;
        this.gameObject.SetActive(true);
        this.canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuint);
        this.rectTransform.DOAnchorMin(new Vector2(0f, 0f), duration).SetEase(Ease.InOutQuint);
        this.rectTransform.DOAnchorMax(new Vector2(1f, 0.95f), duration).SetEase(Ease.InOutQuint);

        if (Informations.IsPlaySFX)
            audioSource.PlayOneShot(openFX);

        return this;
    }

    public virtual void Hide(bool animated = true)
    {
        float duration = 0.4f;

        if (Informations.IsPlaySFX)
            audioSource.PlayOneShot(closeFX);

        if (animated == false)
        {
            this.rectTransform.anchorMin = new Vector2(0f, -0.98f);
            this.rectTransform.anchorMax = new Vector2(1f, 0f);
            this.canvasGroup.alpha = 1f;
            this.gameObject.SetActive(false);
        }
        else
        {
            this.rectTransform.DOAnchorMin(new Vector2(0f, -0.98f), duration).SetEase(Ease.InOutQuint);
            this.rectTransform.DOAnchorMax(new Vector2(1f, 0f), duration).SetEase(Ease.InOutQuint);
            this.canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuint).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        }
    }
}
