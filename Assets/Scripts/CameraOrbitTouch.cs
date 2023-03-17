using UnityEngine;

public class CameraOrbitTouch : MonoBehaviour
{
    public float rotationSpeed = 0.5f;

    private Vector2 touchStart;
    private bool dragging;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                dragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && dragging)
            {
                Vector2 touchDelta = touch.position - touchStart;
                float angleX = touchDelta.x * rotationSpeed * Time.deltaTime;

                transform.Rotate(Vector3.up, angleX, Space.World);

                touchStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                dragging = false;
            }
        }
    }
}
