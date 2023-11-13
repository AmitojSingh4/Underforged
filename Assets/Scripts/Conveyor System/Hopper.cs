using UnityEngine;

public class Hopper : Conveyor{
    [SerializeField] Dropper dropper;
    bool dropperActivated = false;

    new void Start(){
        inventory[0] = null;
        isConveyorOccupied = false;
    }
    // sets its inventory to empty

    new void Update(){   
        if(inventory[0] == null)
            // guard clause to check if the inventory is empty, this prevents errors in the next if statement
            return;
        
        if(inventory[0].transform.position == inventorySlotPositions[0].position + yOffset() && inventory != null && isConveyorOccupied == true){
            RemoveInventory();

            if(dropper == null)
                // guard clause
                return;
            if(dropper.isDropperActive == false && !dropperActivated){
                dropper.isDropperActive = true;
                dropperActivated = true;
            }
        }
    }
    // makes sure item has arived before deleting to prevent errors with missing refrences 
    // enables linked dropper, this is only used in the main menu to make the background 
    // look nice
}
// Hopper inherits from Conveyor and only exists to be a bin in the levels and make the
// main menus background look nice