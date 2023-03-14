using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] RectTransform gamePanel;
    [SerializeField] GameObject levelBoxPrefab;
    [SerializeField] GameObject quizCardPrefab;
    [SerializeField] GameObject descriptionCardPrefab;
    [SerializeField] GameObject stageClearPrefab;

    [SerializeField] AudioClip finishGameSFX;

    [SerializeField] GameObject confirmPopupPrefab;

    private AudioSource audioSource;

    private Image gamePanelImage;

    private LevelBox levelBox;

    private CardController firstQuizCard = null;
    private CardController secondQuizCard = null;
    private CardController tempQuizCard = null;

    private int currentQuizSequence;

    private List<Quiz> quizList;

    private void Awake()
    {
        gamePanelImage = this.gamePanel.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGameCanvas(int levelIndex)
    {
        currentQuizSequence = 0;
        quizList = CSVReader.Instance.Read(levelIndex);

        if (quizList.Count > 0)
        {
            levelBox = Instantiate(levelBoxPrefab, gamePanel).GetComponent<LevelBox>();
            levelBox.ShowAndHide(levelIndex, 0.3f, () =>
            {
                Destroy(levelBox.gameObject);
                InitializeQuizCard(levelIndex);
            });
        }
    }

    public void StopCanvas(DidFinishedStopCanvas callback)
    {
        callback?.Invoke();
    }

    private void InitializeQuizCard(int levelIndex)
    {
        GameObject cardPrefab = levelIndex != Informations.StageIndex ? descriptionCardPrefab : quizCardPrefab;

        if (this.quizList.Count > 0)
        {
            this.firstQuizCard = Instantiate(cardPrefab, gamePanel).GetComponent<CardController>();
            this.firstQuizCard.SetQuiz(this.quizList[currentQuizSequence], currentQuizSequence, this.quizList.Count);

            this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.FIRST, () =>
            {
                if (this.quizList.Count > 1)
                {
                    this.secondQuizCard = Instantiate(cardPrefab, gamePanel).GetComponent<CardController>();
                    this.secondQuizCard.SetNextPosition(QuizCardController.PositionType.SECOND, null);
                }
            });
        }
    }

    public void PreviousQuiz()
    {
        if (this.currentQuizSequence - 1 >= 0)
        {
            if (this.secondQuizCard)
            {
                this.tempQuizCard = this.secondQuizCard.MoveToPrevious(() =>
                {
                    this.secondQuizCard = null;
                    this.secondQuizCard = this.firstQuizCard.MoveToPrevious(() =>
                    {
                        this.firstQuizCard = null;
                        this.currentQuizSequence--;

                        this.tempQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                        this.firstQuizCard = this.tempQuizCard.MoveToPrevious(() =>
                        {
                            this.tempQuizCard = null;
                        });
                    });
                });
            }
            else
            {
                this.secondQuizCard = this.firstQuizCard.MoveToPrevious(() => {
                    this.firstQuizCard = null;

                    this.currentQuizSequence--;
                    this.tempQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                    this.firstQuizCard = this.tempQuizCard.MoveToPrevious(() => {
                        this.tempQuizCard = null;
                    });
                });
            }
        }
    }

    public void NextQuiz()
    {
        if (this.firstQuizCard)
        {
            if (this.tempQuizCard) Destroy(this.tempQuizCard.gameObject);

            this.tempQuizCard = this.firstQuizCard.MoveToNext(() =>
            {
                this.firstQuizCard = null;
                if (this.secondQuizCard)
                {
                    this.currentQuizSequence++;   
                    this.secondQuizCard.SetQuiz(this.quizList[this.currentQuizSequence], this.currentQuizSequence, this.quizList.Count);

                    this.firstQuizCard = this.secondQuizCard.MoveToNext(() =>
                    {
                        this.secondQuizCard = null;

                        if (this.currentQuizSequence + 1 < this.quizList.Count)
                        {
                            this.secondQuizCard = this.tempQuizCard.MoveToNext(() =>
                            {
                                this.tempQuizCard = null;
                            });
                        }
                    });
                }
                else
                {
                    // Destroy unused quiz card
                    Destroy(this.tempQuizCard.gameObject);

                    // Increase stage index
                    Informations.StageIndex++;

                    // Show the "Stage Clear" panel
                    StageClear stageClear = Instantiate(stageClearPrefab, this.gamePanel).GetComponent<StageClear>();

                    stageClear.Show(3f, () =>
                    {
                        // Game Over
                        if (Informations.StageIndex < CSVReader.Instance.TotalStage)
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                Destroy(stageClear.gameObject);
                                this.StartGameCanvas(Informations.StageIndex);
                            });
                            long score = Informations.StageIndex * 5;
                        }
                        else
                        {
                            DOVirtual.DelayedCall(0.5f, () =>
                            {
                                Destroy(stageClear.gameObject);
                                this.AllClearGame();
                            });
                        }
                    });
                }
            });
        }
    }

    public void ChangeBackgrounColor(bool isCorrect)
    {
        float duration = 3f;
        float startDuration = 0.2f;
        float endDuration = 1f;

        if (isCorrect)
        {
            this.gamePanelImage.DOBlendableColor(new Color32(41, 171, 226, 255), startDuration);

            DOVirtual.DelayedCall(duration, () =>
            {
                this.gamePanelImage.DOBlendableColor(new Color32(242, 68, 149, 255), endDuration).OnComplete(() =>
                {

                });
            });
        }
        else
        {
            this.gamePanelImage.DOBlendableColor(new Color32(44, 55, 89, 255), startDuration);

            DOVirtual.DelayedCall(duration, () =>
            {
                this.gamePanelImage.DOBlendableColor(new Color32(242, 68, 149, 255), endDuration);
            });
        }
    }

    public void AllClearGame()
    {
        if (firstQuizCard) Destroy(firstQuizCard.gameObject);
        if (secondQuizCard) Destroy(secondQuizCard.gameObject);
        if (tempQuizCard) Destroy(tempQuizCard.gameObject);

        ConfirmPopupController confirmPopupController = Instantiate(confirmPopupPrefab, transform).GetComponent<ConfirmPopupController>();
        confirmPopupController.Show("All stages have been cleared.\nWe will create new quiz content soon.", ConfirmPopupController.ConfirmType.NORMAL, false, () =>
        {
            CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
            canvasManager.ShowMain();
        });
    }

    public void FinishGame()
    {
        if (this.secondQuizCard)
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                this.secondQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                    canvasManager.ShowMain();

                    Destroy(this.secondQuizCard.gameObject);
                });
            });

            if (this.firstQuizCard)
            {
                this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    Destroy(this.firstQuizCard.gameObject);
                });
            }
        }
        else
        {
            if (this.firstQuizCard)
            {
                this.firstQuizCard.SetNextPosition(QuizCardController.PositionType.NONE, () =>
                {
                    CanvasManager canvasManager = GameObject.Find("/Canvas Manager").GetComponent<CanvasManager>();
                    canvasManager.ShowMain();

                    Destroy(this.firstQuizCard.gameObject);
                });
            }
        }

        if (this.tempQuizCard) Destroy(this.tempQuizCard.gameObject);

        if (Informations.IsPlaySFX) audioSource.PlayOneShot(finishGameSFX);
    }
}
