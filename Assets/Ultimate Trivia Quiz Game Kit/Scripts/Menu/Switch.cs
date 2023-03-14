using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Switch : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] Image handleImage;
    [SerializeField] AudioClip onFX;
    
    private AudioSource audioSource;
    private RectTransform handleRectTransform;
    private Color32 onColor = new Color32(242, 68, 149, 255);
    private Color32 offColor = new Color32(44, 55, 89, 255);

    public delegate void DidChangedSwitch(bool isOn);
    public DidChangedSwitch didChangedSwitch;
    public bool isOn;

    private void Awake()
    {
        this.handleRectTransform = handleImage.GetComponent<RectTransform>();
        this.SetSwitchOn(false, false);
        audioSource = GetComponent<AudioSource>();
    }

    public void SetSwitchOn(bool isOn, bool animated)
    {
        float duration = 0.2f;

        if (animated)
        {
            if (!isOn)
            {
                this.backgroundImage.DOBlendableColor(this.offColor, duration);
                this.handleRectTransform.DOAnchorPos(new Vector2(-38f, 0), duration);

                didChangedSwitch?.Invoke(false);
            }
            else
            {
                this.backgroundImage.DOBlendableColor(this.onColor, duration);
                this.handleRectTransform.DOAnchorPos(new Vector2(38f, 0), duration);

                didChangedSwitch?.Invoke(true);

                if (Informations.IsPlaySFX)
                {
                    audioSource.PlayOneShot(onFX);
                }
            }
        }
        else
        {
            if (!isOn)
            {
                this.backgroundImage.color = this.offColor;
                this.handleRectTransform.anchoredPosition = new Vector2(-38f, 0);
            }
            else
            {
                this.backgroundImage.color = this.onColor;
                this.handleRectTransform.anchoredPosition = new Vector2(38f, 0);
            }
        }
        this.isOn = isOn;
    }

    public void OnClickSwitch()
    {
        this.SetSwitchOn(!this.isOn, true);
    }
}
