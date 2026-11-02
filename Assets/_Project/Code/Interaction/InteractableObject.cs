using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class InteractableObject : MonoBehaviour
{
    [Header("Item Settings")]
    public string ItemName;

    public bool playerInRange;
 
    public string GetItemName()
    {
        return ItemName;
    }

    void Update() {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame && playerInRange && SelectionManager.Instance.onTarget) {
            if (!InventorySystem.Instance.CheckIfFull()) {
                InventorySystem.Instance.AddToInventory(ItemName);
                Destroy(gameObject);
            } else {
                Debug.Log("the inventory is full");
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
        }
    }
}