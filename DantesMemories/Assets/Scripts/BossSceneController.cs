using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneController : MonoBehaviour
{

    public GameObject BossPanel;
    public GameObject BossDoor;

    public static BossSceneController instance;

    Vector3 startPosition;
    Vector3 endPosition;
    public float moveUp;
    public float moveDuration;

    private bool isMoving = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        BossPanel.SetActive(false);
        BossDoor.SetActive(false);

        startPosition = BossDoor.transform.position;
        endPosition = BossDoor.transform.position + Vector3.up * moveUp;

    }

    public void BossActivation()
    {
        BossPanel.SetActive(true);
        BossDoor.SetActive(true);
        StartCoroutine(MoveDoor());
    }

    IEnumerator MoveDoor()
    {
        if (!isMoving)
        {
            isMoving = true;

            float elapsedTime = 0f;
            while (elapsedTime < moveDuration)
            {
                BossDoor.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            BossDoor.transform.position = endPosition;
            isMoving = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
