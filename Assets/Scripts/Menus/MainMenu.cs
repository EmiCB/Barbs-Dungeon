using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame() {
        Debug.Log("Not Implemented Yet: Play Game");
        SceneManager.LoadScene("MovementPlayground");
    }

    public void OpenSettings() {
        Debug.Log("Not Implemented Yet: Open Settings");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
