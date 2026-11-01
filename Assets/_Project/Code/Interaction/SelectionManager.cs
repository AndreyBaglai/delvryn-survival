using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    [Header("Interaction UI")]
    [SerializeField] private GameObject interaction_Info_UI;
    [SerializeField] private TMP_Text interaction_text;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        if (interaction_Info_UI == null)
        {
            Debug.LogError("SelectionManager: Interaction UI is not assigned!");
            enabled = false;
            return;
        }

        if (interaction_text == null)
        {
            Debug.LogError("SelectionManager: Interaction Text is not assigned!");
            enabled = false;
            return;
        }

        interaction_Info_UI.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            InteractableObject interactable =
                hit.transform.GetComponentInParent<InteractableObject>();

            if (interactable != null)
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                return;
            }
        }

        interaction_Info_UI.SetActive(false);
    }
}