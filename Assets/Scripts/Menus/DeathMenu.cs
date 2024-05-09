using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour {
    public void OnRestart() {
        Scene currentLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentLevel.name);
        Time.timeScale = 1.0f;
    }

    public void OnMainMenu() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }
}
