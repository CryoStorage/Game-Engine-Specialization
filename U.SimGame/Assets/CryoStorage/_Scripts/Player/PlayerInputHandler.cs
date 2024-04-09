using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInputHandler : MonoBehaviour
{
    public GameInputActions InputActions{get; private set;}

    [FormerlySerializedAs("playerPressA")]
    [Header("GameEvents")] 
    [SerializeField] private GameEvent playerPressInteract;

    [SerializeField] private GameEvent inputChangedKeyboard;
    [SerializeField] private GameEvent inputChangedGamepad;
    
    private InputDevice _currentDevice;
    
    private void OnEnable()
    { 
        InputActions = new GameInputActions(); 
        InputActions.Player.Enable();
        InputActions.Player.Interact.performed += ctx => PlayerPressInteract();
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
    
    private void PlayerPressInteract()
    {
        playerPressInteract.Raise();
    }
}
