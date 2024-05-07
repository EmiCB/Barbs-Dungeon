using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenu;
    private bool isMenuOpen;

    public void OnPause() {
        if (pauseMenu == null) { return; }

        if (isMenuOpen) {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            isMenuOpen = false;
        } else {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            isMenuOpen = true;
        }
    }

    public void OnResume() {
        if (isMenuOpen) {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            isMenuOpen = false;
        }
    }

    public void OnMainMenu() {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}
