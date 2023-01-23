using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int levelID;
    public double baseScore = 100;
    private int sideID = 0;
    private float nextUpdate = 0f;
    public GameObject mainCamera;
    public static GameManager instance;
    public GameObject[] pauseObjects;
    public GameObject[] resumeObjects;
    public Image[] health;
    public Sprite heart;
    public Sprite lostHealth;
    public GameObject WinMenu;
    public GameObject DeathMenu;
    private bool isPaused = false;
    public AudioSource Music;
    public AudioClip VictoryJingle;
    public PlayerController player;
    private bool isFinished = false;

    private void Start()
    {
        Time.timeScale = 1;
        //Null check
        if (instance == null)
        {
            //Define instance for easy access later
            instance = this;
        } else
        {
            if (instance.isPaused)
            {
                //Game last ended while paused, delete instance as they left the match to go to main menu
                Destroy(instance);
                instance = this;
            }
            else
            //Check if we're on same level, no need to reset if we are, but if we are on a different level, we do need to reset
            if (levelID != instance.levelID)
            {
                //This is a different level, IF we have stuff to carry between levels, define here
                Destroy(instance);
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        isPaused = false;
        //Toggle menu to correct states
        if (pauseObjects.Length > 0)
        {
            foreach (GameObject obj in pauseObjects)
            {
                obj.SetActive(false);
            }
        }
        if (resumeObjects.Length > 0)
        {
            foreach (GameObject obj in resumeObjects)
            {
                obj.SetActive(true);
            }
        }

        nextUpdate = Time.time + 1f;

        WinMenu.SetActive(false);
        DeathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel") && !isFinished)
        {
            togglePause();
        }
        //Dont fire anything after this
        if (isPaused) { return; }
        /*-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
             * Below this line is paused when pause script is ran *
         *-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-*/
        if (Time.time > nextUpdate)
        {
            nextUpdate += 1;
            player.updateIFrames();
        }

        for(int i = 0; i < 3; i++)
        {
            health[i].sprite = lostHealth;
        }
        for(int i = 0; i < player.health; i++)
        {
            health[i].sprite = heart;
        }
    }

    public void finishLevel()
    {
        //Music.Stop();
        //Show win menu
        WinMenu.SetActive(true);
        isFinished = true;
        isPaused = false;
        togglePause();
        //Music.PlayOneShot(VictoryJingle);
        if (pauseObjects.Length > 0)
        {
            foreach (GameObject obj in pauseObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    public int getSideID()
    {
        return sideID;
    }
    public void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Death()
    {
        DeathMenu.SetActive(true);
    }

    public void switchScenes(string scene)
    {
        if (SceneManager.GetSceneByName(scene) == null) { Debug.LogError("Scene " + scene + "did not exist!"); return; }
        SceneManager.LoadScene(scene);
    }

    public void togglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f;
            //Music.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            //Music.UnPause();
        }
        isPaused = !isPaused;
        //Update all pause/unpause objects
        if(pauseObjects.Length > 0)
        {
            foreach(GameObject obj in pauseObjects)
            {
                obj.SetActive(isPaused);
            }
        }
        if (resumeObjects.Length > 0)
        {
            foreach (GameObject obj in resumeObjects)
            {
                obj.SetActive(!isPaused);
            }
        }
    }

    public int timerDecrment(int timer)
    {
        if (Time.time > nextUpdate)
        {
            return timer -= 1;
        }
        else return timer;
    }
}
