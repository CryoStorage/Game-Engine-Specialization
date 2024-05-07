using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public GameInputActions InputActions{get; private set;}
    
    private void OnEnable()
    { 
        InputActions = new GameInputActions(); 
        InputActions.Player.Enable();
    }
}
