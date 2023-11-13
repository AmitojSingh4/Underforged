using System.Collections;
using UnityEngine;

public class Conveyor : Inventory{
    public Conveyor conveyorInfront;
    public bool isConveyorOccupied;
    bool isLastConveyor;
    float waitTime = 0.5f;
    bool inCoroutine = false;

    new void Start(){
        base.Start();

        isLastConveyor = false;
        conveyorInfront = FindNextConveyor();
    }
    // tries to find the next conveyor along

    new void Update(){
        if (inventory[0] != null && conveyorInfront != null && !isLastConveyor && !conveyorInfront.isConveyorOccupied)
            StartCoroutine(StartBeltMove());
    }
    // checks if conveyor is full and starts the movement coroutine if it is

    public Vector3 yOffset(){
        return (inventory[0].gameObject.GetComponent<Item>().itemData.height / 2) * transform.up;
    }
    // calculates y coordinate of center the item on the conveyor

    protected void RemoveInventory(){
        Destroy(inventory[0].gameObject);
        inventory[0] = null;
        isConveyorOccupied = false;
    }
    // deletes the item in the conveyors inventory and resets its boolean values accordingly

    public override void Interact(){
        return;
    }

    public override void PickupAndPlace(Inventory playerInventory, Inventory _){
        if(playerInventory.inventory[0] != null && !isConveyorOccupied){
            // place
            PlaceItem(playerInventory.inventory[0]);
            playerInventory.inventory[0] = null;
            isConveyorOccupied = true;

            inventory[0].gameObject.transform.position = inventorySlotPositions[0].position + yOffset();
            inventory[0].gameObject.transform.SetParent(null);
            // moves items gameobject and unparents it from the conveyor
        }

        else if(playerInventory.inventory[0] == null && isConveyorOccupied && inventory[0] != null){
            // pickup
            playerInventory.inventory[0] = TakeItem();
            isConveyorOccupied = false;
            if (inCoroutine)
                conveyorInfront.isConveyorOccupied = false;
        }
    }
    // pickup and place method is adapted to handle conveyor logic

    private IEnumerator StartBeltMove(){
        inCoroutine = true;
        isConveyorOccupied = true;
        conveyorInfront.isConveyorOccupied = true;

        float elapsedTime = 0.0f;

        Vector3 itemPosition = inventorySlotPositions[0].position + yOffset();
        Vector3 futurePosition = conveyorInfront.inventorySlotPositions[0].position + yOffset();

        while (elapsedTime < waitTime){
            if (inventory[0] == null)
                yield break;
                
            inventory[0].transform.position = Vector3.Lerp(itemPosition, futurePosition, (elapsedTime / waitTime));
            // linearly interpolates between its current position to the future position 
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        inventory[0].transform.position = futurePosition;
        // used to make sure the item is in the correct position
        // deals with floating point inaccuracy

        conveyorInfront.inventory[0] = inventory[0];
        inventory[0] = null;
        isConveyorOccupied = false;
        inCoroutine = false;
        // transfers inventories
        yield break;
    }
    // coroutine that moves the item in its inventory physically in the game and transfers the 
    // item in its inventory to the next conveyor

    Conveyor FindNextConveyor(){
        Conveyor nextConveyor = null;
        RaycastHit hit;
        Vector3 intialPositionToCastFrom = transform.position + transform.forward + Vector3.up;

        if (Physics.Raycast(intialPositionToCastFrom, Vector3.down, out hit, 100)){ 
            // raycast length of 100, anything further is seen as unreasonable
            if (hit.collider.gameObject.GetComponent<Conveyor>() != null)
                // checks if the gameobject collided with has a conveyor script attached
                nextConveyor = hit.collider.gameObject.GetComponent<Conveyor>();
            else
                isLastConveyor = true;
        }
        return nextConveyor;
    }
    // finds the next conveyor by sending a raycast downwards infront of it

    // Debugging
    // void OnDrawGizmos(){
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawRay(transform.position, Vector3.up);
    // }
    // used for debugging to see where the conveyors inventory is physically located
}