using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("GameManager Setting")]
    public bool isGameOver;
    // ถ้าอยากให้คะแนนคือ "ความสูง" ที่กระโดดได้ เดี๋ยวเราค่อยมาเขียนระบบอัปเดตคะแนนตามแกน Y ของผู้เล่นทีหลังครับ
    [SerializeField] private int currentScore = 0;

    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI scoreText; // เปลี่ยนจากเวลาเป็นโชว์คะแนน/ความสูง
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject creditsPanel;

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
        // ปิดหน้าต่าง UI ทั้งหมดตอนเริ่มเกม
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
            audioSource.volume = 0.08f;
            audioSource.Play();
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        if (isGameOver) return;
        currentScore += scoreToAdd;
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
    }

    // ฟังก์ชันนี้เอาไว้ให้ PlayerHealth เรียกใช้ตอนผู้เล่นตาย
    public void GameOver()
    {
        EndGame(false);
    }

    // ฟังก์ชันเอาไว้เรียกตอนเข้าเส้นชัย (ยอดหอคอย)
    public void GameWin()
    {
        EndGame(true);
    }

    void EndGame(bool isWin)
    {
        isGameOver = true;
        Time.timeScale = 0f; // หยุดเวลาในเกม

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
}