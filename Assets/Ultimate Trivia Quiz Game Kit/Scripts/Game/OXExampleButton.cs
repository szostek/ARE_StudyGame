using UnityEngine;
using UnityEngine.UI;

public class OXExampleButton : ExampleButton
{
    [SerializeField]
    private Image oxImage;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetCorrect()
    {
        base.SetCorrect();
        oxImage.color = whiteGrayColor;
    }

    public override void SetIncorrect()
    {
        base.SetIncorrect();
        oxImage.color = whiteGrayColor;
    }

    public override void SetNormal()
    {
        base.SetNormal();
        oxImage.color = darkBlueColor;
    }
}
