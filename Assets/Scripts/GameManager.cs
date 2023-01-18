using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Camera[] cameras;
    public int levelID;
    private static int attemptNumber = 1;
    private int TimesSwitched = 0;
    public TextMeshProUGUI TimeUntilNextSwitch;
    public TextMeshProUGUI BaseScore;
    public TextMeshProUGUI SwapsScore;
    public TextMeshProUGUI TotalScore;
    public double baseScore = 100;
    public int baseTime = 30;
    public static int timeBetweenSwitches = 30;
    private int timeSinceLastSwitch = 0;
    private int timer = 0;
    private int totalTime = 0;
    private int sideID = 0;
    private float nextUpdate = 0f;
    public static GameManager instance;
    public GameObject[] pauseObjects;
    public GameObject[] resumeObjects;
    public GameObject[] goals;
    public Rigidbody2D[] playerHitboxes;
    public GameObject WinMenu;
    private bool isPaused = false;
    public AudioSource Music;
    public AudioClip VictoryJingle;

    private void Start()
    {
        Time.timeScale = 1;
        //Null check
        if (instance == null)
        {
            //Define instance for easy access later
            instance = this;
        }else
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

        //AttemptsText.text = "Attempts: " + attemptNumber;
        timer = baseTime;
        nextUpdate = Time.time + 1f;
        for (int i = 0; i < cameras.Length; i++)
        {
            if (i != 0)
            {
                cameras[i].gameObject.SetActive(false);
                playerHitboxes[i].constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            }
        }

        WinMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
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
            timer += 1;
            nextUpdate += 1;
            totalTime += 1;
            timeSinceLastSwitch += 1;
        }
        if (Input.GetButtonDown("Fire1") && timeSinceLastSwitch > 1)
        {
            timeSinceLastSwitch += timeBetweenSwitches - timeSinceLastSwitch;
        }

        if (timeSinceLastSwitch >= timeBetweenSwitches)
        {
            //Switch sides
            //Reset if at max side number
            if (sideID == (cameras.Length - 1))
            {
                sideID = 0;
                cameras[cameras.Length - 1].gameObject.SetActive(false);
                playerHitboxes[cameras.Length - 1].constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                cameras[0].gameObject.SetActive(true);
                playerHitboxes[0].constraints = RigidbodyConstraints2D.FreezeRotation;
                playerHitboxes[0].AddForce(new Vector2(0, 0.1f));
            }
            else
            {
                cameras[sideID].gameObject.SetActive(false);
                playerHitboxes[sideID].constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                sideID++;
                cameras[sideID].gameObject.SetActive(true);
                playerHitboxes[sideID].constraints = RigidbodyConstraints2D.FreezeRotation;
                playerHitboxes[sideID].AddForce(new Vector2(0, 0.1f));
            }
            timeSinceLastSwitch = 0;
            TimesSwitched += 1;
            //Restart music
            Music.Stop();
            Music.Play();
        }
        //Timer.text = "Time: " + timer;
        TimeUntilNextSwitch.text = "Switch: " + (timeBetweenSwitches - timeSinceLastSwitch);
        //Check if all players are on the goals
        if (allOnGoal())
        {
            //You win
            finishLevel();
        }
    }

    bool allOnGoal()
    {
        bool onGoal = true;
        foreach (GameObject goal in goals)
        {
            if(goal.GetComponent<GoalScript>() == null) { continue; }
            if (!goal.GetComponent<GoalScript>().getOnGoal())
            {
                onGoal = false;
            }
        }
        return onGoal;
    }

    void finishLevel()
    {
        Music.Stop();
        //Show win menu
        WinMenu.SetActive(true);
        togglePause();
        Music.PlayOneShot(VictoryJingle);
        if (pauseObjects.Length > 0)
        {
            foreach (GameObject obj in pauseObjects)
            {
                obj.SetActive(false);
            }
        }
        //Populate info
        BaseScore.text = "Base Score: " + baseScore;
        SwapsScore.text = "Swaps: " + TimesSwitched;
        TotalScore.text = "Score: " + Mathf.RoundToInt((float) (baseScore * 30/ (TimesSwitched*30+timeSinceLastSwitch)));
    }

    public int getSideID()
    {
        return sideID;
    }
    public void Reset()
    {
        attemptNumber += 1;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            Music.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            Music.UnPause();
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
}
