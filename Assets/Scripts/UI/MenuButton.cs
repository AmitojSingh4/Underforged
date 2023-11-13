using UnityEngine;

public abstract class MenuButton : MonoBehaviour{
    public virtual void OnClick(){
        Debug.Log(gameObject.name + ' ' + "was clicked");
    }
}
// parent class for menu buttons