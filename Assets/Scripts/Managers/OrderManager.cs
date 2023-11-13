using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class OrderManager : MonoBehaviour{
    int totalScore = 0;
    [SerializeField] AllItems allItemsScriptableObject; 
    [SerializeField] GameManager gameManager;
    [SerializeField] List<Order> orders = new List<Order>();
    [SerializeField] GameObject orderPrefab;
    [SerializeField] GameObject TimerScore;
    [SerializeField] Canvas Canvas;
    [SerializeField] Transform OrderBoundingBox;
    List<GameObject> uiOrderList = new List<GameObject>();
    int maxNumOfOrders = 5;
    int timeBetweenOrders = 15;
    int levelLength = 4 * 60;
    List<Item> allItems;
    int elapsedTime = 0;
    bool scoreSent = false;
    bool hasAddedOrderThisSecond = false;
    int tempElapsed;

    void Awake(){
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void Start(){
        allItems = new List<Item>(allItemsScriptableObject.allItems);
        scoreSent = false;
        hasAddedOrderThisSecond = false;
        AddOrder();
    }

    void Update(){
        if(elapsedTime == levelLength){
            if (!scoreSent){
                scoreSent = true;
                gameManager.OnLevelEnd(totalScore);
            }
        }
        // if the level finishes, sends the total score to the game manager
        
        if(elapsedTime % timeBetweenOrders == 0 && !hasAddedOrderThisSecond){
            // add order every 15 seconds
            
            hasAddedOrderThisSecond = true;
            tempElapsed = elapsedTime;
            
            AddOrder();
        }

        if(elapsedTime == tempElapsed + 13)
            hasAddedOrderThisSecond = false;
        // resets the boolean value
        // boolean value is used to prevent multiple orders from being set in a single
        // second as multiple frames are rendered in one second

        elapsedTime = (int) Math.Truncate(Time.realtimeSinceStartup);

        UpdateTimeScoreUI();
    }
    
    void AddOrder(){
        if(orders.Count == maxNumOfOrders)
            return;
        
        int itemDifficulty = CalculateDifficulty();
        int orderTime = 15;

        System.Random rnd = new System.Random();

        List<Item> possibleOrders = allItems.FindAll(i => i.itemData.difficultyTier == itemDifficulty);

        Order chosenOrder = new Order(
            possibleOrders[rnd.Next(possibleOrders.Count)],
            orderTime
        );

        orders.Add(chosenOrder);
        UpdateOrdersUI(chosenOrder);
    }
    // adds an order from the list of all items depending on the difficulty calculated
    // (can't add ores as they do not have a difficulty between 1 and 5)

    public void FulfillOrder(Item item){
        for(int i = 0; i < orders.Count; i++){
            if(orders[i].orderItem.itemData.itemName == item.itemData.itemName){
                totalScore += Item.CalculateItemValue(orders[i].orderItem);
                orders.RemoveAt(i);
                GameObject temp = uiOrderList[i];
                uiOrderList.RemoveAt(i);
                Destroy(temp);
                break;
            }
        }
    }
    // if an order if fulfilled this function is called which calls CalculateItemValue
    // to recursivly generate the score of the item and adds the score to the total score

    double DifficultyFunction(float t){
        // returns value between 1 and 5
        // f(t) = (2)(sin((pi)(t-0.5f)) + 1.5)
        return 2 * (Mathf.Sin((Mathf.PI) * (t - 0.5f)) + 1.5f);
    }

    double DifficultyOverTime(){
        double difficulty = DifficultyFunction((float)elapsedTime / (float)levelLength);
        //Debug.Log($"elapsedTime: {elapsedTime}\t levelLength: {levelLength}");  
        //Debug.Log($"percentage competed level: {(float)elapsedTime / (float)levelLength}\t diff returned: {difficulty}");
        return difficulty;
    }
    // calculates difficulty depending on the time in the level

    int CalculateDifficulty(){
        // calculate which difficulty item to give
        double difficulty = DifficultyOverTime();

        int integerPart = (int) Math.Truncate(difficulty);
        double decimalPart = difficulty - integerPart;

        float percentageOfHigherDifficultyItem = (float) TruncateToSignificantDigits(decimalPart, 2);
        float percentageOfLowerDifficultyItem = 1 - percentageOfHigherDifficultyItem;

        int chosenDifficulty;
        float randomNumber = UnityEngine.Random.Range(0f, 1.0f);
        if(randomNumber < percentageOfHigherDifficultyItem)
            chosenDifficulty = integerPart + 1;
        else
            chosenDifficulty = integerPart;

        //Debug.Log($"Elapsed Time: {elapsedTime}\t chose diff: {chosenDifficulty}\t difffunc: {difficulty}");

        return chosenDifficulty;
    }
    // uses a transformed sin graph to generate a dificulty number depending on time
    // for example if the sin function returns 1.4, the intiger part is used to determain
    // the lower difficulty and then + 1 will be the higher dificulty. if you take the 
    // lower difficulty in this case -1 away from the result 1.4 - 1 = 0.4 this will return the
    // probabilty that the higher dificulty item is chosen and 1 - 0.4 will return 0.6 - the 
    // probability that the lower difficulty item is chosen.

    // https://stackoverflow.com/questions/374316/round-a-double-to-x-significant-figures
    double TruncateToSignificantDigits(double d, int digits){
        if(d == 0)
            return 0;

        double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1 - digits);
        return scale * Math.Truncate(d / scale);
    }

    void UpdateOrdersUI(Order chosenOrder){
        GameObject orderUI = Instantiate(orderPrefab, OrderBoundingBox);
        TextMeshProUGUI[] _texts = orderUI.GetComponentsInChildren<TextMeshProUGUI>();
        string orderName = chosenOrder.orderItem.itemData.itemName;
        _texts[0].text = orderName;
        uiOrderList.Add(orderUI);
    }

    void UpdateTimeScoreUI(){
        TextMeshProUGUI[] texts = TimerScore.GetComponentsInChildren<TextMeshProUGUI>();
        int timeRemaining = levelLength - elapsedTime;
        string timeInMinsAndSec = (Math.Floor((decimal)timeRemaining / (decimal)60)).ToString() + ':' + (((decimal)timeRemaining % (decimal)60)).ToString();
        texts[1].text = timeInMinsAndSec;
        texts[3].text = totalScore.ToString();
    }

    private Texture2D MakeTex( int width, int height, Color col )
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i )
            pix[ i ] = col;
        Texture2D result = new Texture2D( width, height );
        result.SetPixels( pix );
        result.Apply();
        return result;
    }
}