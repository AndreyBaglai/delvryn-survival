using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI;

    public List<string> inventoryItemList = new List<string>();

    public bool isOpen;

    // Category Buttons
    Button toolsBTN;

    // Craft Buttons
    Button craftAxeBTN;

    // Requirement Text
    Text AxeReq1, AxeReq2;

    // All Blueprints

    public static CraftingSystem Instance { get; set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start() {
      isOpen = false;

      toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
      toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

      // Axe
      AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
      AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

      craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
      craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(); });
    }

    void Update() {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.cKey.wasPressedThisFrame && !isOpen)
        {
            craftingScreenUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isOpen = true;
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isOpen = false;
        }
    }

    void OpenToolsCategory() {
      craftingScreenUI.SetActive(false);
      toolsScreenUI.SetActive(true);
    }

    void CraftAnyItem() {
      
    }
}
