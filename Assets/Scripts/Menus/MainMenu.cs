using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void OpenSettings() {
        Debug.Log("Not Implemented Yet: Open Settings");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
