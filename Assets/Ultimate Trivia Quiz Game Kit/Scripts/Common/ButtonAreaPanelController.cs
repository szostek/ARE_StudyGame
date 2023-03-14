using UnityEngine;
using UnityEngine.UI;

public class ButtonAreaPanelController : MonoBehaviour
{
    [SerializeField]
    protected Button[] buttons;

    public void SetCorrectIncorrectButton(int index, bool isCorrect)
    {
        if (isCorrect)
        {
            buttons[index].GetComponent<ExampleButton>().SetCorrect();
        }
        else
        {
            buttons[index].GetComponent<ExampleButton>().SetIncorrect();
        }
    }

    public void SetInteractableButtons(bool isInteractable)
    {
        foreach (Button button in buttons)
        {
            button.interactable = isInteractable;
        }
    }
}
