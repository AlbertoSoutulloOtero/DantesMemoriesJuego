using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectyleController : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public float frecuenciaLanzamiento = 0.3f;
    public bool lanzarProyectiles = false;
    private float tiempoUltimoLanzamiento;

    private void Start()
    {
        tiempoUltimoLanzamiento = Time.time;
    }

    private void Update()
    {
        
        if (lanzarProyectiles && Time.time - tiempoUltimoLanzamiento > 1f / frecuenciaLanzamiento)
        {
            LanzarProyectil();
            tiempoUltimoLanzamiento = Time.time;
        }
    }

    private void LanzarProyectil()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.Throw);
        GameObject proyectil = Instantiate(proyectilPrefab, transform.position, Quaternion.identity, transform);
    }
}