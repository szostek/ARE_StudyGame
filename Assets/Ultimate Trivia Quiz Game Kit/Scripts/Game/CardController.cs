using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public abstract class CardController : MonoBehaviour
{
    [SerializeField] AudioClip moveToNextSFX;
    [SerializeField] AudioClip moveToPreviousSFX;

    protected AudioSource audioSource;

    public GameObject confirmPopupPanelPrefab;
    public enum SubPanelType { NONE, QUIZ, NEXT }
    public enum PositionType { NONE, FIRST, SECOND }
    protected PositionType currentPositionType;

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected GameCanvasController gameCanvasController;

    public delegate void FinishedMove();

    public abstract void ShowSubPanel(SubPanelType type);

    protected virtual void Awake()
    {
        this.rectTransform = this.GetComponent<RectTransform>();
        this.canvasGroup = this.GetComponent<CanvasGroup>();
        this.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        this.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
        this.canvasGroup.alpha = 0f;
        this.rectTransform.SetAsFirstSibling();
        this.gameCanvasController = GameObject.Find("/Game Canvas").GetComponent<GameCanvasController>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start() { }
    protected virtual void Update() { }

    public abstract void SetQuiz(Quiz quiz, int currentQuizSequence, int maxQuizAmount);

    public void SetPreviousPosition(PositionType type, FinishedMove callback)
    {
        float duration = 0.2f;

        switch (type)
        {
            case PositionType.SECOND:
                this.currentPositionType = PositionType.SECOND;
                this.ShowSubPanel(SubPanelType.NONE);
                this.rectTransform.DOAnchorMin(new Vector2(0.1f, 0.19f), duration);
                this.rectTransform.DOAnchorMax(new Vector2(0.9f, 0.99f), duration);
                this.rectTransform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), duration);
                this.canvasGroup.DOFade(0.5f, duration).OnComplete(()=>
                {
                    callback?.Invoke();
                });
                break;
            case PositionType.FIRST:
                this.currentPositionType = PositionType.FIRST;
                this.gameObject.SetActive(true);
                this.rectTransform.SetAsLastSibling();
                this.ShowSubPanel(SubPanelType.QUIZ);
                this.canvasGroup.DOFade(1f, duration);
                this.rectTransform.DOLocalMoveY(0f, duration * 2).SetEase(Ease.OutBack).OnComplete(() => {
                    callback?.Invoke();                    
                });
                break;
            case PositionType.NONE:
                this.currentPositionType = PositionType.NONE;
                this.rectTransform.DOAnchorMin(new Vector2(0.1f, 0.28f), 0f);
                this.rectTransform.DOAnchorMax(new Vector2(0.9f, 1.08f), 0f);
                this.rectTransform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0f);
                this.canvasGroup.DOFade(0f, 0f).OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    this.rectTransform.anchoredPosition = new Vector2(0f, -500f);
                    this.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
                    this.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
                    this.rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    callback?.Invoke();
                });
                break;
        }
    }

    public void SetNextPosition(PositionType type, FinishedMove callback)
    {
        float duration = 0.2f;

        switch (type)
        {
            case PositionType.SECOND:
                this.currentPositionType = PositionType.SECOND;
                this.rectTransform.anchoredPosition = new Vector2(0f, 0f);
                this.ShowSubPanel(SubPanelType.NONE);
                this.gameObject.SetActive(true);
                this.canvasGroup.alpha = 0f;
                this.rectTransform.anchorMin = new Vector2(0.1f, 0.19f);
                this.rectTransform.anchorMax = new Vector2(0.9f, 0.99f);
                this.rectTransform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                this.canvasGroup.DOFade(0.5f, duration).OnComplete(()=>
                {
                    callback?.Invoke();
                });
                break;
            case PositionType.FIRST:
                this.currentPositionType = PositionType.FIRST;
                this.rectTransform.SetAsLastSibling();
                this.rectTransform.DOScale(1f, duration);
                this.rectTransform.DOAnchorMin(new Vector2(0.1f, 0.1f), duration);
                this.rectTransform.DOAnchorMax(new Vector2(0.9f, 0.9f), duration);
                this.canvasGroup.DOFade(1f, duration).OnComplete(() =>
                {
                    this.ShowSubPanel(SubPanelType.QUIZ);
                    callback?.Invoke();
                });
                break;
            case PositionType.NONE:
                this.currentPositionType = PositionType.NONE;                
                this.rectTransform.DOLocalMoveY(-500f, duration * 2).SetEase(Ease.InBack);
                this.canvasGroup.DOFade(0f, duration * 2).SetEase(Ease.InBack).OnComplete(() =>
                {
                    this.ShowSubPanel(SubPanelType.NONE);
                    this.gameObject.SetActive(false);
                    callback?.Invoke();
                });
                break;
        }
    }
    public CardController MoveToNext(FinishedMove callback)
    {
        switch (this.currentPositionType)
        {
            case PositionType.FIRST:
                this.SetNextPosition(PositionType.NONE, callback);
                if (Informations.IsPlaySFX) audioSource.PlayOneShot(moveToNextSFX);
                break;

            case PositionType.SECOND:
                this.SetNextPosition(PositionType.FIRST, callback);
                break;

            case PositionType.NONE:
                this.SetNextPosition(PositionType.SECOND, callback);
                break;
        }

        return this;
    }

    public CardController MoveToPrevious(FinishedMove callback)
    {
        switch (this.currentPositionType)
        {
            case PositionType.FIRST:
                this.SetPreviousPosition(PositionType.SECOND, callback);
                if (Informations.IsPlaySFX) audioSource.PlayOneShot(moveToPreviousSFX);
                break;

            case PositionType.SECOND:
                this.SetPreviousPosition(PositionType.NONE, callback);
                break;

            case PositionType.NONE:
                this.SetPreviousPosition(PositionType.FIRST, callback);
                break;
        }
        return this;
    }
}
