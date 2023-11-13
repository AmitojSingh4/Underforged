using UnityEngine;

public class Item : MonoBehaviour{
    public ItemData itemData; 
    // seperates items and thier data

    public static int CalculateItemValue(Item inputItem){
        int itemValue = inputItem.itemData.scoreValue;

        foreach(Item item in inputItem.itemData.recipeItems)
            itemValue += CalculateItemValue(item);

        return itemValue;
    }
}
// Item script is given to the prefab of every item
// to link item Gameobjects to thier data