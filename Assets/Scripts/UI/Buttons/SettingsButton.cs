using UnityEngine;

public class SettingsButton : MenuButton{

    [SerializeField] Canvas settingsCanvas;
    
    public override void OnClick(){
        settingsCanvas.gameObject.SetActive(true); 
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}