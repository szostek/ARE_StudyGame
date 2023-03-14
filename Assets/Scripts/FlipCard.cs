using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public float x, y, z;
    public GameObject cardBack;
    public bool doFlip = false;
    private bool cardBackIsActive = false;
    private int timer;

    private void Update() 
    {
        if (doFlip) {
            StartFlip();
        }    
    }

    public void StartFlip()
    {
        StartCoroutine(CalculateFlip());
    }

    private void Flip()
    {
        if (cardBackIsActive) {
            cardBack.SetActive(false);
            cardBackIsActive = false;
        } else {
            cardBack.SetActive(true);
            cardBackIsActive = true;
        }
        doFlip = false;
    }

    IEnumerator CalculateFlip()
    {
        for (int i = 0; i < 180; i++) {
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(new Vector3(x, y, z));
            timer++;
            if (timer == 90 || timer == -90) {
                Flip();
            }
        }
        timer = 0;
    }
}
