using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {
    // item data
    private string itemName;
    private int quanitiy;
    private Sprite itemSprite;
    private string itemDescription;
    public Sprite emptySprite;

    // item slot
    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    // selection
    public GameObject selectedShader;
    public bool isSlotSelected;
    [SerializeField]
    private InventoryManager inventoryManager;

    // item description
    public Image itemDescriptionImage;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    void Awake() {
        inventoryManager = FindObjectOfType<InventoryManager>();
        isFull = false;
        isSlotSelected = false;
    }

    public void AddItem(string itemName, int quanitiy, Sprite itemSprite, string itemDescription) {
        this.itemName = itemName;
        this.quanitiy = quanitiy;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

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
        // show selection in UI
        inventoryManager.OnDeselectAllSlots();
        selectedShader.SetActive(true);
        isSlotSelected = true;

        // update description panel
        itemNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;

        if (itemSprite == null) {
            itemDescriptionImage.sprite = emptySprite;
        }
    }

    private void OnRightClick() {

    }
}
