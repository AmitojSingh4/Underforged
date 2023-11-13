using UnityEngine;

class DontDestroy : MonoBehaviour{
    void Start(){
        DontDestroyOnLoad(this.gameObject);
    }
}
// stops the gameobject that has this script from being destroyed when
// a new scene is loaded