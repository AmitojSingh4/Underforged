using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class LevelButton : MenuButton{
    [SerializeField] int levelNum;
    [SerializeField] PlayableDirector timeline;
    [SerializeField] GameManager gameManager;

    public override void OnClick(){
        //Debug.Log($"Load level {levelNum}");
        //load scene 
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        gameManager.ToggleDroppers(false);

        // camera transition
        timeline.Play();
    }
}