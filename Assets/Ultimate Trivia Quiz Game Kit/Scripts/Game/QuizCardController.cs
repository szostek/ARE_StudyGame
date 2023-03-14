using UnityEngine;
using DG.Tweening;

public class QuizCardController : CardController
{
    [SerializeField] QuizPanelController quizPanelController;
    [SerializeField] NextPanelController nextPanelController;
    [SerializeField] AudioClip correctSFX;
    [SerializeField] AudioClip incorrectSFX;

    private Quiz quiz;
    private int currentQuizSequence;
    private int maxQuizAmount;

    protected override void Awake()
    {
        base.Awake();
    }

    override public void SetQuiz(Quiz quiz, int currentQuizSequence, int maxQuizAmount)
    {
        this.ShowSubPanel(SubPanelType.NONE);

        this.quiz = quiz;

        this.currentQuizSequence = currentQuizSequence;
        this.maxQuizAmount = maxQuizAmount;
    }

    private void QuizCardEffect(bool isCorrec)
    {
        if (isCorrec)
        {
            if (Informations.IsPlaySFX) audioSource.PlayOneShot(correctSFX);
        }
        else
        {
            this.rectTransform.DOPunchRotation(new Vector3(0f, 0f, 10f), 0.5f);
            if (Informations.IsPlaySFX) audioSource.PlayOneShot(incorrectSFX);
        }
    }

    public void OnClickExampleButton(int buttonIndex)
    {
        this.quizPanelController.SetInteractableButtons(false);
        
        if (buttonIndex + 1 == this.quiz.answer)
        {
            this.quizPanelController.SetCorrectIncorrectButton(buttonIndex, true);

            this.gameCanvasController.ChangeBackgrounColor(true);
            this.quizPanelController.ShowResult(QuizPanelController.ResultType.CORRECT);
            this.QuizCardEffect(true);

            DOVirtual.DelayedCall(3f, () =>
            {
                this.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.ShowSubPanel(SubPanelType.NEXT);
                    this.nextPanelController.SetPanel(true);

                    this.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);
                });
            });
        }
        else
        {
            this.quizPanelController.SetCorrectIncorrectButton(buttonIndex, false);
            this.gameCanvasController.ChangeBackgrounColor(false);
            this.quizPanelController.ShowResult(QuizPanelController.ResultType.INCORRECT);
            this.QuizCardEffect(false);

            DOVirtual.DelayedCall(3f, () =>
            {
                this.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.ShowSubPanel(SubPanelType.NEXT);
                    this.nextPanelController.SetPanel(false);

                    this.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);
                });
            });
        }
    }

    public void OnClickNextButton()
    {
        this.nextPanelController.SetInteractableButtons(false);

        this.gameCanvasController.NextQuiz();
    }

    public void OnClickContinueButton()
    {
        this.nextPanelController.SetInteractableButtons(false);

        if (Informations.HeartAmount > 0)
        {
            this.nextPanelController.SetSubtractHeart(() =>
            {
                this.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.ShowSubPanel(SubPanelType.QUIZ);
                    this.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);
                });
            });
        }
        else
        {
            this.nextPanelController.SetNotEnough(() =>
            {
                this.nextPanelController.SetInteractableButtons(true);
            });
        }
    }

    public void OnClickFinishButton()
    {
        this.nextPanelController.SetInteractableButtons(false);

        ConfirmPopupController confirmPopupController = Instantiate(this.confirmPopupPanelPrefab, transform.parent).GetComponent<ConfirmPopupController>();
        confirmPopupController.Show("Do you want to quit?", ConfirmPopupController.ConfirmType.NORMAL, true, () => {
            this.gameCanvasController.FinishGame();
        }, () =>
        {
            this.nextPanelController.SetInteractableButtons(true);
        });
    }

    public void Timeout()
    {
        this.quizPanelController.SetInteractableButtons(false);

        this.gameCanvasController.ChangeBackgrounColor(false);
        this.quizPanelController.ShowResult(QuizPanelController.ResultType.TIMEOUT);

        this.QuizCardEffect(false);

        DOVirtual.DelayedCall(3f, () =>
        {
            this.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                this.ShowSubPanel(SubPanelType.NEXT);
                this.nextPanelController.SetPanel(false);

                this.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);
            });
        });
    }

    public override void ShowSubPanel(SubPanelType type)
    {
        switch (type)
        {
            case SubPanelType.NONE:
                this.quizPanelController.gameObject.SetActive(false);
                this.nextPanelController.gameObject.SetActive(false);
                break;

            case SubPanelType.QUIZ:
                this.quizPanelController.gameObject.SetActive(true);
                this.quizPanelController.SetQuiz(this.quiz, this.currentQuizSequence);
                this.quizPanelController.SetInteractableButtons(true);
                this.nextPanelController.gameObject.SetActive(false);
                break;

            case SubPanelType.NEXT:
                this.nextPanelController.gameObject.SetActive(true);
                this.nextPanelController.SetDescription(this.quiz.description, this.currentQuizSequence, this.maxQuizAmount);
                this.quizPanelController.gameObject.SetActive(false);
                break;
        }
    }
}
