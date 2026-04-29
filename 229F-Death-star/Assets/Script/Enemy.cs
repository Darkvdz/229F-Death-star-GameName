using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int scoreValue = 10;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy was Attacked! Current HP: " + currentHealth);

      
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Die");

        if (GameManager.instance != null)
        {
            GameManager.instance.UpdateScore(scoreValue);
        }

        Destroy(gameObject);
    }
}