using UnityEngine;

public abstract class Inventory : MonoBehaviour{
    [Header("Inventory")]
    public int numberOfInventorySlots;
    public Item[] inventory;
    public Transform[] inventorySlotPositions;

    protected void Start(){
        inventory = new Item[numberOfInventorySlots];
    }
    // generates the inventory array and populates it with nulls

    protected void Update(){
        for(int i = 0; i < inventory.Length; i++){
            if(inventory[i] == null)
                continue;

            inventory[i].gameObject.transform.position = new Vector3(
                inventorySlotPositions[i].position.x,
                inventorySlotPositions[i].position.y + inventory[i].GetComponent<Item>().itemData.height / 2,
                inventorySlotPositions[i].position.z
                // sets the inventories position 
                // y level is altered per item as they are not all the same hight
            );
            inventory[i].gameObject.transform.rotation = gameObject.transform.rotation;
            inventory[i].gameObject.transform.parent = gameObject.transform;
        }
    }
    // sets the inventories position to the correct place

    public virtual Item TakeItem(){
        //check if inventory is empty
        if(inventory[0] == null)
            return null;

        // get last item in inventory
        int lastItemIndex = inventory.Length - 1;

        for(int i = 0; i < inventory.Length; i++){
            if(inventory[i] != null){
                lastItemIndex = i;
            }
        }

        Item temp = inventory[lastItemIndex];
        inventory[lastItemIndex] = null;

        return temp;
    }

    public virtual void PlaceItem(Item itemToPlace){
        //check if inventory is full
        if(isInventoryFull())
            return;
                
        // get last available empty slot index
        int lastEmptySlot = 0;
        for(int i = 0; i < inventory.Length; i++){
            if(i - 1 < 0)
                continue;

            if(inventory[i] == null && inventory[i - 1] != null){
                lastEmptySlot = i;
            }
        }

        inventory[lastEmptySlot] = itemToPlace;
        // delete itemtoPlace in interact function
    }

    protected bool isInventoryFull(){
        return inventory[inventory.Length - 1] == null ? false : true;
    }

    protected void DestroyInventory(){
        // destroy ingregients
        for(int i = 0; i < inventory.Length; i++){
            if(inventory[i] == null)
                continue;

            Destroy(inventory[i].gameObject);
            inventory[i] = null;
        }
    }

    public abstract void Interact();

    public virtual void PickupAndPlace(Inventory playerInventory, Inventory benchInventory)
    {
        if (playerInventory.inventory[0] && !isInventoryFull()){
            // place
            benchInventory.PlaceItem(playerInventory.inventory[0]);
            playerInventory.inventory[0] = null;
        }
        else if(!playerInventory.inventory[0]){
            // take
            playerInventory.inventory[0] = benchInventory.TakeItem();
        }
    }
}