using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour{
    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform table;
    [SerializeField] GameObject usernameInput;
    [SerializeField] GameObject passwordInput;

    public void Login(){
        Debug.Log("logging in");

        string username = usernameInput.GetComponent<TMP_InputField>().text;
        string password = usernameInput.GetComponent<TMP_InputField>().text;

        Debug.Log(username + ' ' + password);

        StartCoroutine(ApiManager.Login(username, password, () => {
            GetLeaderboard();
        }));
    }
    // gets the inputs from the login boxes and passes them into ApiManager

    public void GetLeaderboard(){
        foreach (Transform child in table) {
            GameObject.Destroy(child.gameObject);
        }

        if(ApiManager.token == null)
            return;

        // get leaderboard data
        StartCoroutine(ApiManager.GetScores(0, 0, (scores) => {
            // populate leaderboard
            for(int i = 0; i < scores.Length; i++){
                GameObject newRow = Instantiate(rowPrefab, table);
                TextMeshProUGUI[] texts = newRow.GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = (i + 1).ToString();
                texts[1].text = scores[i].displayName;
                texts[2].text = scores[i].score.ToString();
            }
        }));
    }   
}