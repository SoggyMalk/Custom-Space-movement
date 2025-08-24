using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public Transform playerCamera;   // Assign your Camera here
    public float sensitivity = 200f;
    public float smoothTime = 0.05f; // Smaller = snappier, bigger = floaty

    private float xRotation;
    private Vector2 currentVelocity;
    private Vector2 currentRotation;
    private Vector2 targetRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        targetRotation.x += mouseX;
        targetRotation.y -= mouseY;
        targetRotation.y = Mathf.Clamp(targetRotation.y, -90f, 90f);

        currentRotation = Vector2.SmoothDamp(currentRotation, targetRotation, ref currentVelocity, smoothTime);

        // Apply rotation
        transform.rotation = Quaternion.Euler(0f, currentRotation.x, 0f);
        playerCamera.localRotation = Quaternion.Euler(currentRotation.y, 0f, 0f);
    }
}
