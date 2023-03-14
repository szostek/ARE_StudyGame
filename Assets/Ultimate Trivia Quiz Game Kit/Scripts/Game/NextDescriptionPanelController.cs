using UnityEngine;
using UnityEngine.UI;

public class NextDescriptionPanelController : MonoBehaviour
{
    [SerializeField] GameObject correctArea;
    [SerializeField] GameObject incorrectArea;

    [SerializeField] Button[] buttons;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject nextButton;

    [SerializeField] Text descriptionText;

    private string quizDescription;

    private void OnEnable()
    {
        this.correctArea.SetActive(false);
        this.incorrectArea.SetActive(false);
    }

    public void SetDescription(string quizDescription, int currentQuizSequence, int maxQuizAmount)
    {
        this.quizDescription = quizDescription;

        if (maxQuizAmount == currentQuizSequence + 1)
        {
            this.nextButton.gameObject.SetActive(false);
        }
        else
        {
            this.nextButton.gameObject.SetActive(true);
        }
    }

    public void SetPanel(bool isCorrect)
    {
        this.SetInteractableButtons(true);

        if (isCorrect)
        {
            this.correctArea.SetActive(true);
            this.incorrectArea.SetActive(false);

            this.descriptionText.text = this.quizDescription;
        }
        else
        {
            this.correctArea.SetActive(false);
            this.incorrectArea.SetActive(true);
        }
    }

    public void SetInteractableButtons(bool isInteractable)
    {
        foreach (Button button in this.buttons)
        {
            button.interactable = isInteractable;
        }
    }
}
