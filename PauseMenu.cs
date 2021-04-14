using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool gamePaused = false;
    public GameObject pause;

    void Start() {
        Pause();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gamePaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    public void Resume(){
        pause.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause(){
        pause.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void MenuLoad(){
        Debug.Log("Loading menu");
    }

    public void QuitGame(){
        Debug.Log("Quitting");
    }
}
