using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Rigidbody body;
    private float throwStrength = 5f;

    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = new Vector3(cursorPosition.x, transform.position.y, cursorPosition.z);
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }

    private void OnMouseUp()
    {
        body.velocity = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * throwStrength;
    }
}
