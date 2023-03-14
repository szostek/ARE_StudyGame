using UnityEngine;
using UnityEngine.UI;

public class ExampleButton : MonoBehaviour
{
    private Image buttonImage;

    protected Color32 darkBlueColor = new Color32(44, 55, 89, 255);
    protected Color32 whiteGrayColor = new Color32(242, 242, 242, 255);
    protected Color32 blackPinkColor = new Color32(242, 68, 149, 255);

    protected virtual void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        SetNormal();
    }

    public virtual void SetCorrect()
    {
        buttonImage.color = darkBlueColor;
    }

    public virtual void SetIncorrect()
    {
        buttonImage.color = blackPinkColor;
    }

    public virtual void SetNormal()
    {
        buttonImage.color = whiteGrayColor;
    }
}
