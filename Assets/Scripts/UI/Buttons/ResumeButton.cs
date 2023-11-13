
public class ResumeButton : MenuButton{
    public GameManager gameManager;

    public override void OnClick(){
        // unpause game
        gameManager.ResumeGame();
    }    
    
}