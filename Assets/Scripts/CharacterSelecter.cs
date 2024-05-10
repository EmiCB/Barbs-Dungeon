using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecter : MonoBehaviour {
    public GameObject[] characters;

    void Awake() {
        int selected = PlayerPrefs.GetInt("selectedCharacter");
        characters[selected].SetActive(true);
    }
}
