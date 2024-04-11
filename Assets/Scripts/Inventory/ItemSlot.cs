using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {
    // item data
    private String itemName;
    private int quanitiy;
    private Sprite itemSprite;

    // item slot
    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    public GameObject selectedShader;
    public bool isSlotSelected;

    [SerializeField]
    private InventoryManager inventoryManager;

    void Awake() {
        //quantityText = GetComponentInChildren<TMP_Text>();
        //itemImage = GetComponentInChildren<Image>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        isFull = false;
        isSlotSelected = false;
    }

    public void AddItem(String itemName, int quanitiy, Sprite itemSprite) {
        Debug.Log(itemName + " " + quanitiy + " " + itemSprite);
        this.itemName = itemName;
        this.quanitiy = quanitiy;
        this.itemSprite = itemSprite;
        isFull = true;

        quantityText.text = quanitiy.ToString();
        quantityText.enabled = true;

        itemImage.sprite = itemSprite;
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        // left click
        if (eventData.button == PointerEventData.InputButton.Left) {
            OnLeftClick();
        }

        // right click
        if (eventData.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }

    private void OnLeftClick() {
        inventoryManager.OnDeselectAllSlots();
        selectedShader.SetActive(true);
        isSlotSelected = true;
    }

    private void OnRightClick() {

    }
}
