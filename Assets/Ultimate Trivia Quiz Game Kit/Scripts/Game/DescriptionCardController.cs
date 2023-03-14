using UnityEngine;

public class DescriptionCardController : CardController
{
    [SerializeField] DescriptionPanelController descriptionPanelController;

    private Quiz quiz;
    private int currentQuizSequence;
    private int maxQuizAmount;

    override public void SetQuiz(Quiz quiz, int currentQuizSequence, int maxQuizAmount)
    {        
        this.ShowSubPanel(SubPanelType.NONE);
        this.quiz = quiz;
        this.currentQuizSequence = currentQuizSequence;
        this.maxQuizAmount = maxQuizAmount;
    }

    public override void ShowSubPanel(SubPanelType type)
    {
        switch (type)
        {
            case SubPanelType.NONE:
                this.descriptionPanelController.gameObject.SetActive(false);
                break;

            case SubPanelType.QUIZ:
                this.descriptionPanelController.SetDescription(this.quiz.description, this.currentQuizSequence, this.maxQuizAmount);
                this.descriptionPanelController.gameObject.SetActive(true);
                break;
        }
    }

    public void OnClickNextButton()
    {
        this.descriptionPanelController.SetInteractableButtons(false);
        this.gameCanvasController.NextQuiz();
    }

    public void OnClickPreviousButton()
    {
        this.descriptionPanelController.SetInteractableButtons(false);
        this.gameCanvasController.PreviousQuiz();
    }

    public void OnClickExitButton()
    {
        this.descriptionPanelController.SetInteractableButtons(false);

        ConfirmPopupController confirmPopupController = Instantiate(this.confirmPopupPanelPrefab, transform.parent).GetComponent<ConfirmPopupController>();
        confirmPopupController.Show("Do you want to quit?", ConfirmPopupController.ConfirmType.NORMAL, true, () => {
            this.gameCanvasController.FinishGame();
        }, () =>
        {
            this.descriptionPanelController.SetInteractableButtons(true);
        });
    }
}
