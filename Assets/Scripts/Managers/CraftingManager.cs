using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CraftingManager : MonoBehaviour{
    [SerializeField] AllItems allItemsScriptableObject; 
    Item[] allItems;

    void Start(){
        allItems = allItemsScriptableObject.allItems;
    }

    Item[] RemoveNulls(Item[] input){
        // removes null items from arrays
        List<Item> noNulls = new List<Item>();
        for(int i = 0; i < input.Length; i++){
            if(input[i] != null)
                noNulls.Add(input[i]);
        }

        return noNulls.ToArray();
    }

    public Item findMatchingRecipe(Item[] inputItems, bool isFromSmelter){
        inputItems = RemoveNulls(inputItems);
        // removes nulls from arrays as
        // [1,2,3] != [1,2,3,null]

        ItemData[] inputItemsData = new ItemData[inputItems.Length];
        for(int i = 0; i < inputItems.Length; i++){
            inputItemsData[i] = inputItems[i].itemData;
        }
        // extracts item data from the items inputed

        foreach(Item item in allItems){
            Item[] ingredients = item.itemData.recipeItems;

            ItemData[] ingredientsData = new ItemData[ingredients.Length];
            for(int i = 0; i < ingredients.Length; i++){
                ingredientsData[i] = ingredients[i].itemData;
            }

            if(inputItems.Length != ingredients.Length)
                continue;

            ingredientsData = BubbleSort(ingredientsData);
            inputItemsData = BubbleSort(inputItemsData);
            
            IEnumerable<string> ingredientNames = ingredientsData.Select(i => i.itemName);
            IEnumerable<string> inputItemsNames = inputItemsData.Select(i => i.itemName);

            if(item.itemData.isSmelted == isFromSmelter && ingredientsData.SequenceEqual(inputItemsData)){
                return item;
            }
        }

        return null;
    }
    // compares inputted array to every recipe 
    // bubble sorts to make sure both arrays are in order

    ItemData[] BubbleSort(ItemData[] input)
    {
        ItemData temp;

        int lengthOfArray = input.Length;

        for (int i = 0; i < lengthOfArray; i++)
        {
            for (int j = 0; j < lengthOfArray - 1; j++)
            {
                if (input[j].itemName.CompareTo(input[j + 1].itemName) > 0)
                {
                    temp = input[j];
                    input[j] = input[j + 1];
                    input[j + 1] = temp;
                }
            }
        }
        return input;
    }
}