using UnityEngine.UI;

public class ExampleButtonAreaPanelController : ButtonAreaPanelController
{
    public void SetExamples(Quiz quiz)
    {
        buttons[0].GetComponentInChildren<Text>().text = quiz.e01;
        buttons[1].GetComponentInChildren<Text>().text = quiz.e02;
        buttons[2].GetComponentInChildren<Text>().text = quiz.e03;
    }
}
