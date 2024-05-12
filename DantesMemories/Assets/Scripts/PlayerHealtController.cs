using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] public float health;
    float maxHealth;

    bool isInmune;
    [SerializeField] float inmuneTime;
    [SerializeField] float knockbackForce;

    SpriteRenderer spriteRenderer;
    Color damageColor; // Representará el estado de invulnerabilidad

    Animator animator;

    [SerializeField] Image healthBar;

    public GameObject gameOver;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthBar.fillAmount = health / 100;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.GetComponent<EnemyController>();

        ProjectileController enemyProjectile = collision.GetComponent<ProjectileController>();

        if (enemy != null && !isInmune)
        {
            health -= enemy.attack;
            AudioManager.instance.PlayAudio(AudioManager.instance.Hitted);

            if (health < 0)
            {

                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(Inmunity());
                StartCoroutine(FlashSprite());
                ApplyKnockback(collision.transform);
            }
        }
        if (enemyProjectile != null && !isInmune)
        {
            health -= enemyProjectile.attack;
            AudioManager.instance.PlayAudio(AudioManager.instance.Hitted);

            if (health < 0)
            {

                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(Inmunity());
                StartCoroutine(FlashSprite());
                ApplyKnockback(collision.transform);
            }
        }
    }

    public void OnDestroy()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.Death);
        gameOver.SetActive(true);
    }

    IEnumerator Inmunity()
    {
        isInmune = true;
        animator.SetBool("isHitted", true);
        yield return new WaitForSeconds(inmuneTime);
        isInmune = false;
        animator.SetBool("isHitted", false);
    }

    IEnumerator FlashSprite()
    {
        float duration = inmuneTime;
        float timer = 0f;

        while (timer < duration)
        {
            // Cambiar el color del sprite al color de daño
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f); // Esperar un corto tiempo

            // Cambiar el color del sprite al color original
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f); // Esperar un corto tiempo

            timer += 0.2f; // Incrementar el temporizador
        }

        // Restablecer el color del sprite al color original al finalizar el parpadeo
        spriteRenderer.color = Color.white;
    }

    void ApplyKnockback(Transform enemyTransform)
    {
        // Calcular la dirección del knockback
        Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;

        // Aplicar la fuerza del knockback al Rigidbody del jugador
        GetComponent<Rigidbody2D>().velocity = knockbackDirection * knockbackForce;

        // Detener el knockback después de un tiempo
        StartCoroutine(StopKnockback());
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(inmuneTime); // Esperar antes de detener el knockback
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Detener el movimiento del jugador
    }

    
}
