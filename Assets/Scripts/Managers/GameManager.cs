using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline; 
    [SerializeField] Menu pauseMenu;
    [SerializeField] Menu currentMenu;
    [SerializeField] Dropper dropper1;
    [SerializeField] Dropper dropper2;
    [SerializeField] Dropper dropper3;

    public bool isPaused = false;

    public void PauseGame(){
        //switch to pause menu items

        currentMenu.ToggleMenu(true);
        pauseMenu.ToggleMenu(false);


        isPaused = true;
        Time.timeScale = 0;
        StartCoroutine(TimelineReverse.Reverse(timeline, () => {}));
    }

    public void ResumeGame(){
        isPaused = false;
        Time.timeScale = 1;
        timeline.Play();
    }

    public void OnLevelEnd(int score){
        Debug.Log($"Score: {score}");

        // sends score to webserver
        StartCoroutine(ApiManager.SetScore(score, () => {
            // go back to main menu
            StartCoroutine(TimelineReverse.Reverse(timeline, () => {
                SceneManager.UnloadSceneAsync("Level1");
            }));
        }));
    }

    public void ToggleDroppers(bool toggle){
        dropper1.isDropperActive = toggle;
        dropper2.isDropperActive = toggle;
        dropper3.isDropperActive = toggle;
    }
    // used to disable droppers in main menus background to stop
    // unnecessary calculations in the background
}
