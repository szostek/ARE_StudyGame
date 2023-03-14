using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlideGameCanvasController : MonoBehaviour
{
    [SerializeField] RectTransform gamePanel;
    [SerializeField] GameObject levelBoxPrefab;
    [SerializeField] GameObject quizBoxPrefab;

    private Image gamePanelImage;

    private LevelBox levelBox;

    private QuizBox previousQuizBox = null;
    private QuizBox currentQuizBox = null;
    private QuizBox nextQuizBox = null;

    private void Awake()
    {
        this.gamePanelImage = this.gamePanel.GetComponent<Image>();
    }

    public void StartCanvas()
    {
        this.levelBox = Instantiate(levelBoxPrefab, gamePanel).GetComponent<LevelBox>();

        this.previousQuizBox = Instantiate(quizBoxPrefab, gamePanel).GetComponent<QuizBox>();

        this.levelBox.ShowAndHide(Informations.StageIndex, 0.5f, () =>
        {
            Destroy(this.levelBox.gameObject);

            this.NextQuiz();
        });
    }

    public void StopCanvas(DidFinishedStopCanvas callback)
    {
        callback?.Invoke();
    }

    public void NextQuiz()
    {
        QuizBox newQuizBox = null;

        if (this.nextQuizBox)
        {
            newQuizBox = this.nextQuizBox.MoveToNext(null);
        }
        else
        {
            newQuizBox = Instantiate(quizBoxPrefab, gamePanel).GetComponent<QuizBox>();
        }

        if (this.currentQuizBox)
        {
            this.nextQuizBox = this.currentQuizBox.MoveToNext(null);
        }

        if (this.previousQuizBox)
        {
            this.currentQuizBox = this.previousQuizBox.MoveToNext(null);
            this.previousQuizBox = newQuizBox;
        }
    }

    public void ChangeBackcolor(Color32 endColor, float duration)
    {
        this.gamePanelImage.DOBlendableColor(endColor, duration);
    }
}
