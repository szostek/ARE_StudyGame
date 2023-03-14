using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class QuizBox : MonoBehaviour
{
    [SerializeField] GameObject quizPanel;
    [SerializeField] RectTransform quizArea;
    [SerializeField] Animator correctAnimator;

    enum PositionType { PREVIOUS, CURRENT, NEXT }
    private PositionType currentPositionType;

    enum QuizBoxType { QUIZ, RESULT, CORRECT_NEXT, INCORRECT_NEXT, CLEAR }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public delegate void FinishedMoveToNext();

    private void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        this.canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        this.rectTransform.anchorMin = new Vector2(0.92f, 0.03f);
        this.rectTransform.anchorMax = new Vector2(1.72f, 0.97f);
        this.rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        this.canvasGroup.alpha = 0f;

        this.quizPanel.SetActive(false);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            this.canvasGroup.DOFade(0.6f, 0.2f);
        });

        this.currentPositionType = PositionType.PREVIOUS;
    }

    public QuizBox MoveToNext(FinishedMoveToNext callback)
    {
        float duration = 0.3f;
        float alpha = 0.6f;

        switch (this.currentPositionType)
        {
            case PositionType.NEXT:
                this.canvasGroup.alpha = 0f;
                this.rectTransform.DOAnchorMin(new Vector2(0.92f, 0.03f), 0f);
                this.rectTransform.DOAnchorMax(new Vector2(1.72f, 0.97f), 0f);
                this.rectTransform.DOScale(0.9f, 0f);

                DOVirtual.DelayedCall(0.3f, () =>
                 {
                     this.canvasGroup.DOFade(alpha, 0.2f);
                 });
                this.currentPositionType = PositionType.PREVIOUS;

                break;

            case PositionType.PREVIOUS:
                this.rectTransform.DOAnchorMin(new Vector2(0.07f, 0.03f), duration);
                this.rectTransform.DOAnchorMax(new Vector2(0.93f, 0.97f), duration);
                this.rectTransform.DOScale(1f, duration);
                this.canvasGroup.DOFade(1f, duration).OnComplete(() => this.quizPanel.SetActive(true));
                this.currentPositionType = PositionType.CURRENT;

                break;

            case PositionType.CURRENT:
                this.quizPanel.SetActive(false);

                this.rectTransform.DOAnchorMin(new Vector2(-0.72f, 0.03f), duration);
                this.rectTransform.DOAnchorMax(new Vector2(0.08f, 0.97f), duration);
                this.rectTransform.DOScale(0.9f, duration);
                this.canvasGroup.DOFade(alpha, duration);
                this.currentPositionType = PositionType.NEXT;

                break;
        }

        DOVirtual.DelayedCall(duration, () => callback?.Invoke());

        return this;
    }

    public void OnClickExampleButton(int buttonIndex)
    {
        GameCanvasController gameCanvasController = GameObject.Find("/Game Canvas").GetComponent<GameCanvasController>();
        this.correctAnimator.SetTrigger("show");

        DOVirtual.DelayedCall(3f, () =>
        {
            this.rectTransform.DOScaleX(0f, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                this.quizPanel.SetActive(false);
                this.rectTransform.DOScaleX(1f, 0.2f).SetEase(Ease.OutBack);

                DOVirtual.DelayedCall(3f, () =>
                {
                    gameCanvasController.NextQuiz();
                });
            });
        });
    }
}
