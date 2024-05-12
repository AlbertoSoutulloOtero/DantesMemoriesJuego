using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer sfx;
    public AudioMixer music;

    public AudioSource Hit, Jump, EnemyHitted, Hitted, Money, Heal, Throw, ThrowBoss, DeathBoss, TPBoss, Pause, Unpause, Death, BackMusic, BackBossMusic;

    public static AudioManager instance;

    public GameObject boss;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayAudio(BackMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(boss == null)
        {
            BackBossMusic.Stop();
        }
        StartCoroutine(CheckEndSound());

    }

    IEnumerator CheckEndSound()
    {
        yield return new WaitForSeconds(1.5f);

        if (boss == null)
        {
            Throw.volume = 0f;
        }
        if (boss != null)
        {
            Throw.volume = 1f;
        }
    }

    public void PlayAudio(AudioSource audio)
    {

        audio.Play();

    }

    public void SwitchToBossMusic()
    {
        
        BackMusic.Stop();

        
        BackBossMusic.Play();
    }

}
