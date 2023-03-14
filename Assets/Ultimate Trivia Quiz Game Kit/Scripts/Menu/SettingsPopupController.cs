using UnityEngine;

public class SettingsPopupController : PopupController
{
    [SerializeField] Switch fxSwitch;
    [SerializeField] Switch bgmSwitch;

    override protected void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        fxSwitch.didChangedSwitch = delegate (bool isOn)
        {
            if (Informations.IsPlaySFX != isOn)
            {
                Informations.IsPlaySFX = isOn;
            }
        };

        bgmSwitch.didChangedSwitch = delegate (bool isOn)
        {
            if (Informations.IsPlayBGM != isOn)
            {
                Informations.IsPlayBGM = isOn;

                CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                canvasManager.CheckBGM();
            }
        };
    }

    public override PopupController Show()
    {
        PopupController popupController = base.Show();

        if (Informations.IsPlaySFX)
        {
            fxSwitch.SetSwitchOn(true, false);
        }
        else
        {
            fxSwitch.SetSwitchOn(false, false);
        }

        if (Informations.IsPlayBGM)
        {
            bgmSwitch.SetSwitchOn(true, false);
        }
        else
        {
            bgmSwitch.SetSwitchOn(false, false);
        }

        return popupController;
    }

    public void OnClickRateGame()
    {
#if UNITY_ANDROID
        Application.OpenURL("");
#elif UNITY_IPHONE
        Application.OpenURL("");
#endif
    }

    public void OnClickVisitCafe()
    {
        Application.OpenURL("");
    }

    public void OnClickVisitInstagram()
    {
        Application.OpenURL("");
    }

    public void OnClickVisitFacebook()
    {
        Application.OpenURL("");
    }

    public void OnClickVisitHomepage()
    {
        Application.OpenURL("");
    }
}
