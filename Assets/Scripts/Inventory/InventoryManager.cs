using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public GameObject inventoryMenu;
    private bool isMenuOpen;

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
}
