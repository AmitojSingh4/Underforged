using UnityEngine;

public class CompletedOrderConveyor : Conveyor{
    [SerializeField] OrderManager orderManager;

    new void Update(){   
        if(inventory[0] == null)
            // guard clause to check if the inventory is empty, this prevents errors in the next if statement
            return;
        
        if(inventory[0].transform.position == inventorySlotPositions[0].position + yOffset() && isConveyorOccupied == true){
            // fullfil order
            orderManager.FulfillOrder(inventory[0]);
            RemoveInventory();
        }
    }
}

// CompletedOrderConveyor is a class that inherits from Conveyor and is created
// to take in completed order items, it updates the score and deletes the item 
// in its inventory