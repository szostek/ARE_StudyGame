using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RemainQuizGauge : MonoBehaviour
{
    [SerializeField] Image guageImage;
    [SerializeField] Text levelUpMessageText;

    public void SetRemainQuizAmount(int currentQuizIndex, int maxQuizAmount)
    {
        this.levelUpMessageText.text = string.Format("You have to solve {0} quizzes\nto level up.", currentQuizIndex);
        this.guageImage.rectTransform.DOAnchorMax(new Vector2(currentQuizIndex / maxQuizAmount, 1f), 1f);
    }
}
