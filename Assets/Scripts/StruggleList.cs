using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StruggleList : MonoBehaviour
{
    [SerializeField] StruggleButton struggleButtonPrefab;
    [SerializeField] RectTransform content;
    private GameManager gameManager;

    private void Awake() 
    {
        gameManager = GetComponent<GameManager>();    
    }

    public void PopulateStruggles(List<int> struggles)
    {
        foreach (RectTransform item in content) {
            Destroy(item.gameObject);
        }
        foreach (int struggleId in struggles) {
            StruggleButton button = Instantiate(struggleButtonPrefab, content);
            button.struggleList = this;
            button.qIndex = struggleId;
            button.buttonText.text = struggleId.ToString();
        }
    }

    public void GetStruggleQuestion(int qIndex)
    {
        gameManager.CreateStrugglePreviewQuestion(qIndex);
    }
}
