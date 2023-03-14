using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]
public class StageClear : MonoBehaviour
{
    [SerializeField] RectTransform stageObject;
    [SerializeField] RectTransform clearObject;

    [SerializeField] AudioClip clearSFX;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    public delegate void DidFinishShow();

    private void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();

        Hide(0f, null);
    }

    public void Show(float duration, DidFinishShow callback)
    {
        this.stageObject.DOAnchorPos(new Vector2(0f, 0f), duration * 0.15f).SetEase(Ease.OutBack);
        this.clearObject.DOAnchorPos(new Vector2(0f, 0f), duration * 0.15f).SetEase(Ease.OutBack);
        this.canvasGroup.DOFade(1f, duration * 0.15f).SetEase(Ease.OutBack);
        audioSource.PlayOneShot(clearSFX);
        DOVirtual.DelayedCall(duration * 0.7f, () =>
        {
            this.Hide(duration * 0.15f, callback);
        });
    }

    private void Hide(float duration, DidFinishShow callback)
    {
        if (duration == 0f)
        {
            this.stageObject.anchoredPosition = new Vector2(-400f, 0f);
            this.clearObject.anchoredPosition = new Vector2(400f, 0f);
            this.canvasGroup.alpha = 0f;
        }
        else
        {
            this.stageObject.DOAnchorPos(new Vector2(-400f, 0f), duration).SetEase(Ease.InBack);
            this.clearObject.DOAnchorPos(new Vector2(400f, 0f), duration).SetEase(Ease.InBack).OnComplete(()=>callback?.Invoke());
            this.canvasGroup.DOFade(0f, duration).SetEase(Ease.InBack);
        }
    }
}
