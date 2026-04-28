using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 20;

    // เพิ่มฟังก์ชัน Start เพื่อจับเวลาทำลายตัวเอง
    void Start()
    {
        // สั่งให้กระสุนสลายตัวไปเองใน 5 วินาที (ป้องกันกระสุนลอยออกนอกจอแล้วกินเมมโมรี่เครื่อง)
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}