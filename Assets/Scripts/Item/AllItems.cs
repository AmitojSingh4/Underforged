using UnityEngine;

[CreateAssetMenu(fileName = "AllItems", menuName = "AllItems", order = 3)]
public class AllItems : ScriptableObject{
    public Item[] allItems;
}
// stores the item data of every item in the game