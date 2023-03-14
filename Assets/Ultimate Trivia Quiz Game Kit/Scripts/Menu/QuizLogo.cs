using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class QuizLogo : MonoBehaviour
{
    [SerializeField] private GameObject topLogo;
    [SerializeField] private GameObject middleLogo;
    [SerializeField] private GameObject bottomLogo;

    private RectTransform topLogoRectTransform;
    private RectTransform middleLogoRectTransform;
    private RectTransform bottomLogoRectTransform;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        this.topLogoRectTransform = topLogo.GetComponent<RectTransform>();
        this.middleLogoRectTransform = middleLogo.GetComponent<RectTransform>();
        this.bottomLogoRectTransform = bottomLogo.GetComponent<RectTransform>();

        this.canvasGroup = GetComponent<CanvasGroup>();

        this.Hide(0f);
    }
    
    public void Show(float duration)
    {
        this.gameObject.SetActive(true);
        this.canvasGroup.DOFade(1f, duration * 0.7f).OnComplete(() =>
        {
            this.topLogoRectTransform.DOAnchorPos(new Vector2(0f, 15f), duration * 0.3f);
            this.bottomLogoRectTransform.DOAnchorPos(new Vector2(0f, -15f), duration * 0.3f);
        });
    }

    public void Hide(float duration)
    {
        if (duration <= 0)
        {
            this.topLogoRectTransform.anchoredPosition = new Vector2(0f, 0f);
            this.bottomLogoRectTransform.anchoredPosition = new Vector2(0f, 0f);
            this.canvasGroup.alpha = 0f;
            this.gameObject.SetActive(false);
        }
        else
        {
            this.topLogoRectTransform.DOAnchorPos(new Vector2(0f, 0f), duration * 0.3f);
            this.bottomLogoRectTransform.DOAnchorPos(new Vector2(0f, 0f), duration * 0.3f).OnComplete(() =>
            {
                this.canvasGroup.DOFade(0f, duration * 0.7f).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                });
            });
        }
    }
}
