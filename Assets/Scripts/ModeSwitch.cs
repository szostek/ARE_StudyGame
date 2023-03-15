using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class ModeSwitch : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image handleImage;
    [SerializeField] AudioClip onFX;
    [SerializeField] GameObject practicePanel;
    [SerializeField] GameObject challengePanel;
    
    private AudioSource audioSource;
    private RectTransform handleRectTransform;
    public Color32 onColor;
    public Color32 offColor;
    private float moveXAmount = 225f;

    public delegate void DidChangedSwitch(bool isOn);
    public DidChangedSwitch didChangedSwitch;
    public bool isOn;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
                this.handleImage.DOBlendableColor(this.offColor, duration);
                this.handleRectTransform.DOAnchorPos(new Vector2(-moveXAmount, 0), duration);

                didChangedSwitch?.Invoke(false);
                practicePanel.SetActive(true);
                challengePanel.SetActive(false);
                gameManager.isChallengeMode = false;
                Debug.Log(gameManager.isChallengeMode);
            }
            else
            {
                this.handleImage.DOBlendableColor(this.onColor, duration);
                this.handleRectTransform.DOAnchorPos(new Vector2(moveXAmount, 0), duration);

                didChangedSwitch?.Invoke(true);

                if (Informations.IsPlaySFX)
                {
                    audioSource.PlayOneShot(onFX);
                }
                practicePanel.SetActive(false);
                challengePanel.SetActive(true);
                gameManager.isChallengeMode = true;
                Debug.Log(gameManager.isChallengeMode);
            }
        }
        else
        {
            if (!isOn)
            {
                this.handleImage.color = this.offColor;
                this.handleRectTransform.anchoredPosition = new Vector2(-moveXAmount, 0);
                practicePanel.SetActive(true);
                challengePanel.SetActive(false);
                gameManager.isChallengeMode = false;
            }
            else
            {
                this.handleImage.color = this.onColor;
                this.handleRectTransform.anchoredPosition = new Vector2(moveXAmount, 0);
                practicePanel.SetActive(false);
                challengePanel.SetActive(true);
                gameManager.isChallengeMode = true;
            }
        }
        this.isOn = isOn;
    }

    public void OnClickSwitch()
    {
        this.SetSwitchOn(!this.isOn, true);
    }
}
