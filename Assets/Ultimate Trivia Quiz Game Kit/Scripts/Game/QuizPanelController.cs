using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuizPanelController : MonoBehaviour
{
    [SerializeField] GameObject quizArea;
    [SerializeField] GameObject resultArea;

    [SerializeField] GameObject exampleButtonArea;
    [SerializeField] GameObject oxButtonArea;
    ButtonAreaPanelController buttonAreaPanelController;

    [SerializeField] CircularTimer timer;
    [SerializeField] Text quizSequenceText;
    [SerializeField] Text quizText;

    [SerializeField] Animator correctAnimator;
    [SerializeField] Animator incorrectAnimator;
    [SerializeField] Text resultText;

    [SerializeField] Button[] buttons;

    [SerializeField] Image pangEffectImage;
    [SerializeField] Image quizCardEffectImage;

    private string[] correctMessages = new string[]
    {
        "정답", "굉장해요",
        "UNBELIEVABLE","CORRECT", "GREAT", "WOW",
        "BON", "WAOUH"
    };

    private string[] incorrectMessages = new string[]
    {
        "헐", "틀렸습니다",
        "OMG", "INCORRECT",
        "SEGOURER"
    };

    private string[] timesupMessages = new string[]
    {
        "시간초과", "TIME'S UP", "HECK", "WHEW"
    };

    public enum ResultType { CORRECT, INCORRECT, TIMEOUT }

    private void OnEnable()
    {
        this.resultArea.SetActive(false);
        this.quizArea.SetActive(true);
        this.pangEffectImage.DOFade(0f, 0f);
        this.quizCardEffectImage.DOFade(0f, 0f);
    }

    public void SetQuiz(Quiz quiz, int quizSequence)
    {
        this.correctAnimator.gameObject.SetActive(false);
        this.incorrectAnimator.gameObject.SetActive(false);

        string quizSequenceText = (++quizSequence).ToString();
        this.quizText.text = quiz.question;
        this.quizSequenceText.text = quizSequenceText;

        switch (quiz.type)
        {
            case 1:
                oxButtonArea.SetActive(true);
                exampleButtonArea.SetActive(false);
                buttonAreaPanelController = oxButtonArea.GetComponent<OXButtonAreaPanelController>();
                break;
            case 2:
                oxButtonArea.SetActive(false);
                exampleButtonArea.SetActive(true);
                exampleButtonArea.GetComponent<ExampleButtonAreaPanelController>().SetExamples(quiz);
                buttonAreaPanelController = exampleButtonArea.GetComponent<ExampleButtonAreaPanelController>();
                break;
            default:
                oxButtonArea.SetActive(false);
                exampleButtonArea.SetActive(false);
                buttonAreaPanelController = null;
                break;
        }
        this.timer.StartTimer();
    }

    public void ShowResult(ResultType type)
    {
        float textAnimationIdleDuration = 0.5f;
        this.resultArea.SetActive(true);
        this.quizArea.SetActive(false);
        this.timer.StopTimer();
        this.resultText.text = "";
        switch (type)
        {
            case ResultType.CORRECT:
                this.pangEffectImage.DOFade(1f, 0f);

                this.quizCardEffectImage.DOFade(1f, 0f);
                this.quizCardEffectImage.DOFade(0f, 1f);
                this.quizCardEffectImage.rectTransform.DOScale(1.3f, 1f).OnComplete(() =>
                {
                    this.quizCardEffectImage.rectTransform.DOScale(1f, 0f);
                    this.pangEffectImage.rectTransform.DOScale(0.2f, 1f).OnComplete(() =>
                    {
                        this.pangEffectImage.DOFade(0f, 0f).OnComplete(() =>
                        {
                            this.pangEffectImage.rectTransform.DOScale(1f, 0f);
                        });
                    });
                });

                this.correctAnimator.gameObject.SetActive(true);
                this.correctAnimator.SetTrigger("show");

                DOVirtual.DelayedCall(textAnimationIdleDuration, () =>
                {
                    int r = Random.Range(0, correctMessages.Length);
                    this.resultText.DOText(correctMessages[r], 1f);
                });
                break;
            case ResultType.INCORRECT:
                this.incorrectAnimator.gameObject.SetActive(true);
                this.incorrectAnimator.SetTrigger("show");
                DOVirtual.DelayedCall(textAnimationIdleDuration, () =>
                {
                    int r = Random.Range(0, incorrectMessages.Length);
                    this.resultText.DOText(incorrectMessages[r], 1f);
                });
                break;
            case ResultType.TIMEOUT:
                this.incorrectAnimator.gameObject.SetActive(true);
                this.incorrectAnimator.SetTrigger("show");
                DOVirtual.DelayedCall(textAnimationIdleDuration, () =>
                {
                    int r = Random.Range(0, timesupMessages.Length);
                    this.resultText.DOText(timesupMessages[r], 1f);
                });
                break;
        }
    }

    public void SetCorrectIncorrectButton(int index, bool isCorrect)
    {
        buttonAreaPanelController.SetCorrectIncorrectButton(index, isCorrect);
    }

    public void SetInteractableButtons(bool isInteractable)
    {
        buttonAreaPanelController.SetInteractableButtons(isInteractable);
    }
}
