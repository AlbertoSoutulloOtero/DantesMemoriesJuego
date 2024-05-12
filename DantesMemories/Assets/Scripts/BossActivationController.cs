using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivationController : MonoBehaviour
{
    public bool bossActivating;
    private BoxCollider2D bossCollider;

    public GameObject boss;

    public static BossActivationController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // Obtener el componente Collider2D adjunto al objeto
        bossCollider = GetComponent<BoxCollider2D>();

        boss.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetActive(true);

            BossSceneController.instance.BossActivation();

            bossActivating = true;

            AudioManager.instance.SwitchToBossMusic();

            StartCoroutine(BossEntrance());
        }
    }

    IEnumerator BossEntrance()
    {

        bossCollider.enabled = false;

        yield return new WaitForSeconds(3f);

        //Debug.Log("yasta");
        bossActivating = false;
    }
}

