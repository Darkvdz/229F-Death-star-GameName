using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 15f;

    [Header("Combat Settings")]
    public Transform attackPoint; // จุดศูนย์กลางวงกลมโจมตี
    public float attackRange = 0.5f; // รัศมีวงกลมโจมตี
    public LayerMask enemyLayers; // ตัวกรองว่าอะไรคือศัตรู
    public int attackDamage = 40; // ดาเมจต่อการฟัน 1 ครั้ง

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private Animator anim;

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private int jumpCount;
    private bool isGrounded = false;
    public bool onIce = false;

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");

        rb = GetComponent<Rigidbody2D>();

        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        var moveInput = moveAction.ReadValue<Vector2>();

        if (!onIce)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(moveInput.x));
        }

        if (moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            Flip();

        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            Flip();
        }


        if (jumpAction.WasPressedThisFrame() && jumpCount < 2)
        {
            jumpCount++;

            isGrounded = false;
            if (anim != null) anim.SetBool("isGrounded", isGrounded);

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);


        }

        if (attackAction.WasPressedThisFrame())
        {
            Attack();
        }
    }


    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Attack()
    {
        // 1. เล่นแอนิเมชันฟันดาบ
        if (anim != null) anim.SetTrigger("Attack");

        // 2. เช็ควงกลมหาศัตรูแค่ "ตัวแรกที่เจอ" (ไม่มีคำว่า All แล้ว)
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);

        // 3. ถ้าฟันโดนศัตรู (hitEnemy ไม่เป็นความว่างเปล่า) ให้ลดเลือดตัวนั้นแค่ตัวเดียว
        if (hitEnemy != null)
        {
            hitEnemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;

            isGrounded = true;
            if (anim != null) anim.SetBool("isGrounded", isGrounded);

        }

        if (collision.gameObject.CompareTag("IcePlatform"))
        {
            onIce = true; // บอกสคริปต์ว่ายืนบนน้ำแข็งแล้วนะ (หยุดโค้ดเดิน ปล่อยแท่นผลัก)
        }
    }

   

    // พอเท้าหลุดออกจากแท่นน้ำแข็ง (กระโดด หรือ ตกขอบ)
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("IcePlatform"))
        {
            onIce = false; // กลับมาเดินปกติได้
        }
    }




}

