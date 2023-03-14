using UnityEngine;
using UnityEngine.EventSystems;

public class TapToMark : MonoBehaviour, IPointerClickHandler
{
    public GameObject markerPrefab; // The prefab for the marker
    public RectTransform correctArea; // The rect transform of the correct area

    private GameObject currentMarker; // The current marker in the scene
    private RectTransform rectTransform; // The rect transform of the image

    private MultiCardController cardController;

    private void Awake()
    {
        // Get the rect transform of the image
        rectTransform = GetComponent<RectTransform>();
        cardController = GetComponentInParent<MultiCardController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Get the position of the tap on the image
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // Destroy the current marker if it exists
        if (currentMarker != null)
        {
            Destroy(currentMarker);
        }

        // Instantiate a marker at the tapped location
        currentMarker = Instantiate(markerPrefab, rectTransform.TransformPoint(localPoint), Quaternion.identity);
        currentMarker.transform.SetParent(rectTransform);

        // Test if the marker is within the correct area
        if (RectTransformUtility.RectangleContainsScreenPoint(correctArea, eventData.position, eventData.pressEventCamera))
        {
            Debug.Log("Correct!");
            cardController.tappedOnCorrectArea = true;
        }
        cardController.hasTappedImage = true;
    }
}
