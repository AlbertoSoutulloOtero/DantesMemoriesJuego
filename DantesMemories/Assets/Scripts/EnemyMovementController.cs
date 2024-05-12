using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    float speed;
    Rigidbody2D rb;
    [SerializeField] bool isRigid;
    [SerializeField] bool isWalking;
    [SerializeField] bool lookRight; // Nuevo booleano para controlar la dirección inicial del enemigo

    Animator animator;

    [SerializeField] Transform wallCheck;
    [SerializeField] bool isWalled;

    [SerializeField] Transform pitCheck;
    [SerializeField] bool isPitted;

    [SerializeField] Transform groundCheck;
    [SerializeField] bool isGrounded;

    [SerializeField] float radius; //radio de detección de los límites.

    [SerializeField] LayerMask whatIsGround;

    bool isFlipped;

    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<EnemyController>().speed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Establecer la dirección inicial del enemigo según el booleano lookRight
        if (!lookRight)
        {
            Flip();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isPitted = Physics2D.OverlapCircle(pitCheck.position, radius, whatIsGround);
        isWalled = Physics2D.OverlapCircle(wallCheck.position, radius, whatIsGround);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, whatIsGround);

        if ((!isPitted || isWalled) && isGrounded)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (isRigid)
        {
            animator.SetBool("isIdle", true);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (isWalking)
        {
            animator.SetBool("isIdle", false);
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (lookRight)
            {
                rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            }
        }
    }

    private void Flip()
    {
        lookRight = !lookRight;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}