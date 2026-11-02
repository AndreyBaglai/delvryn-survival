using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
 
    public GameObject inventoryScreenUI;
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
    }
 
    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.iKey.wasPressedThisFrame && !isOpen)
        {
		      	Debug.Log("i is pressed");
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
}