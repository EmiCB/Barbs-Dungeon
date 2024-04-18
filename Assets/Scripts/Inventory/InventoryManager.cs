using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public GameObject inventoryMenu;
    private bool isMenuOpen;
    public ItemSlot[] itemSlots;

    void Start() {
        // TODO: fix to be automatically finding item slots?
        //itemSlots = GetComponentsInChildren<ItemSlot>();
        //Debug.Log(GetComponentsInChildren<ItemSlot>());
    }

    public void OnInventory() {
        if (inventoryMenu == null) { return; }

        if (isMenuOpen) {
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            isMenuOpen = false;
        } else {
            Time.timeScale = 0;
            inventoryMenu.SetActive(true);
            isMenuOpen = true;
        }
    }

    public int AddItem(string itemName, int quanitiy, Sprite itemSprite, string itemDescription) {
        // get next available slot
        for (int i = 0; i < itemSlots.Length; i++) {
            if (!itemSlots[i].isFull
                && itemSlots[i].itemName == itemName
                || itemSlots[i].quanitiy == 0) {

                int numItemsLeftOver = itemSlots[i].AddItem(itemName, quanitiy, itemSprite, itemDescription);

                if (numItemsLeftOver > 0) {
                    numItemsLeftOver = AddItem(itemName, numItemsLeftOver, itemSprite, itemDescription);
                }

                return numItemsLeftOver;
            }
        }
        return quanitiy;
    }

    public void OnDeselectAllSlots() {
        for (int i = 0; i < itemSlots.Length; i++) {
            itemSlots[i].selectedShader.SetActive(false);
            itemSlots[i].isSlotSelected = false;
        }
    }
}
