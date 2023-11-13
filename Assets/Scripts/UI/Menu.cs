using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour{
    public string menuName;
    [SerializeField] List<MenuButton> buttons;

    public void ToggleMenu(bool value){
        gameObject.SetActive(value);
    }
}
// class can be called to show or hide any children of the gameobject
// that this script is attatched to