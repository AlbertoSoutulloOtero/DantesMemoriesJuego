using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoScript : MonoBehaviour
{
    [SerializeField] Vector2 velocidadMovimiento;

    private Material material;
    private Camera mainCamera;

    private Vector2 textureOffset;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No se pudo encontrar la cámara principal en la escena.");
        }
    }

    private void Update()
    {
        if (mainCamera != null)
        {
            // Obtenemos la posición de la cámara
            float cameraPositionX = mainCamera.transform.position.x;

            // Calculamos el desplazamiento del fondo basado en la posición de la cámara
            float parallax = cameraPositionX * 0.1f * velocidadMovimiento.x;
            float offsetY = mainCamera.transform.position.y * velocidadMovimiento.y;

            // Aplicamos el desplazamiento al material del fondo
            textureOffset.x = parallax;
            textureOffset.y = offsetY;
            material.mainTextureOffset = textureOffset;
        }
    }
}



