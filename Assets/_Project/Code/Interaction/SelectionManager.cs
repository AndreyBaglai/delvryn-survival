using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    [Header("Interaction UI")]
    [SerializeField] private GameObject interaction_Info_UI;
    [SerializeField] private TMP_Text interaction_text;

    private Camera mainCamera;

    public bool onTarget;

    public GameObject selectedObject;

    private void Start()
    {
        mainCamera = Camera.main;
        interaction_Info_UI.SetActive(false);
        onTarget = false;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
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

            if (interactable != null && interactable.playerInRange)
            {
                onTarget = true;

                selectedObject = interactable.gameObject;

                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
               
                return;
            }
        }

        interaction_Info_UI.SetActive(false);
         onTarget = false;
    }
}