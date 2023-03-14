using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopupController : PopupController
{
    [SerializeField] RectTransform contentArea;
    [SerializeField] GameObject levelButtonPrefab;
    [SerializeField] ScrollRect scrollRect;

    private List<LevelButton> levelButtons;

    override protected void Awake()
    {
        base.Awake();
        this.levelButtons = new List<LevelButton>();

        int maxLevelButtonLine = CSVReader.Instance.TotalStage / 3;
        maxLevelButtonLine += ((CSVReader.Instance.TotalStage % 3) > 0) ? 1 : 0;

        float maxContentAreaHeight = (300f * maxLevelButtonLine) + (25f * (maxLevelButtonLine - 1)) + 50f;

        this.contentArea.sizeDelta = new Vector2(0f, maxContentAreaHeight);

        for (int i = 0; i < CSVReader.Instance.TotalStage; i++)
        {
            LevelButton levelButton = Instantiate(levelButtonPrefab, contentArea).GetComponent<LevelButton>();
            this.levelButtons.Add(levelButton);
        }
    }

    public override PopupController Show()
    {
        this.ShowLevelButton();

        PopupController popupController = base.Show();
        return popupController;
    }

    private void ShowLevelButton()
    {
        for (int i = 0; i < this.levelButtons.Count; i++)
        {
            if (i < Informations.StageIndex)
            {
                this.levelButtons[i].Show(LevelButton.LevelButtonType.CLEAR, i);
            }
            else if (i >= Informations.StageIndex)
            {
                this.levelButtons[i].Show(LevelButton.LevelButtonType.LOCK, i);
            }
        }

        int lineIndex = (Informations.StageIndex - 1) / 3;
        float scrollPosition = (lineIndex * 300f) + (lineIndex * 25f);

        this.contentArea.anchoredPosition = new Vector2(0f, scrollPosition);
    }
}
