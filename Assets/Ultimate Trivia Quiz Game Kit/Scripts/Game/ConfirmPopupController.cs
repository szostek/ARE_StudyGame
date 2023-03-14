using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]
public class ConfirmPopupController : MonoBehaviour
{
    [SerializeField] Text messageText;
    [SerializeField] Button cancelButton;
    [SerializeField] AudioClip popupSFX;
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] AudioClip addHeartSFX;

    public enum ConfirmType { NORMAL, BUY };

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    Action confirmCallback;
    Action cancelCallback;

    private void Awake() {
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.Hide(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Show(string message, ConfirmType type, bool animated, Action confirmCallback, Action cancelCallback = null)
    {
        this.messageText.text = message;

        this.confirmCallback = confirmCallback;

        if (cancelCallback != null)
        {
            cancelButton.gameObject.SetActive(true);
            this.cancelCallback = cancelCallback;
        }
        else
        {
            cancelButton.gameObject.SetActive(false);
        }

        this.gameObject.SetActive(true);

        float duration;

        if (animated)
        {
            duration = 0.2f;
        }
        else
        {
            duration = 0f;
        }

        this.canvasGroup.DOFade(1f, duration);

        if (type == ConfirmType.NORMAL)
        {
            if (Informations.IsPlaySFX) audioSource.PlayOneShot(popupSFX);
        }
        else if (type == ConfirmType.BUY)
        {
            if (Informations.IsPlaySFX) audioSource.PlayOneShot(addHeartSFX);
        }
    }

    public void Hide(bool animated = false, Action callback = null)
    {
        if (animated)
        {
            float duration = 0.3f;
            this.canvasGroup.DOFade(0f, duration).OnComplete(() => {
                callback?.Invoke();
            });
        }
        else
        {
            this.canvasGroup.alpha = 0f;
            callback?.Invoke();
        }
    }

    public void OnClickConfirm()
    {
        if (Informations.IsPlaySFX) audioSource.PlayOneShot(buttonSFX);
        this.Hide(true, () => {
            this.confirmCallback();
            Destroy(gameObject);
        });
    }

    public void OnClickCancel()
    {
        if (Informations.IsPlaySFX) audioSource.PlayOneShot(buttonSFX);
        this.Hide(true, () => {
            this.cancelCallback();
            Destroy(gameObject);
        });
    }
}
