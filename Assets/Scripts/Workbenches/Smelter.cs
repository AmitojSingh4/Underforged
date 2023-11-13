using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Smelter : Inventory{
    Item outputInventory = null;
    [SerializeField] Transform outputSlot;
    [Header("Crafting/Smelting")]
    [SerializeField] CraftingManager craftingManager;
    [Header("UI Stuff")]
    [SerializeField] Canvas progressbarCanvas;
    [SerializeField] Image progressBar;
    bool isSmelting = false;

    new void Start(){
        base.Start();

        progressbarCanvas.gameObject.SetActive(false);
    }

    new void Update(){
        base.Update();

        if(outputInventory != null){
            outputInventory.gameObject.transform.position = outputSlot.position;
            outputInventory.gameObject.transform.parent = gameObject.transform;
        }
    }

    public override void PickupAndPlace(Inventory playerInventory, Inventory benchInventory){
        //Debug.Log("smelt PickupAndPlace");

        bool isItemInPlayerInventory = playerInventory.inventory[0] != null;
        bool isItemInBenchOutput = outputInventory != null;
       
        // place inputs if player inventory is not empty
        if(isItemInPlayerInventory && !isInventoryFull()){
            //Debug.Log("Place");
            PlaceItem(playerInventory.inventory[0]);
            playerInventory.inventory[0] = null;
        }
        // take items if player inv is empty and bench output is empty
        else if(!isItemInPlayerInventory && !isItemInBenchOutput){
            //Debug.Log("take input");
            playerInventory.inventory[0] = TakeItem();
        }
        // take output if player inv is empty and bench output is not empty
        else if(!isItemInPlayerInventory && isItemInBenchOutput){
            //Debug.Log("take output");
            playerInventory.inventory[0] = outputInventory;
            outputInventory = null;
        }
    }
    
    public override void Interact(){
        if(isSmelting == true || outputInventory != null || inventory[0] == null)
            return;

        Item itemToSmelt = craftingManager.findMatchingRecipe(inventory, true);
        //Debug.Log($"Smelting: {itemToSmelt.itemData.itemName}");

        if(!itemToSmelt.itemData.isSmelted || itemToSmelt == null)
            return;

        // Debug.Log(itemToSmelt.itemData.itemName);

        StartCoroutine(Smelt(itemToSmelt));
    }   

    IEnumerator Smelt(Item itemToSmelt){
        //Debug.Log("smelting");
        isSmelting = true;

        DestroyInventory();

        float timeToSmelt = itemToSmelt.itemData.timeTakenToSmelt;
        float _tempTime = timeToSmelt;

        progressbarCanvas.gameObject.SetActive(true);

        while(_tempTime > 0){
            _tempTime -= Time.deltaTime;
            progressBar.fillAmount = 1 - (_tempTime / timeToSmelt);
            yield return null;
        }
        progressbarCanvas.gameObject.SetActive(false);

        GameObject smeltedThing = Instantiate(itemToSmelt.gameObject, outputSlot.position, outputSlot.rotation);
        outputInventory = smeltedThing.GetComponent<Item>();

        isSmelting = false;
        yield break;
    } 
}