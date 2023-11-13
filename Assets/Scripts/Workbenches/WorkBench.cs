using UnityEngine;

public class WorkBench : Inventory{
    new void Start(){
        base.Start();
    }

    new void Update(){
        base.Update();
    }
        
    public override void Interact(){
        return;
    }
}