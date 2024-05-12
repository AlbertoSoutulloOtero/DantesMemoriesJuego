using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamScript : MonoBehaviour
{
    public Transform playerPos;
    public float xpos;
    public float ypos;
    public float zpos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(playerPos.position.x + xpos, playerPos.position.y + ypos, zpos);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPos.position.x + xpos, playerPos.position.y + ypos, zpos);
    }
}
