using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopupController : PopupController
{
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Button noadsHeart60Button;
    [SerializeField] Button rewardHeartButton;
    [SerializeField] Button noadsButton;
    [SerializeField] Button heart20Button;
    [SerializeField] Button heart60Button;
    [SerializeField] Button heart150Button;
    [SerializeField] Button heart320Button;
    [SerializeField] Button heart450Button;

    [SerializeField] Text noadsHeart60PriceText;
    [SerializeField] Text noadsPriceText;
    [SerializeField] Text heart20PriceText;
    [SerializeField] Text heart60PriceText;
    [SerializeField] Text heart150PriceText;
    [SerializeField] Text heart320PriceText;
    [SerializeField] Text heart450PriceText;
    [SerializeField] Button[] buttons;
    [SerializeField] AudioClip buttonTouchSFX;
    [SerializeField] GameObject confirmPopupPrefab;
    [SerializeField] GameObject restoreButton;

    override protected void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        this.RefreshUI();
    }

    void Start()
    {
        this.scrollRect.verticalNormalizedPosition = 1f;

        GameObject canvasManager = GameObject.Find("/Canvas Manager");

#if UNITY_IPHONE
        restoreButton.SetActive(true);
        noadsHeart60PriceText.text = "$2,99";
        noadsPriceText.text = "";
        heart20PriceText.text = "$0.99";
        heart60PriceText.text = "$1.99";
        heart150PriceText.text = "$3.99";
        heart320PriceText.text = "$7.99";
        heart450PriceText.text = "$9.99";
#elif UNITY_ANDROID
        restoreButton.SetActive(false);
        noadsHeart60PriceText.text = "$2.99";
        noadsPriceText.text = "$2.99";
        heart20PriceText.text = "$0.99";
        heart60PriceText.text = "$1.99";
        heart150PriceText.text = "$3.99";
        heart320PriceText.text = "$7.99";
        heart450PriceText.text = "$9.99";
#endif

    }

    public void SetInteractableButtons(bool isInteractable)
    {
        foreach (Button button in this.buttons)
        {
            button.interactable = isInteractable;
        }
    }

    private void RefreshUI()
    {
        if (Informations.IsNoAds == true)
        {
            noadsHeart60Button.gameObject.SetActive(false);
            noadsButton.gameObject.SetActive(false);
        }
        else
        {
            noadsHeart60Button.gameObject.SetActive(true);
            noadsButton.gameObject.SetActive(true);
        }
    }

    public void OnClickShopItem(int index)
    {
        if (Informations.IsPlaySFX) audioSource.PlayOneShot(buttonTouchSFX);

        switch (index)
        {
            case 0:
                {
                    break;
                }
            case 1:
                {
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4:
                {
                    break;
                }
            case 5:
                {
                    break;
                }
            case 6:
                {
                    break;
                }
            case 7:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void RestorePurchase()
    {
        if (Informations.IsPlaySFX) audioSource.PlayOneShot(buttonTouchSFX);
    }
}
