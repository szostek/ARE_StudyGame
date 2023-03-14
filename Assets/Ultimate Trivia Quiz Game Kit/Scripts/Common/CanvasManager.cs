using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject gameCanvas;

    private MainCanvasController mainCanvasController;
    private GameCanvasController gameCanvasController;

    private AudioSource audioSource;

    private void Awake()
    {
        this.mainCanvasController = mainCanvas.GetComponent<MainCanvasController>();
        this.gameCanvasController = gameCanvas.GetComponent<GameCanvasController>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        this.ShowMain();
        CheckBGM();
    }

    public void ShowGame(int levelIndex)
    {
        if (levelIndex == Informations.StageIndex)
        {
            this.mainCanvasController.StopCanvas(false ,() =>
            {
                this.mainCanvas.SetActive(false);
                this.gameCanvas.SetActive(true);
                this.gameCanvasController.StartGameCanvas(levelIndex);
            });
        }
        else
        {
            this.mainCanvasController.StopCanvas(true ,() =>
            {
                this.mainCanvas.SetActive(false);
                this.gameCanvas.SetActive(true);
                this.gameCanvasController.StartGameCanvas(levelIndex);
            });
        }
    }

    public void ShowMain()
    {
        this.gameCanvasController.StopCanvas(()=>
        {
            this.gameCanvas.SetActive(false);
            this.mainCanvas.SetActive(true);

            this.mainCanvasController.StartMainCanvas();
        });
    }

    public void CheckBGM()
    {
        if (Informations.IsPlayBGM)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void SetBGM(bool isOn)
    {
        if (isOn && Informations.IsPlayBGM)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}