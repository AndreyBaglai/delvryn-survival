using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList() {
      foreach (Transform child in inventoryScreenUI.transform) {
        if (child.CompareTag("Slot")) {
          slotList.Add(child.gameObject);
        }
      }
    }
 
    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.iKey.wasPressedThisFrame && !isOpen)
        {
            inventoryScreenUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isOpen = true;
        }
        else if (Keyboard.current.iKey.wasPressedThisFrame && isOpen)
        {
            inventoryScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName) {
      whatSlotToEquip = FindNextEmptySlot();

      itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
      itemToAdd.transform.SetParent(whatSlotToEquip.transform);

      itemList.Add(itemName);
    }

    public bool CheckIfFull() {
      int counter = 0;

      foreach(GameObject slot in slotList) {
        if (slot.transform.childCount > 0) {
          counter++;
        }
      }

      return counter == slotList.Count;
    }

    private GameObject FindNextEmptySlot() {
      foreach(GameObject slot in slotList) {
        if (slot.transform.childCount == 0) {
          return slot;
        }
      }

      return new GameObject();
    }
}