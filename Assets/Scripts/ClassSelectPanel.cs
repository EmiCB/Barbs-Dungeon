using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectPanel : MonoBehaviour {
    public int classNumber;
    public Image highlightPanel;
    public CharacterSelectMenu characterSelectMenu;

    public void SelectCharacter() {
        characterSelectMenu.OnDeselectAll();
        highlightPanel.enabled = true;
        characterSelectMenu.selectedCharacter = classNumber;
    }

    public void DeselectCharacter() {
        highlightPanel.enabled = false;
    }
}
