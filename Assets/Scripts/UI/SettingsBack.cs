using UnityEngine;

public class SettingsBack : MonoBehaviour{   
    [SerializeField] Canvas canvas;
    [SerializeField] Menu menu;
    public void Click(){
        canvas.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
    }
}
// shows settings menu and hides current menu
