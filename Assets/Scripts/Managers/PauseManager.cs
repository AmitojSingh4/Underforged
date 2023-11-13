using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour{
    GameManager gameManager;
    
    void Awake(){
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update(){
        if(Keyboard.current.escapeKey.isPressed && !gameManager.isPaused){
            gameManager.PauseGame();
        }
    }
}
// pauses game when Esc key is pressed