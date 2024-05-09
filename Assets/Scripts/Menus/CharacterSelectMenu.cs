using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectMenu : MonoBehaviour {
    
    public ClassSelectPanel[] characterPanels;
    public int selectedCharacter = int.MaxValue;

    void Start() {
        OnDeselectAll();
    }

    public void OnStartButton() {
        // disable button if no character selected
        if (selectedCharacter == int.MaxValue) {
            return;
        }
        // pick character for next scene
        // use playerprefs to keep track of selected class
        // 0 - knight, 1 - ranger, 3 - assassin, 4 - mage
        PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
        SceneManager.LoadScene("MovementPlayground");
    }

    public void OnBackButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnDeselectAll() {
        for (int i = 0; i < characterPanels.Length; i++) {
            characterPanels[i].DeselectCharacter();
        }
    }
}
