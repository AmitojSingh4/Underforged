using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayButton : MenuButton{
    // resume pause
    // [SerializeField] CameraAnimation cameraAnimation;
    // public override void OnClick(){
    //     cameraAnimation.PlayAnimation();
    //     // load level and camera transition
    // }
    [SerializeField] PlayableDirector timeline;
    [SerializeField] GameManager gameManager;
    [SerializeField] Menu menuToChangeTo;
    public override void OnClick(){
        if(gameManager.isPaused){
            ResumeButton resumeButton = gameObject.AddComponent<ResumeButton>();
            resumeButton.gameManager = gameManager;

            resumeButton.OnClick();

            Destroy(this);
        }
        else{
            // load second menu 
            Destroy(gameObject.GetComponent<ResumeButton>());
            transform.parent.GetComponent<Menu>().ToggleMenu(false);
            menuToChangeTo.ToggleMenu(true);
        }

    }    
    
}