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
    public Transform attackPoint; // ®ÿ¥»Ÿπ¬Ï°≈“ß«ß°≈¡‚®¡µ’
    public float attackRange = 0.5f; // √—»¡’«ß°≈¡‚®¡µ’
    public LayerMask enemyLayers; // µ—«°√Õß«Ë“Õ–‰√§◊Õ»—µ√Ÿ
    public int attackDamage = 40; // ¥“‡¡®µËÕ°“√ø—π 1 §√—Èß

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private Animator anim;

    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private int jumpCount;
    private bool isGrounded = false;
    //public bool onIce = false;

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

        //if (!onIce)
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
        
        if (anim != null) anim.SetTrigger("Attack");
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayers);
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

        //if (collision.gameObject.CompareTag("IcePlatform"))
        //{
        //    onIce = true; 
        //}
    }

   
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("IcePlatform"))
    //    {
    //        onIce = false; 
    //    }
    //}




}

