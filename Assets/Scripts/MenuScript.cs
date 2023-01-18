using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public void SwitchOpenMenu(GameObject newMenu)
    {
        if (newMenu == null) { Debug.LogError("Object did not exist!"); return; }
        newMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void switchScene(string scene)
    {
        if(SceneManager.GetSceneByName(scene) == null) { Debug.LogError("Scene " + scene + "did not exist!"); return; }
        SceneManager.LoadScene(scene);
    }

    //Exit the game
    public void Exit()
    {
        Application.Quit(); 
    }
}
