using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public float speed = 10f; // Velocidad de movimiento del jugador
    [SerializeField] public float jumpForce = 8f; // Fuerza de salto del jugador
    private Rigidbody2D rb; // Referencia al Rigidbody2D del jugador
    public bool isGrounded; // Booleano para verificar si el jugador está en el suelo
    private bool canJump = true; // Booleano para verificar si el jugador puede saltar
    private bool facingRight = true; // Booleano para verificar la dirección en la que está mirando el jugador
    private Animator animator; // Referencia al Animator
    private CapsuleCollider2D cc; // Referencia al collider del personaje
    private CircleCollider2D cc2;

    private bool canAttack = true;

    public static PlayerScript instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtenemos la referencia al Rigidbody2D
        animator = GetComponent<Animator>(); // Obtener la referencia al Animator
        cc = GetComponent<CapsuleCollider2D>(); // Obtener la referencia al CapsuleCollider2D
        cc2 = GetComponent<CircleCollider2D>(); // Obtener la referencia al CircleCollider2D
        cc2.enabled = false; // Desactivar el CircleCollider2D por defecto
    }

    void FixedUpdate()
    {
        // Verificar si el jugador está en el suelo
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground")); // Ajustamos para que el Raycast colisione solo con el Layer "Ground"

        // Si el jugador no está en el suelo y está cayendo, activar la animación de salto
        if (!isGrounded && rb.velocity.y < -1 && rb.velocity.x == 0)
        {
            animator.SetBool("isJumping", true);
        }
    }


    void Update()
    {
        if(!BossActivationController.instance.bossActivating)
        {
            Movement();
            UpdateAnimator();
            Attack();
        }
        if (BossActivationController.instance.bossActivating)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isWalking", false);
        }
    }

    private void Attack()
    {
        if (canAttack && isGrounded && Input.GetMouseButtonDown(0) && !animator.GetBool("isHitted"))
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.Hit);
            StartCoroutine(AttackCooldown());
            animator.SetTrigger("attack");
            animator.Play("HitAnim");

            // Idea: resetear de alguna forma el transform para desbuguear el arma.

        }

        
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    private void Movement()
    {
        // Movimiento horizontal
        float moveInput = Input.GetAxis("Horizontal");

        // Verificar si el jugador está en el suelo
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0f);

        // verificar que no está atacando
        if (canAttack)
        {

        // Agacharse si está en el suelo y presiona el botón de control izquierdo
        if (isGrounded)
        {
            bool isCrouding = animator.GetBool("isCrouding");

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                animator.SetBool("isCrouding", true);
                rb.velocity = new Vector2(0f, rb.velocity.y); // Parar el movimiento lateral
                moveInput = 0; // Frenar en seco al agacharse
                cc.enabled = false; // Desactivar el CapsuleCollider2D
                cc2.enabled = true; // Activar el CircleCollider2D
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                animator.SetBool("isCrouding", false);
                cc.enabled = true; // Activar el CapsuleCollider2D
                cc2.enabled = false; // Desactivar el CircleCollider2D
            }

            if (!isCrouding)
            {
                // Movimiento horizontal solo si no está agachado
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

                // Saltar si está en el suelo, puede saltar y se presiona la tecla de salto (Espacio)
                if (Input.GetKeyDown(KeyCode.Space) && canJump)
                {
                    AudioManager.instance.PlayAudio(AudioManager.instance.Jump);
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                    canJump = false; // El jugador ya no puede saltar hasta que toque el suelo nuevamente
                    animator.SetBool("isJumping", true); // Activar la animación de salto
                    moveInput = rb.velocity.x; // Conservar la dirección y velocidad durante el salto
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Ignorar la entrada de control mientras está en el aire
            return;
        }

        // Invertir el sprite si el jugador se mueve hacia la izquierda
        if (moveInput < 0 && facingRight && !animator.GetBool("isCrouding"))
        {
            Flip();
        }
        // Invertir el sprite si el jugador se mueve hacia la derecha
        else if (moveInput > 0 && !facingRight && !animator.GetBool("isCrouding"))
        {
            Flip();
        }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y); // Parar el movimiento lateral
            moveInput = 0;
        }
    }




    // Verificar si el jugador toca el suelo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Establecer isGrounded a verdadero cuando el jugador toca el suelo
            canJump = true; // El jugador puede saltar nuevamente
            animator.SetBool("isJumping", false); // Desactivar la animación de salto
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Establecer isGrounded a falso cuando el jugador deja de tocar el suelo
        }
    }

    // Método para invertir el sprite del jugador
    private void Flip()
    {
        facingRight = !facingRight; // Cambiar la dirección en la que está mirando el jugador

        // Obtener la escala actual del jugador
        Vector3 scale = transform.localScale;
        // Invertir la escala en el eje X para voltear el sprite
        scale.x *= -1;
        // Aplicar la nueva escala al jugador
        transform.localScale = scale;
    }

    private void UpdateAnimator()
    {
        // Actualizar el parámetro isWalking del Animator
        bool isWalking = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
        animator.SetBool("isWalking", isWalking);
    }
}
