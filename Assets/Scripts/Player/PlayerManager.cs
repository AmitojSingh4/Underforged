using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPoint;
    [Range(1, 4)]
    [SerializeField] int maxNumOfPlayers = 1;
    [Header("Player Colour Materials")]
    [SerializeField] Material[] playerColourMaterials;
    private int _tempNumOfPlayers;

    void Start(){
        maxNumOfPlayers = PlayerPrefs.GetInt("numOfPlayers");

        for(int i = 0; i < maxNumOfPlayers; i++){
            string controlScheme = "Player" + (i + 1).ToString();
            PlayerInput.Instantiate(playerPrefab, controlScheme: controlScheme, pairWithDevice : Keyboard.current);
        }
    }
    // gets the number of players and instantiates the prefab of them and gives them
    // unique controll schemes

    public void OnPlayerJoined(PlayerInput playerInput){
        // set spawn position
        playerInput.transform.position = spawnPoint.position;

        // set colour of player
        Transform playerIdentifiers = playerInput.transform.Find("Player Identifier");
        foreach(Transform playerIdentifier in playerIdentifiers){
            playerIdentifier.gameObject.GetComponent<MeshRenderer>().material = playerColourMaterials[_tempNumOfPlayers];
        }
        _tempNumOfPlayers++;
    }
}
