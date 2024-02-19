using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public GameInputActions InputActions{get; private set;}

    [Header("GameEvents")] 
    [SerializeField]private GameEvent playerPressA;
    
    private bool _isFirstPress = true;
    
    private void OnEnable()
    { 
        InputActions = new GameInputActions(); 
        InputActions.Player.Enable();
        InputActions.Player.Interact.performed += ctx => FirstPress();
    }
    
    private void FirstPress()
    {
        if (!_isFirstPress)return;
        playerPressA.Raise();
        _isFirstPress = false;
    }
}
