using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]

public class Player : Inventory{
    CharacterController controller;
    [SerializeField] float playerSpeed = 4.0f;
    public int playerIndex;
    Vector2 movementInput = Vector2.zero;

    new void Start(){
        base.Start();

        controller = gameObject.GetComponent<CharacterController>(); 
        controller.center = new Vector3(0f, 0.575f, 0f);
        controller.radius = 0.45f;
    }
    // sets the colliders for the player to be a capsule with those dimmension
    // relative to the center of the player

    public void OnMovement(InputValue value){
        movementInput = value.Get<Vector2>();
    }
    // gets the movement imput as a Vector2 and assigns it to an atribute
    
    new void Update(){
        base.Update();
    }

    void FixedUpdate(){
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        controller.SimpleMove(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
            gameObject.transform.forward = move;
    }
    // causes the player to move depending on the input by adding forces in the x and y dimension

    public override void Interact(){
        return;
    }
}