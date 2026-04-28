using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float enemyEyeRange = 8f;
    public float shootRange = 5f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;

    [Header("Projectile")]
    public float heightMin = 2f;
    public float heightMax = 4f;

    private float nextFireTime = 0f;
    private bool isFacingRight = true;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("EnemyAI: หา Player ไม่เจอ! ลืมตั้ง Tag ให้ Player หรือเปล่า?");
        }
    }

    void Update()
    {
        if (player == null) return; 

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= enemyEyeRange && distanceToPlayer > shootRange)
        {
            ChasePlayer();

            if (anim != null) anim.SetBool("walk", true);
        }
        else if (distanceToPlayer <= shootRange)
        {
            LookAtPlayer();

            if (anim != null) anim.SetBool("walk", false);

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
        else
        {
            if (anim != null) anim.SetBool("walk", false);
        }
    }

    void ChasePlayer()
    {
        LookAtPlayer();
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void LookAtPlayer()
    {
        if (player.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    Vector2 CalculateArcVelocity(Vector2 start, Vector2 end, float height)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float displacementY = end.y - start.y;
        Vector2 displacementX = new Vector2(end.x - start.x, 0);

        float h = Mathf.Max(height, displacementY + 0.5f);

        float time = Mathf.Sqrt(2 * h / gravity) + Mathf.Sqrt(2 * Mathf.Abs(displacementY - h) / gravity);
        float velocityY = Mathf.Sqrt(2 * gravity * h);
        Vector2 velocityX = displacementX / time;

        return velocityX + Vector2.up * velocityY;
    }

    void Shoot()
    {
        if (anim != null) anim.SetTrigger("attack");

        if (bulletPrefab == null || firePoint == null) return;

        GameObject go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D goRb = go.GetComponent<Rigidbody2D>();

        float height = Random.Range(heightMin, heightMax);

        Vector2 velocity = CalculateArcVelocity(firePoint.position, player.position, height);

        float mass = goRb.mass;
        Vector2 forceToApply = mass * velocity;

        goRb.AddForce(forceToApply, ForceMode2D.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyEyeRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}