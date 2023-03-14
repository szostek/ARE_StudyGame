using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CircleEffect : MonoBehaviour
{
    [SerializeField] GameObject circleEffect;

    private bool isPlay = false;

    public void Play()
    {

        if (!this.isPlay)
        {
            float circleEffectDuration = 2f;
            this.circleEffect.SetActive(true);

            this.circleEffect.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            this.circleEffect.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            this.circleEffect.GetComponent<RectTransform>().DOScale(1.5f, circleEffectDuration).SetLoops(-1);
            this.circleEffect.GetComponent<Image>().DOFade(0f, circleEffectDuration).SetLoops(-1);

            this.isPlay = true;
        }
    }

    public void Stop()
    {
        if (this.isPlay)
        {
            this.circleEffect.GetComponent<RectTransform>().DOKill();
            this.circleEffect.GetComponent<Image>().DOKill();
            this.circleEffect.SetActive(false);
            this.isPlay = false;
        }
    }
}
