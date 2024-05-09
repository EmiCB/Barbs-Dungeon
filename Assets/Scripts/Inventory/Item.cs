using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] private string itemName;
    [SerializeField] private int quantity;
    [SerializeField] private Sprite sprite;
    [TextArea] [SerializeField] private string itemDescription;

    private InventoryManager inventoryManager;

    [SerializeField] private AudioClip collectClip;

    // Start is called before the first frame update
    void Start() {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            int numItemsLeftOver = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);

            if (numItemsLeftOver != quantity) {
                SoundFXManager.instance.PlaySoundFXClip(collectClip, transform, 1.0f);
            }

            // remove object if no more items left over
            if (numItemsLeftOver <= 0) {
                Destroy(gameObject); // TODO: object pool common items
            }
            else {
                quantity = numItemsLeftOver;
            }
        }
    }
}
