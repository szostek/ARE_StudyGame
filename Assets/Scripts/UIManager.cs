using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public void ShowNextScreen(GameObject nextScreen)
    {
        nextScreen.SetActive(true);
        nextScreen.transform.localPosition = new Vector2(Screen.width * 2, 0);
        nextScreen.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void BackButton(GameObject currentScreen)
    {
        float moveTime = 0.25f;
        currentScreen.transform.LeanMoveLocalX(Screen.width * 2, moveTime).setEaseInExpo();
        StartCoroutine(HidePanel(currentScreen, moveTime, false));
    }

    public void ShowFinishedPanel(GameObject finishedPanel)
    {
        finishedPanel.SetActive(true);
        finishedPanel.transform.localPosition = new Vector2(Screen.width * 2, -46);
        finishedPanel.transform.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
    }

    public void RemoveCurrentQuestion(GameObject questionCard)
    {
        float moveTime = 0.25f;
        questionCard.transform.LeanMoveLocalX(-Screen.width * 2, moveTime).setEaseInExpo();
        StartCoroutine(HidePanel(questionCard, moveTime, true));
    }

    IEnumerator HidePanel(GameObject screen, float delayTime, bool shouldDestroy)
    {
        yield return new WaitForSeconds(delayTime);
        if (shouldDestroy) {
            Destroy(screen.gameObject);
        } else {
            screen.SetActive(false);
        }
    }

}
