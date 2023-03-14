using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]
public class LevelBox : MonoBehaviour
{
    [SerializeField] Text levelText;
    [SerializeField] Text levelSubText;

    [SerializeField] AudioClip showSFX;
    [SerializeField] AudioClip hideSFX;

    public delegate void DidFinishedShow();

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    AudioSource audioSource;

    private void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.canvasGroup.alpha = 0f;
        this.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    public void ShowAndHide(int stageIndex,  float duration, DidFinishedShow didFinishedShow = null)
    {
        this.gameObject.SetActive(true);

        this.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        this.rectTransform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(Ease.OutBack);

        this.levelText.text = (++stageIndex).ToString();

        if (Informations.IsPlaySFX) audioSource.PlayOneShot(showSFX);

        this.canvasGroup.DOFade(1f, duration).SetEase(Ease.OutBack).OnComplete(() =>
        {
            DOVirtual.DelayedCall(2f, () =>
            {
                if (Informations.IsPlaySFX) audioSource.PlayOneShot(hideSFX);

                this.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.4f).SetEase(Ease.InBack);

                this.canvasGroup.DOFade(0f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    didFinishedShow?.Invoke();
                });
            });
        });
    }
}