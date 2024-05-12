using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{

    public Transform[] transforms;

    public GameObject shoot;

    public float timeToShoot;
    public float coolDown;
    public float timeToPosition;
    public float coolDownPosition;

    private int previousPositionIndex;

    public float bossHealth;
    public float actualHealth;

    public Image bossHealthBarr;

    // Start is called before the first frame update
    void Start()
    {

        var firstPosition = UnityEngine.Random.Range(0, transforms.Length);
        previousPositionIndex = firstPosition;


        if (firstPosition == 0 || firstPosition == 2)
        {
            FlipSprite();
        }


        transform.position = transforms[firstPosition].position;
        coolDown = timeToShoot;
        coolDownPosition = timeToPosition;

        actualHealth = GetComponent<EnemyController>().healt;
        bossHealth = actualHealth;

    }

    // Update is called once per frame
    void Update()
    {

        coolDown -= Time.deltaTime;
        coolDownPosition -= Time.deltaTime;


        if (coolDown < 0)
        {
            Shoot();
            coolDown = timeToShoot;
        }


        if (coolDownPosition <= 0f)
        {
            coolDownPosition = timeToPosition;
            ChangePosition();
        }

        actualHealth = GetComponent<EnemyController>().healt;
        bossHealthBarr.fillAmount = actualHealth / bossHealth;

       
        if(actualHealth <= 0f)
        {
            bossHealthBarr.fillAmount = 0f;
        }

    }

    public void Shoot()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.ThrowBoss);
        GameObject cast = Instantiate(shoot, transform.position, Quaternion.identity);
    }

    public void ChangePosition()
    {
        int newPositionIndex;


        do
        {
            newPositionIndex = UnityEngine.Random.Range(0, transforms.Length);
        }
        while (newPositionIndex == previousPositionIndex);


        if ((newPositionIndex % 2 == 0 && previousPositionIndex % 2 != 0) ||
            (newPositionIndex % 2 != 0 && previousPositionIndex % 2 == 0))
        {

            FlipSprite();
        }


        previousPositionIndex = newPositionIndex;
        transform.position = transforms[newPositionIndex].position;
        AudioManager.instance.PlayAudio(AudioManager.instance.TPBoss);
    }



    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDestroy()
    {
        AudioManager.instance.PlayAudio(AudioManager.instance.DeathBoss);
        BossSceneController.instance.BossDoor.SetActive(false);
        BossSceneController.instance.BossPanel.SetActive(false);
    }

}