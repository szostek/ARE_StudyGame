using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject normalButton;
    [SerializeField] GameObject clearButton;
    [SerializeField] GameObject lockButton;

    [SerializeField] Text[] levelIndexTexts;

    private int currentIndex;

    public enum LevelButtonType { NORMAL, CLEAR, LOCK }
    private LevelButtonType type;
    private LevelButtonType Type
    {
        get
        {
            return this.type;
        }
        set
        {
            this.type = value;

            switch(type)
            {
                case LevelButtonType.NORMAL:
                    this.normalButton.SetActive(true);
                    this.clearButton.SetActive(false);
                    this.lockButton.SetActive(false);
                    break;
                case LevelButtonType.LOCK:
                    this.normalButton.SetActive(false);
                    this.clearButton.SetActive(false);
                    this.lockButton.SetActive(true);
                    break;
                case LevelButtonType.CLEAR:
                    this.normalButton.SetActive(false);
                    this.clearButton.SetActive(true);
                    this.lockButton.SetActive(false);
                    break;
            }
        }
    }

    public void Show(LevelButtonType type, int index)
    {
        this.Type = type;
        this.currentIndex = index;

        foreach (Text levelIndexText in levelIndexTexts)
        {
            levelIndexText.text = (index + 1).ToString();
        }
    }

    public void OnClickNormalButton() { }

    public void OnClickClearButton()
    {
        MainCanvasController mainCanvasController = GameObject.Find("/Main Canvas").GetComponent<MainCanvasController>();
        mainCanvasController.ShowDescriptions(this.currentIndex);
    }

}
