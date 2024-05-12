using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public bool isPaused = false;

    public GameObject pauseMenu;

    public GameObject gameOver;

    public GameObject startMenu;

    public GameObject boss;

    public GameObject endScreen;


    private void Start()
    {
        startMenu.SetActive(true);
        Time.timeScale = 0f;

        isPaused = false;
        pauseMenu.SetActive(false);
        
    }

    void Update()
    {
        

        if (Input.anyKeyDown && startMenu.activeSelf)
        {
            startMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        if (!endScreen.activeSelf)
        {

            if (!gameOver.activeSelf && !startMenu.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    TogglePause();
                }
            }

        }


                if (isPaused)
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        RestartScene();
                    }

                    if (Input.GetKeyDown(KeyCode.L))
                    {
                        QuitGame();
                    }
                }

        if(boss == null)
        {
            StartCoroutine(EndGame());
        }

    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.5f);

        endScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.Pause);
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            AudioManager.instance.PlayAudio(AudioManager.instance.Unpause);
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void QuitGame()
    {
        Application.Quit();
    }

}
