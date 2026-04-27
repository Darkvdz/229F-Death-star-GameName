using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
           
            Debug.Log("กระสุนโดนผู้เล่น!");
            Destroy(gameObject); 
        }
        
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}