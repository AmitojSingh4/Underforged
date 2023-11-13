using UnityEngine;

public class ExitButton : MenuButton{
    public override void OnClick(){
        Application.Quit();
    }
}