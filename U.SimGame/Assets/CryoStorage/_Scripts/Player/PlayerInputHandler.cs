using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameInputActions InputActions{get; private set;}

    [Header("GameEvents")] 
    [SerializeField] private GameEvent playerPressA;

    [SerializeField] private GameEvent inputChangedKeyboard;
    [SerializeField] private GameEvent inputChangedGamepad;
    
    private bool _isFirstPress = true;
    private InputDevice _currentDevice;
    
    private void OnEnable()
    { 
        InputActions = new GameInputActions(); 
        InputActions.Player.Enable();
        InputActions.Player.Interact.performed += ctx => FirstPress();
        InputActions.Player.KeyboardAction.performed += ctx => InputChangeKeyboard();
        InputActions.Player.GamepadAction.performed += ctx => InputChangeGamepad();
    }
    
    private void InputChangeKeyboard()
    {
        inputChangedKeyboard.Raise();
    }
    
    private void InputChangeGamepad()
    {
        inputChangedGamepad.Raise();
    }
    
    private void FirstPress()
    {
        if (!_isFirstPress)return;
        playerPressA.Raise();
        _isFirstPress = false;
    }
}
