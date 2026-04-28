using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuBGM;

    private void Start()
    {
        creditsPanel.SetActive(false);
        
        audioSource.clip = menuBGM;
        audioSource.loop = true;  
        audioSource.volume = 0.1f;
        audioSource.Play();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!"); 
        Application.Quit();
    }
    
    public void OpenCredits()
    {
        creditsPanel.SetActive(true); 
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
}
