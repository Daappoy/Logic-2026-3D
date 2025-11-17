using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameManager gameManager;
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;
    public Transform playerBody;

    void Start()
    {  
       Cursor.lockState = CursorLockMode.Locked;        
    }
    void Update()
    {
        if (GameManager.currentState != GameManager.GameState.InGame)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
