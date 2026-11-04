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
    public Blueprint AxeBLP = new Blueprint("Axe", 2, "Stone", 3, "Stick", 3);

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
      craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
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

            if (!InventorySystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            isOpen = false;
        }
    }

    void OpenToolsCategory() {
      craftingScreenUI.SetActive(false);
      toolsScreenUI.SetActive(true);
    }

    void CraftAnyItem(Blueprint blueprintToCraft) {
      InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

      if (blueprintToCraft.numOfRequirements == 1) {
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
      } else if (blueprintToCraft.numOfRequirements == 2) {
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1Amount);
        InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2Amount);
      }

      StartCoroutine(Calculate());
    }

    public void RefreshNeededItemsToCraft() {
      int stone_count = 0;
      int stick_count = 0;

      inventoryItemList = InventorySystem.Instance.itemList;

      foreach (string itemName in inventoryItemList) {
        switch (itemName) {
          case "Stone":
            stone_count += 1;
            break;
          case "Stick":
            stick_count += 1;
            break;
        }
      }

      // *** AXE *** //
      AxeReq1.text = "3 Stone [" + stone_count + "]";
      AxeReq2.text = "3 Stick [" + stick_count + "]";

      if (stone_count >= 3 && stick_count >= 3) {
        craftAxeBTN.gameObject.SetActive(true);
      } else {
        craftAxeBTN.gameObject.SetActive(false);
      }
    }

    public IEnumerator Calculate() {
      yield return 0;
      InventorySystem.Instance.ReCalculateList();
      RefreshNeededItemsToCraft();
    }
}
