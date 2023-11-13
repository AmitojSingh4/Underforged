using UnityEngine;

public class ChangeMenuButton : MenuButton{
    [SerializeField] Menu menuToChangeTo;

    public override void OnClick(){
        // Debug.Log($"change from {currentMenuPrefab.menuName} to {menuToChangeToPrefab.menuName}");
        transform.parent.GetComponent<Menu>().ToggleMenu(false);
        menuToChangeTo.ToggleMenu(true);
    }
}