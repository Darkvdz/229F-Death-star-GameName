using UnityEngine;
using System.Collections;

public class CrumblingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float crumbleTime = 2f; // เวลาทั้งหมดก่อนพัง
    private bool isSteppedOn = false;

    private SpriteRenderer sr;
    private Color originalColor; // ตัวแปรเก็บสีเดิมของแท่น

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // จำสีเดิมของแท่นไว้ก่อน เผื่อแท่นหินสีเทา แท่นน้ำแข็งสีฟ้า จะได้กลับไปสีเดิมถูก
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isSteppedOn)
        {
            isSteppedOn = true;
            StartCoroutine(BlinkAndDestroyRoutine());
        }
    }

    // ฟังก์ชันระเบิดเวลาแบบกระพริบ
    IEnumerator BlinkAndDestroyRoutine()
    {
        float timer = 0f;
        bool toggleColor = false;

        // ลูปนี้จะทำงานไปเรื่อยๆ จนกว่าเวลา timer จะเกิน crumbleTime
        while (timer < crumbleTime)
        {
            // สลับสีไปมา (ถ้า toggleColor เป็นจริงให้สีแดง ถ้าเป็นเท็จให้สีเดิม)
            if (sr != null)
            {
                sr.color = toggleColor ? new Color(1f, 0.5f, 0.5f) : originalColor;
            }
            toggleColor = !toggleColor; // สลับค่า true/false

            // --- สูตรคำนวณความเร็วกระพริบ ---
            // ช่วงแรกจะกระพริบช้า (รอ 0.3 วินาที) ตอนใกล้แตกจะกระพริบถี่รัวๆ (รอ 0.05 วินาที)
            float waitTime = Mathf.Lerp(0.3f, 0.05f, timer / crumbleTime);

            yield return new WaitForSeconds(waitTime); // สั่งให้ระบบรอชั่วคราว

            timer += waitTime; // บวกเวลาที่ผ่านไปเข้ากับ timer
        }

        // เวลาหมด ลบแท่นทิ้งเลย
        Destroy(gameObject);
    }
}