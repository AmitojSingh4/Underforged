using System.Collections;
using UnityEngine;

public class Dropper : MonoBehaviour{
    public float delayBetweenSpawns;
    public bool isDropperActive = false;
    int layerIndex;
    Conveyor conveyorUnderneath;
    [SerializeField] GameObject itemToSpawn;
    float _time = 0.0f; // named _time as time is already taken
    float waitTime = 0.2f;

    void Start(){
        conveyorUnderneath = FindConveyorUnderneath();
        layerIndex = LayerMask.NameToLayer("Objects");
        _time = delayBetweenSpawns;
    }
    // finds the conveyor underneath the dropper and 
    // sets the time equal to the time between spawns

    void Update(){
        if (_time > 0)
            _time -= Time.deltaTime;
        else{
            spawnItem();
            _time = delayBetweenSpawns;
        }
    }
    // checks if the time is less then 0
    // if it is not then it will decrease the tiem
    // if it is then it will spawn and item and reset the time

    void spawnItem(){
        if (conveyorUnderneath.inventory[0] == null && isDropperActive)
            StartCoroutine(dropItem());
    }
    // checks if the conveyors inventory is empty and that the dropper is active 
    // starts the coroutine to dispense the item if the if statement is true

    IEnumerator dropItem(){
        GameObject item = Instantiate(itemToSpawn, transform.position, transform.rotation);
        item.name = itemToSpawn.name;
        item.layer = layerIndex;
        // sets the spawned item to the item name 
        // (removes " (clone)" after the gameobject in the inspector and game)
        
        conveyorUnderneath.inventory[0] = item.GetComponent<Item>();
        float elapsedTime = 0.0f;

        Vector3 itemPosition = item.transform.position;
        Vector3 futurePosition = conveyorUnderneath.inventorySlotPositions[0].position + conveyorUnderneath.yOffset();

        while (elapsedTime < waitTime){
            item.transform.position = Vector3.Lerp(itemPosition, futurePosition, (elapsedTime / waitTime));
            // linearly interpolates between its current position to the future position 
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        item.transform.position = futurePosition;
        // used to make sure the item is in the correct position
        // deals with floating point inaccuracy

        yield break;
    }
    // coroutine that spawns and moves the item physically in the game and transfers the 
    // item into inventory of the conveyor under it

    Conveyor FindConveyorUnderneath(){
        Conveyor conveyorUnderneath = null;
        RaycastHit hit;
        Vector3 intialPositionToCastFrom = (transform.position + 0.45f * transform.right);

        if (Physics.Raycast(intialPositionToCastFrom, Vector3.down, out hit, 10)){
            if (hit.collider.gameObject.GetComponent<Conveyor>() != null)
                conveyorUnderneath = hit.collider.gameObject.GetComponent<Conveyor>();
        }

        return conveyorUnderneath;
    }
    // uses raycasts to find the conveyor under the dropper

    // Debugging
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawRay(transform.position + 0.45f * transform.right, 10.0f * Vector3.down);
    // }
    // draws a line down from the dropper to aid in making the raycast to find the
    // conveyor under it
}
