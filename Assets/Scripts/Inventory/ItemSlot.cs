using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler {
    // item data
    public string itemName;
    public int quanitiy;
    private Sprite itemSprite;
    private string itemDescription;
    public Sprite emptySprite;

    [SerializeField]
    private int maxStackSize;

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

    public int AddItem(string itemName, int quanitiy, Sprite itemSprite, string itemDescription) {

        if (isFull) {
            return quanitiy;
        }

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        this.itemDescription = itemDescription;

        // cehck if slot overfilling
        int totalItems = this.quanitiy + quanitiy;
        if (totalItems >= maxStackSize) {
            this.quanitiy = maxStackSize;
            isFull = true;
            quantityText.text = quanitiy.ToString();
            quantityText.gameObject.SetActive(true);
            return totalItems - maxStackSize;
        }
    
        this.quanitiy += quanitiy;
        quantityText.text = quanitiy.ToString();
        quantityText.gameObject.SetActive(true);
        return 0;
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
