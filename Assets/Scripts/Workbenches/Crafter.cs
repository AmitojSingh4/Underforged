using UnityEngine;
using UnityEngine.UI;

public class Crafter : Inventory{
    [Header("Crafting/Smelting")]
    [SerializeField] CraftingManager craftingManager;
    [Header("UI")]
    [SerializeField] Canvas progressbarCanvas;
    [SerializeField] Image progressBar;
    int _currentHits = 0;

    new void Start(){
        base.Start();

        progressbarCanvas.gameObject.SetActive(false);
    }
    // hides the progress bar

    new void Update(){
        base.Update();        
    }

    public override void PickupAndPlace(Inventory playerInventory, Inventory benchInventory){
        
        bool isItemInPlayerInventory = playerInventory.inventory[0] != null;
        Debug.Log(isInventoryFull());
        if(isItemInPlayerInventory && !isInventoryFull()){
            // place
            benchInventory.PlaceItem(playerInventory.inventory[0]);
            playerInventory.inventory[0] = null;
        }
        else if(!playerInventory.inventory[0]){
            // take
            playerInventory.inventory[0] = benchInventory.TakeItem();
            _currentHits = 0;

            // reset bar
            progressBar.fillAmount = 0;
            progressbarCanvas.gameObject.SetActive(false);
        }
    }

    public override void Interact(){
        _currentHits++;

        Item itemToCraft = craftingManager.findMatchingRecipe(inventory, false);

        if(itemToCraft == null || !itemToCraft.itemData.isCrafted || inventory[0] == null)
            return;
        
        progressbarCanvas.gameObject.SetActive(true);

        int interactionsNeededToCraft = itemToCraft.itemData.interactionsNeededToCraft;
        //Debug.Log((float)_currentHits / (float)interactionsNeededToCraft);
        progressBar.fillAmount = (float)_currentHits / (float)interactionsNeededToCraft;

        if(_currentHits == interactionsNeededToCraft){
            DestroyInventory();

            // spawn item
            GameObject craftedThing = Instantiate(itemToCraft.gameObject, inventorySlotPositions[0].position, inventorySlotPositions[0].rotation);
            inventory[0] = craftedThing.GetComponent<Item>();

            _currentHits = 0;
            progressbarCanvas.gameObject.SetActive(false);
        }
    }
}