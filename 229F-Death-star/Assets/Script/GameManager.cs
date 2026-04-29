using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("GameManager Setting")]
    public bool isGameOver;
    [SerializeField] private int currentScore = 0;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText; 
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;


    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip bgmSound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        Time.timeScale = 1f;
        isGameOver = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && bgmSound != null)
        {
            audioSource.clip = bgmSound;
            audioSource.loop = true;
            audioSource.volume = 0.06f;
            audioSource.Play();
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        if (isGameOver) return;
        currentScore += scoreToAdd;
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
    }

    public void GameOver()
    {
        EndGame(false);
    }

    public void GameWin()
    {
        EndGame(true);
    }

    void EndGame(bool isWin)
    {
        isGameOver = true;
        Time.timeScale = 0f; 

        if (audioSource != null) audioSource.Stop();

        if (isWin)
        {
            if (winPanel != null) winPanel.SetActive(true);
            if (audioSource != null && winSound != null) audioSource.PlayOneShot(winSound, 2.0f);
        }
        else
        {
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            if (audioSource != null && loseSound != null) audioSource.PlayOneShot(loseSound, 0.25f);

            if (finalScoreText != null)
            {
                finalScoreText.text = "Your Score: " + currentScore;
            }
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void OpenCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}