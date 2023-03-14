using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NextPanelController : MonoBehaviour
{
    [SerializeField] GameObject correctArea;
    [SerializeField] GameObject incorrectArea;

    [SerializeField] Button[] buttons;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject continueAfterAdsButton;

    [SerializeField] Text descriptionText;

    [SerializeField] Heart heart;
    [SerializeField] Image guageImage;
    [SerializeField] Text levelUpMessageText;
    [SerializeField] Text quizProgressText;

    [SerializeField] GameObject confirmPopupPrefab;
    [SerializeField] Transform popupArea;

    private string quizDescription;
    private int maxQuizAmount;
    private int currentQuizSequence;

    private Coroutine checkRewardedCoroutine;

    private void OnEnable()
    {
        this.correctArea.SetActive(false);
        this.incorrectArea.SetActive(false);
    }

    public void SetDescription(string quizDescription, int currentQuizSequence, int maxQuizAmount)
    {
        this.quizDescription = quizDescription;
        this.maxQuizAmount = maxQuizAmount;
        this.currentQuizSequence = currentQuizSequence;
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

            this.heart.Show(0f);

            this.guageImage.rectTransform.anchorMax = new Vector2(0f, 1f);
            float guageValue = (float)this.currentQuizSequence / (float)this.maxQuizAmount;
            this.guageImage.rectTransform.DOAnchorMax(new Vector2(guageValue, 1f), 1f);
            this.levelUpMessageText.text = string.Format("You have to solve {0} quizzes\nto level up.", this.maxQuizAmount - this.currentQuizSequence);

            if (Informations.HeartAmount > 0)
            {
                this.continueButton.SetActive(true);
                this.continueAfterAdsButton.SetActive(false);
            }
            else
            {
                this.continueButton.SetActive(false);
                this.continueAfterAdsButton.SetActive(true);
            }
        }
    }

    public void SetInteractableButtons(bool isInteractable)
    {
        foreach (Button button in this.buttons)
        {
            button.interactable = isInteractable;
        }
    }

    public void SetSubtractHeart(Heart.AfterSubtractHeart callback)
    {
        this.heart.SubtractHeart(callback);
    }

    public void SetNotEnough(Heart.AfterSubtractHeart callback)
    {
        this.heart.NotEnough(callback);
    }

    #region RewardBasedVideo
    public void OnClickRewardBasedVideoButton()
    {
        if (checkRewardedCoroutine != null)
        {
            StopCoroutine(checkRewardedCoroutine);
        }
        checkRewardedCoroutine = StartCoroutine(CheckRewarded());
    }

    IEnumerator CheckRewarded()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion
}