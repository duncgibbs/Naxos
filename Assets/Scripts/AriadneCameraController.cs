using UnityEngine;

public class AriadneCameraController : MonoBehaviour
{
    public float Y_ANGLE_MIN = -50.0f;
    public float Y_ANGLE_MAX = 50.0f;
    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    public float distance = 2f;
    public float height = 1f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensivityX = 4.0f;
    public float sensivityY = 1.0f;

    private void start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, height, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 ariadnePosition = lookAt.position;
        // ariadnePosition.y += height;
        camTransform.position = ariadnePosition + rotation * dir;

        camTransform.LookAt(lookAt.position + new Vector3(0, height, 0));
    }
}