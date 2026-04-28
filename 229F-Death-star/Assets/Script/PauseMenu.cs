using UnityEngine;
using UnityEngine.InputSystem; // ใช้ New Input System
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameOver())
        {
            return;
        }

        // เช็กการกดปุ่ม ESC ด้วย New Input System
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameOver()) return;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // ระวังชื่อ Scene เมนูหลักต้องชื่อ "MainMenu" เป๊ะๆ นะครับ
    }
}