using UnityEngine;
using UnityEngine.UI;

public class TextExampleButton : ExampleButton
{
    [SerializeField]
    private Text buttonText;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetCorrect()
    {
        base.SetCorrect();
        buttonText.color = whiteGrayColor;
    }

    public override void SetIncorrect()
    {
        base.SetIncorrect();
        buttonText.color = whiteGrayColor;
    }

    public override void SetNormal()
    {
        base.SetNormal();
        buttonText.color = darkBlueColor;
    }
}
