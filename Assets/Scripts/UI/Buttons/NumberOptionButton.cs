using UnityEngine;
using System.Linq;

public class NumberOptionButton : MenuButton{
    enum NumberOptionType {PlayerNumber, DifficultyNumber};
    [SerializeField] int numberOption;
    [SerializeField] NumberOptionType numberOptionType;

    public override void OnClick(){
        Debug.Log($"Selected option {numberOption}");

        // get all NumberOptionButtons in scene
        NumberOptionButton[] allNumOptionButtons = FindObjectsOfType<NumberOptionButton>();

        // store in player prefs
        PlayerPrefs.SetInt(getVariableName(numberOptionType), numberOption);
        Debug.Log($"{numberOptionType}\t{numberOption}");

        // remove highlighting on all buttons
        foreach(NumberOptionButton button in allNumOptionButtons.Where(n => n.numberOptionType == numberOptionType)){
            button.gameObject.GetComponent<Renderer>().material.color = Color.white;
        }
        // highlight this button
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    string getVariableName(NumberOptionType type) => type == NumberOptionType.PlayerNumber ? "numOfPlayers" : "difficulty";
}