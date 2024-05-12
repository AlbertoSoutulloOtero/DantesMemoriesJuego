using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{

    [SerializeField] float healtAdded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.Heal);
            collision.GetComponent<PlayerHealthController>().health += healtAdded;
            Destroy(gameObject);
        }
    }
}
