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

    public void AddItem(string itemName, int quanitiy, Sprite itemSprite) {
        // get next available slot
        for (int i = 0; i < itemSlots.Length; i++) {
            if (!itemSlots[i].isFull) {
                itemSlots[i].AddItem(itemName, quanitiy, itemSprite);
                return;
            }
        }
    }
}
