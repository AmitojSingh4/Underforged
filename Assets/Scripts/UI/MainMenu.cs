using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour{
    void Update(){
        if(Mouse.current.leftButton.wasPressedThisFrame){
            
            RaycastHit hit;
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            if(Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit, 15)){ // furthest button is sqrt(59) away
                
                string buttonName = hit.collider.gameObject.name;
                //Debug.Log(buttonName);

                hit.collider.gameObject.GetComponent<MenuButton>()?.OnClick();
            }
        }
    }
}
// casts a ray cast from the position of the camera and returns the returned colliders
// gameobjects name. if the collider has the component MenuButton it also calls OnClick
// for that collider