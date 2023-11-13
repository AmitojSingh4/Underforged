using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 2)]

public class ItemData : ScriptableObject{
    public Item[] recipeItems;
    public string itemName;

    [Range(0, 10)]
    public int scoreValue = 1;

    [Range(1, 5)]
    public int difficultyTier;

    [Header("Smelting Options")]
    public bool isSmelted = false; 
    // if made in the smelter
    public float timeTakenToSmelt = 0;

    [Header("Crafting Options")]
    public bool isCrafted = false; 
    // if made in crafter
    public int interactionsNeededToCraft = 0;
    
    [Header("Item Height")]
    public float height = 0f; 
} 
// each items has an instance of this class 
// which holds the data for the given item