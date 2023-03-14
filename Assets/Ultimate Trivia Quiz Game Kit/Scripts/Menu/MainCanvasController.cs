using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public delegate void DidFinishedStopCanvas();

public class MainCanvasController : MonoBehaviour
{
    [SerializeField] Text levelText;

    [SerializeField] LevelsPopupController levelsPopupController;
    [SerializeField] SettingsPopupController settingsPopupController;
    [SerializeField] ShopPopupController shopPopupController;

    [SerializeField] QuizLogo logo;

    [SerializeField] Heart heart;

    [SerializeField] GameObject playButtonObject;
    [SerializeField] GameObject allClearTextObject;

    [SerializeField] CircleEffect playButtonCircleEffect;
    [SerializeField] CircleEffect shopButtonCircleEffect;

    [SerializeField] Element[] mainButtons;

    [SerializeField] Element subTitle;

    [SerializeField] Element dimPanel;
    [SerializeField] Element darkPanel;

    [SerializeField] Element waitPopupPanel;

    [SerializeField] Element removeAdsButton;

    private PopupController currentPopupController;

    enum State { IDLE, BUSY, NONE }
    private State state = State.NONE;

    private bool isInitialized = false;

    private delegate void DidFinishedInitialize();

    [SerializeField] GameObject confirmPopupPrefab;

    private void Start()
    {
        GameObject canvasManager = GameObject.Find("/Canvas Manager");
    }

    private void Update()
    {
        switch (this.state)
        {
            case State.BUSY:
                foreach (Element button in mainButtons)
                {
                    button.GetComponent<Button>().interactable = false;
                }
                break;
            case State.IDLE:
                foreach (Element button in mainButtons)
                {
                    button.GetComponent<Button>().interactable = true;
                }
                break;
            case State.NONE:
                foreach (Element button in mainButtons)
                {
                    button.GetComponent<Button>().interactable = false;
                }
                break;
        }
    }

    private void CheckHeartAmount()
    {
        int level = Informations.StageIndex + 1;
        levelText.text = "LEVEL " + level;

        this.heart.SetHeartAmount(Informations.HeartAmount);

        if (Informations.StageIndex < CSVReader.Instance.TotalStage)
        {
            if (Informations.HeartAmount > 0)
            {
                this.playButtonCircleEffect.Play();
                this.shopButtonCircleEffect.Stop();
            }
            else
            {
                this.shopButtonCircleEffect.Play();
                this.playButtonCircleEffect.Stop();
            }
        }
        else
        {
            this.playButtonCircleEffect.Stop();
            this.shopButtonCircleEffect.Stop();
        }
    }

    private void Initialize(DidFinishedInitialize callback)
    {
        this.logo.Show(2f);
        DOVirtual.DelayedCall(2.5f, () =>
        {
            float duration = 0.5f;
            this.subTitle.Show(duration);
            this.heart.Show(duration);
            levelText.GetComponent<Element>().Show(duration);

            foreach (Element button in mainButtons)
            {
                button.Show(duration);
            }

            CheckPlayButtonState();

            removeAdsButton.Show(duration);

            DOVirtual.DelayedCall(duration, () =>
            {
                callback?.Invoke();
                this.isInitialized = true;
            });
        });
    }

    void CheckPlayButtonState()
    {
        if (CSVReader.Instance.TotalStage <= Informations.StageIndex)
        {
            playButtonObject.SetActive(false);
            allClearTextObject.SetActive(true);
        }
        else
        {
            playButtonObject.SetActive(true);
            allClearTextObject.SetActive(false);
        }
    }

    #region Button Events
    public void OnClickPlayButton()
    {
        this.state = State.BUSY;
        this.playButtonCircleEffect.Stop();

        if (Informations.HeartAmount > 0)
        {
            this.heart.SubtractHeart(() =>
            {
                CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                canvasManager.ShowGame(Informations.StageIndex);
            });
        }
        else
        {
            this.heart.NotEnough(() =>
            {
                this.state = State.IDLE;
            });
        }
    }

    public void OnClickLevelsButton()
    {
        if (!this.currentPopupController)
        {
            this.currentPopupController = levelsPopupController.Show();
            this.darkPanel.Show(0.4f, Ease.InOutBack);
        }
    }

    public void OnClickSettingsButton()
    {
        if (!this.currentPopupController)
        {
            this.currentPopupController = settingsPopupController.Show();
            this.darkPanel.Show(0.4f, Ease.InOutBack);
        }
    }

    public void OnClickShopButton()
    {
        if (!this.currentPopupController)
        {
            this.currentPopupController = shopPopupController.Show();
            this.darkPanel.Show(0.4f, Ease.InOutBack);
        }
    }

    public void ShowDescriptions(int index)
    {
        this.HideModalPanel();

        CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
        canvasManager.ShowGame(index);
    }

    public void HideModalPanel()
    {
        if (this.currentPopupController)
        {
            this.CheckHeartAmount();
            this.currentPopupController.Hide();
            this.currentPopupController = null;
            this.darkPanel.Hide(0.4f);
        }
    }
    #endregion

    #region interface
    public void StartMainCanvas()
    {
        this.CheckHeartAmount();
        if (isInitialized)
        {
            this.dimPanel.Hide(1f, () =>
            {
                this.state = State.IDLE;
                CheckPlayButtonState();
            });
        }
        else
        {
            this.dimPanel.Hide(0f);
            this.Initialize(() =>
            {
                this.state = State.IDLE;
            });
        }
    }

    public void StopCanvas(bool immediately, DidFinishedStopCanvas callback)
    {
        if (isInitialized)
        {
            if (immediately)
            {
                this.dimPanel.Show(0f, Ease.Linear, () => {
                    DOVirtual.DelayedCall(0.4f, () => {
                        callback?.Invoke();
                    });
                });
            }
            else
            {
                this.dimPanel.Show(0.4f, Ease.Linear, () => callback?.Invoke()); 
            }
        }
        else
        {
            callback?.Invoke();
        }
    }
    #endregion

    public void ShowWaitPopup()
    {
        waitPopupPanel.Show(0.5f);
    }

    public void HideWaitPopup()
    {
        waitPopupPanel.Hide(0.5f);
    }
}