using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]

public class Heart : MonoBehaviour
{
    [SerializeField] GameObject heartEffectImageObject;
    [SerializeField] GameObject heartImagePrefab;
    [SerializeField] RectTransform heartArea;

    [SerializeField] Text heartText;

    [SerializeField] AudioClip subtractFX;
    [SerializeField] AudioClip notEnoughFX;

    private AudioSource audioSource;

    private CanvasGroup canvasGroup;
    private RectTransform rectTranform;
    private Vector2 currentAnchoredPosition;
    private Vector2 currentAnchoredSize;

    public delegate void AfterSubtractHeart();
    public delegate void AfterAddHeart();

    private void Awake()
    {
        this.heartEffectImageObject.SetActive(false);

        this.rectTranform = GetComponent<RectTransform>();
        this.currentAnchoredPosition = this.rectTranform.anchoredPosition;
        this.currentAnchoredSize = this.rectTranform.sizeDelta;

        this.canvasGroup = GetComponent<CanvasGroup>();

        audioSource = GetComponent<AudioSource>();

        this.Hide(0f);
    }

    public void SetHeartAmount(int heartAmount)
    {
        this.heartText.text = heartAmount.ToString();

        int textLength = this.heartText.text.Length;
        float addSize = 35f * textLength;
        this.rectTranform.sizeDelta = new Vector2(100f + addSize, 64f);
        this.heartText.rectTransform.sizeDelta = new Vector2(addSize, 64f);
    }

    public void Show(float duration)
    {
        this.gameObject.SetActive(true);
        this.SetHeartAmount(Informations.HeartAmount);

        if (duration <= 0)
        {
            this.rectTranform.anchoredPosition = new Vector2(currentAnchoredPosition.x, currentAnchoredPosition.y);
            this.canvasGroup.alpha = 1f;
        }
        else
        {
            this.rectTranform.DOAnchorPos(new Vector2(currentAnchoredPosition.x, currentAnchoredPosition.y), duration).SetEase(Ease.OutBounce);
            this.canvasGroup.DOFade(1f, duration);
        }
    }

    public void Hide(float duration)
    {
        
        if (duration <= 0)
        {
            this.canvasGroup.alpha = 0f;
            this.rectTranform.anchoredPosition = new Vector2(currentAnchoredPosition.x, currentAnchoredPosition.y + currentAnchoredSize.y);
            this.gameObject.SetActive(false);
        }
        else
        {
            this.canvasGroup.DOFade(0f, duration).OnComplete(() =>
            {
                this.rectTranform.anchoredPosition = new Vector2(currentAnchoredPosition.x, currentAnchoredPosition.y + currentAnchoredSize.y);
                this.gameObject.SetActive(false);
            });
        }
    }

    public void SubtractHeart(AfterSubtractHeart callback)
    {
        if (Informations.IsPlaySFX) audioSource.PlayOneShot(subtractFX);
        float heartImageAnimationDuration = 1f;
        this.heartEffectImageObject.SetActive(true);
        this.heartEffectImageObject.GetComponent<RectTransform>().DOScale(3f, heartImageAnimationDuration);
        this.heartEffectImageObject.GetComponent<Image>().DOFade(0f, heartImageAnimationDuration).OnComplete(() =>
        {
            this.heartEffectImageObject.GetComponent<RectTransform>().DOScale(1f, 0f);
            this.heartEffectImageObject.GetComponent<Image>().DOFade(1f, 0f);
            this.heartEffectImageObject.SetActive(false);
        });

        float heartTextAnimationDuration = 0.2f;
        float yPos = 40f;

        DOVirtual.DelayedCall(heartImageAnimationDuration * 0.5f, () =>
        {
            this.heartText.GetComponent<RectTransform>().DOAnchorPosY(-yPos, heartTextAnimationDuration);
            this.heartText.DOFade(0f, heartTextAnimationDuration).OnComplete(() =>
            {
                Informations.HeartAmount--;
                this.heartText.text = Informations.HeartAmount.ToString();

                int textLength = this.heartText.text.Length;
                float addSize = 35f * textLength;
                this.rectTranform.sizeDelta = new Vector2(100f + addSize, 64f);
                this.heartText.rectTransform.sizeDelta = new Vector2(addSize, 64f);

                this.heartText.GetComponent<RectTransform>().DOAnchorPosY(yPos, 0f);
                this.heartText.GetComponent<RectTransform>().DOAnchorPosY(0f, heartTextAnimationDuration);
                this.heartText.DOFade(1f, heartTextAnimationDuration).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        callback?.Invoke();
                    });
                });
            });
        });
    }

    public void NotEnough(AfterSubtractHeart callback)
    {
        audioSource.PlayOneShot(notEnoughFX);
        this.rectTranform.DOPunchPosition(new Vector3(20f, 0f, 0f), 1f, 7).OnComplete(()=>
        {
            callback?.Invoke();
        });
    }

    private void ChangeTextAnimation(bool isAdd, Action completion = null)
    {
        float duration = 0.2f;
        float yPos = 40f;
        this.heartText.rectTransform.DOAnchorPosY(-yPos, duration);
        this.heartText.DOFade(0f, duration).OnComplete(() =>
        {
            if (isAdd)
            {
                Informations.HeartAmount++;
                this.heartText.text = Informations.HeartAmount.ToString();
            }
            else
            {
                Informations.HeartAmount--;
                this.heartText.text = Informations.HeartAmount.ToString();
            }

            this.AutofitText();
            
            this.heartText.rectTransform.DOAnchorPosY(yPos, 0f);
            this.heartText.rectTransform.DOAnchorPosY(0f, duration);
            this.heartText.DOFade(1f, duration).OnComplete(() =>
            {
                completion?.Invoke();
            });
        });
    }

    private void AutofitText()
    {
        int textLength = this.heartText.text.Length;
        float addSize = 35f * textLength;
        this.rectTranform.sizeDelta = new Vector2(100f + addSize, 64f);
        this.heartText.rectTransform.sizeDelta = new Vector2(addSize, 64f);
    }

    private void AddHeartAnimation(bool isAdd, Action completion = null) {
        float duration = 0.4f;
        float fromScale, toScale;
        float fromAlpha, toAlpha;

        Image heartImage = Instantiate(this.heartImagePrefab, this.heartArea).GetComponent<Image>();

        if (isAdd) {
            fromScale = 3f; toScale = 1f;
            fromAlpha = 0f; toAlpha = 1f;
        } else {
            fromScale = 1f; toScale = 3f;
            fromAlpha = 1f; toAlpha = 0f;
        }        

        heartImage.rectTransform.DOScale(fromScale, 0f);
        heartImage.DOFade(fromAlpha, 0f);

        heartImage.rectTransform.DOScale(toScale, duration);
        heartImage.DOFade(toAlpha, duration).OnComplete(() => {
            Destroy(heartImage.gameObject);
            completion?.Invoke();
        });
    }

    public void AddHeart(int heartAmount, Action callback = null)
    {
        int currentHeartAmount = Informations.HeartAmount;
        Informations.HeartAmount += heartAmount - 3;

        Sequence heartSequence = DOTween.Sequence();

        for (int i = 0; i < 3; i++)
        {
            heartSequence.AppendCallback(() => {
                this.ChangeTextAnimation(true);
                if (Informations.IsPlaySFX) audioSource.PlayOneShot(subtractFX);
            });
            heartSequence.AppendInterval(0.5f);
        }
        heartSequence.AppendCallback(() => callback?.Invoke());
    }
}
