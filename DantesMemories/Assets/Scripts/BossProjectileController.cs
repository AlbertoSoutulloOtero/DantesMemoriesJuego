using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileController : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody2D rb;
    Vector2 moveDirection;
    PlayerScript player;

    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = GetComponent<EnemyController>().speed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = PlayerScript.instance;

        moveDirection = (player.transform.position - transform.position).normalized * moveSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);

        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject);
    }

}
