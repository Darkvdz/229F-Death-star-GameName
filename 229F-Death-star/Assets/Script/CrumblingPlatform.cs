using UnityEngine;
using System.Collections;

public class CrumblingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float crumbleTime = 2f; 
    private bool isSteppedOn = false;

    private SpriteRenderer sr;
    private Color originalColor; 

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
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

    IEnumerator BlinkAndDestroyRoutine()
    {
        float timer = 0f;
        bool toggleColor = false;
        while (timer < crumbleTime)
        {
            {
                sr.color = toggleColor ? new Color(1f, 0.5f, 0.5f) : originalColor;
            }
            toggleColor = !toggleColor; 
            float waitTime = Mathf.Lerp(0.3f, 0.05f, timer / crumbleTime);

            yield return new WaitForSeconds(waitTime);
            timer += waitTime;
        }
        Destroy(gameObject);
    }
}