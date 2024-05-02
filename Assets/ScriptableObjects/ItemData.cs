using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatToChange {
    None,
    Health,
    Mana,
    Stamina
}

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject {
    public string itemName;
    public StatToChange statToChange;
    public int amountToChangeStat;

    public void UseItem() {
        PlayerController player = FindObjectOfType<PlayerController>();

        switch (statToChange) {
            case StatToChange.None:
                break;
            case StatToChange.Health:
                player.Heal(amountToChangeStat);
                break;
            case StatToChange.Mana:
                player.RecoverMana(amountToChangeStat);
                break;
            case StatToChange.Stamina:
                player.RecoverStamina(amountToChangeStat);
                break;
        }
    }
}
