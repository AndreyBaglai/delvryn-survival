using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;
 
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

            if (!CraftingSystem.Instance.isOpen) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName) {
      whatSlotToEquip = FindNextEmptySlot();

      itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
      itemToAdd.transform.SetParent(whatSlotToEquip.transform);

      itemList.Add(itemName);

      Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

      TriggerPickupPop(itemName, sprite);

      ReCalculateList();
      CraftingSystem.Instance.RefreshNeededItemsToCraft();
    }

    void TriggerPickupPop(string itemName, Sprite itemSprite) {
      pickupAlert.SetActive(true);
      pickupName.text = itemName;
      pickupImage.sprite = itemSprite;
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

    public void RemoveItem(string nameToRemove, int amountToRemove) {
      int counter = amountToRemove;

      for (var i = slotList.Count - 1; i >= 0; i--) {
        if (slotList[i].transform.childCount > 0) {
          if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0) {
            Destroy(slotList[i].transform.GetChild(0).gameObject);
            counter -= 1;
          }
        }
      }

      ReCalculateList();
      CraftingSystem.Instance.RefreshNeededItemsToCraft();
    }

    public void ReCalculateList() {
      itemList.Clear();

      foreach (GameObject slot in slotList) {
        if (slot.transform.childCount > 0) {
          string name = slot.transform.GetChild(0).name;
          string str = "(Clone)";
          string result = name.Replace(str, "");

          itemList.Add(result);
        }
      }
    }
}