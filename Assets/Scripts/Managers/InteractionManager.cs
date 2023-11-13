using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour{
    [SerializeField] Inventory playerInventory;
    [SerializeField] Player player;
    bool interacted = false;
    bool didPickup = false;

    void Start(){
        playerInventory = gameObject.GetComponent<Inventory>();
        player = gameObject.GetComponent<Player>();
    }
    // gets the components of the player and the players inventory

    public void OnInteract(InputValue value) => interacted = true;
    public void OnPickup(InputValue value) => didPickup = true;

    void Update(){
        RaycastHit hit;
        if(Physics.Raycast(playerInventory.transform.position, playerInventory.transform.forward, out hit, 1.0f) == false){
            return;
        }
        // raycast infront of player to look for gameobjects with an inventory

        GameObject gameObjectHit = hit.collider.gameObject;

        // pickup/place
        if(didPickup){
            //Debug.Log("E");
            Inventory benchInventory = gameObjectHit.GetComponent<Inventory>();

            if(benchInventory == null)  
                return;

            benchInventory.PickupAndPlace(playerInventory, benchInventory);
            didPickup = false;
        }

        // intteraction
        if(interacted){
            //Debug.Log("Q");
            Inventory interactableBench = hit.collider.GetComponent<Inventory>();
            interactableBench.Interact();
            interacted = false;
        }
    }
}