using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemSlot : MonoBehaviour {
    // item data
    private String itemName;
    private int quanitiy;
    private Sprite itemSprite;

    // item slot
    public bool isFull;
    public TMP_Text quantityText;
    public Image itemImage;

    void Awake() {
        //quantityText = GetComponentInChildren<TMP_Text>();
        //itemImage = GetComponentInChildren<Image>();
        isFull = false;
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
}
