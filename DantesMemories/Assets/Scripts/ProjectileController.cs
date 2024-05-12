using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f; // Velocidad del proyectil
    public float attack = 20f; // Da�o causado por el proyectil

    // M�todo para inicializar el movimiento del proyectil
    void Start()
    {
        if (transform.parent != null)
        {
            // Asignamos la velocidad inicial al proyectil
            Vector2 direccionLanzamiento = transform.parent.localScale.x > 0 ? Vector2.left : Vector2.right;

            // Asignamos la velocidad inicial al proyectil en la direcci�n de lanzamiento
            GetComponent<Rigidbody2D>().velocity = direccionLanzamiento * speed;
        }
    }

    // M�todo que se ejecuta cuando el proyectil colisiona con otro objeto
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto con el que colisionamos tiene el tag "Player"
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            // Destruimos el proyectil al colisionar con el jugador
            Destroy(gameObject);

            // Aqu� podr�amos realizar otras acciones relacionadas con la colisi�n si es necesario
        }
    }

    // M�todo que se ejecuta cuando el proyectil sale del �rea de juego
    void OnBecameInvisible()
    {
        // Destruimos el proyectil si sale del �rea de juego
        Destroy(gameObject);
    }
}