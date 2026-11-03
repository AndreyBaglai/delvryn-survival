
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private float YRotation = 0f;

    void Start()
    {
        // Lock cursor to the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen) {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
            float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

            // Control rotation around X axis
            xRotation -= mouseY;

            // Clamp vertical rotation
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Control rotation around Y axis
            YRotation += mouseX;

            // Apply rotation
            transform.localRotation = Quaternion.Euler(
                xRotation,
                YRotation,
                0f
            );
        }  
    }
}