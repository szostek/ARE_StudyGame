using UnityEngine;
using UnityEngine.UI;

public class DescriptionPanelController : MonoBehaviour
{
    [SerializeField] Text descriptionText;
    [SerializeField] Button[] buttons;

    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;

    private void OnEnable() {
        this.SetInteractableButtons(true);
    }

    public void SetDescription(string description, int currentQuizSequence, int maxQuizAmount)
    {
        this.descriptionText.text = description;

        if (currentQuizSequence == 0)
        {
            this.previousButton.gameObject.SetActive(false);
        }
        else
        {
            this.previousButton.gameObject.SetActive(true);
        }

        if (maxQuizAmount == currentQuizSequence + 1)
        {
            this.nextButton.gameObject.SetActive(false);
        }
        else
        {
            this.nextButton.gameObject.SetActive(true);
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
