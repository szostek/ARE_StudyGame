using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class ZoomAndPan : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private Button resetButton;
    [SerializeField] private float zoomSpeed = 0.1f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 10f;

    private RectTransform rectTransform;
    private Vector2 lastTouchPosition;
    private int activeTouchCount;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        resetButton.onClick.AddListener(ResetZoom);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        activeTouchCount = Input.touchCount;
        if (activeTouchCount == 1)
        {
            lastTouchPosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        #if UNITY_EDITOR
        if (Input.mouseScrollDelta.y != 0) return;
        #endif
        
        if (Input.touchCount == 1 && activeTouchCount == 1)
        {
            Vector2 delta = eventData.position - lastTouchPosition;
            rectTransform.anchoredPosition += delta;
            lastTouchPosition = eventData.position;
        }
        else if (Input.touchCount == 2)
        {
            resetButton.gameObject.SetActive(true);
            scrollView.vertical = false;

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 prevTouch0 = touch0.position - touch0.deltaPosition;
            Vector2 prevTouch1 = touch1.position - touch1.deltaPosition;

            float prevTouchDelta = (prevTouch0 - prevTouch1).magnitude;
            float touchDelta = (touch0.position - touch1.position).magnitude;

            float zoomFactor = (prevTouchDelta / touchDelta) - 1f;
            float scaleFactor = rectTransform.localScale.x * (1f - zoomFactor * zoomSpeed);

            scaleFactor = Mathf.Clamp(scaleFactor, minZoom, maxZoom);

            rectTransform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (activeTouchCount == 1)
        {
            activeTouchCount = 0;
        }
    }

    private void ResetZoom()
    {
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition = Vector2.zero;
        scrollView.vertical = true;
        resetButton.gameObject.SetActive(false);
    }


}
