using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    EnemyController enemy;
    [SerializeField] public GameObject deathAnim;
    Color damageColor; // Representar� el estado de invulnerabilidad

    SpriteRenderer spriteRenderer;
    bool canTakeDamage = true;
    [SerializeField] float inmuneTime;

    [SerializeField] float knockbackForce;

    BossController bossController;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossController = GetComponent<BossController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTakeDamage && collision.CompareTag("Weapon")) // Verificar si el enemigo puede recibir da�o
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.EnemyHitted);
            enemy.healt -= enemy.damage;
            canTakeDamage = false; // Desactivar la capacidad de recibir da�o

            if (enemy.healt <= 0)
            {
                if(enemy.enemyName == "BlueFireWizard")
                {
                    bossController.bossHealthBarr.fillAmount = 0;
                }

                Destroy(gameObject);

                Instantiate(deathAnim, transform.position, Quaternion.identity);
            }

            StartCoroutine(ResetDamage()); // Restablecer el umbral de da�o despu�s de un tiempo
            StartCoroutine(FlashSprite()); // Hacer parpadear el sprite
            ApplyKnockback(collision.transform); // Aplicar knockback
        }
    }

    IEnumerator ResetDamage()
    {
        yield return new WaitForSeconds(inmuneTime); // Esperar medio segundo antes de restablecer la capacidad de recibir da�o
        canTakeDamage = true; // Restablecer la capacidad de recibir da�o
    }

    IEnumerator FlashSprite()
    {
        float duration = inmuneTime; // Duraci�n del parpadeo
        float timer = 0f;

        while (timer < duration)
        {
            // Cambiar el color del sprite al color de da�o
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
    void ApplyKnockback(Transform playerTransform)
    {
        // Calcular la direcci�n del knockback
        Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized;

        // Aplicar la fuerza del knockback al Rigidbody del enemigo
        GetComponent<Rigidbody2D>().velocity = knockbackDirection * knockbackForce;

        // Detener el knockback despu�s de un tiempo
        StartCoroutine(StopKnockback());
    }

    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(inmuneTime); // Esperar antes de detener el knockback
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Detener el movimiento del enemigo
    }
}


