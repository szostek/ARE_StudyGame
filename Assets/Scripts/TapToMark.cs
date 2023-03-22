using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TapToMark : MonoBehaviour, IPointerClickHandler
{
    public GameObject correctAreaPrefab; // The rect transform of the correct area
    private GameObject currentAreaMarker;
    public Vector2 correctAreaLocation;

    public GameObject markerPrefab; // The prefab for the marker
    private GameObject currentMarker; // The current marker in the scene

    private RectTransform rectTransform; // The rect transform of the image
    public bool isBuilder = false;
    public bool hasUploadedTapImage = false;

    private MultiCardController cardController;
    private QBuilderManager builderManager;

    private void Awake()
    {
        // Get the rect transform of the image
        rectTransform = GetComponent<RectTransform>();
        cardController = GetComponentInParent<MultiCardController>();
        builderManager = FindObjectOfType<QBuilderManager>();
    }

    private void Start() 
    {
        foreach (RectTransform child in rectTransform.GetComponentInChildren<RectTransform>()) {
            Destroy(child.gameObject);
        }
        if (!isBuilder) {
            currentAreaMarker = Instantiate(correctAreaPrefab, rectTransform);
            currentAreaMarker.GetComponent<Image>().enabled = false;
            Debug.Log(currentAreaMarker.GetComponent<Image>().enabled);
            currentAreaMarker.transform.localPosition = correctAreaLocation;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Get the position of the tap on the image
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        if (!isBuilder) {
            // Destroy the current marker if it exists
            if (currentMarker != null)
            {
                Destroy(currentMarker);
            }

            // Instantiate a marker at the tapped location
            currentMarker = Instantiate(markerPrefab, rectTransform.TransformPoint(localPoint), Quaternion.identity);
            currentMarker.transform.SetParent(rectTransform);

            // Test if the marker is within the correct area
            if (RectTransformUtility.RectangleContainsScreenPoint(currentAreaMarker.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
            {
                Debug.Log("Correct!");
                cardController.tappedOnCorrectArea = true;
            }
            cardController.hasTappedImage = true;
        } else {
            if (hasUploadedTapImage) {
                if (currentAreaMarker != null)
                {
                    Destroy(currentAreaMarker.gameObject);
                }
                // Instantiate an area marker at the tapped location
                currentAreaMarker = Instantiate(correctAreaPrefab, rectTransform.TransformPoint(localPoint), Quaternion.identity);
                currentAreaMarker.transform.SetParent(rectTransform);
                currentAreaMarker.transform.localScale = new Vector3(1, 1, 1);
                builderManager.correctTapAreaPosition = currentAreaMarker.transform.localPosition;
                Debug.Log(currentAreaMarker.transform.localPosition);
            }
        }
    }


}
