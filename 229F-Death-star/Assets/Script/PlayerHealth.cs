using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthBar;

    public SpriteRenderer[] playerSprites;
    public Color damageColor = Color.red;

    private Color[] originalColors;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (playerSprites == null || playerSprites.Length == 0)
        {
            playerSprites = GetComponentsInChildren<SpriteRenderer>();
        }

        if (playerSprites.Length > 0)
        {
            originalColors = new Color[playerSprites.Length];
            for (int i = 0; i < playerSprites.Length; i++)
            {
                originalColors[i] = playerSprites[i].color;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        if (playerSprites != null && playerSprites.Length > 0)
        {
            for (int i = 0; i < playerSprites.Length; i++)
            {
                playerSprites[i].color = damageColor;
            }

            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < playerSprites.Length; i++)
            {
                playerSprites[i].color = originalColors[i];
            }
        }
    }

    void Die()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }

        gameObject.SetActive(false);
    }
}