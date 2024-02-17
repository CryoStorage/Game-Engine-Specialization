using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float friction = 0.9f;

    [Header("Serialized Fields")] 
    [SerializeField] private Vector2VariableSo inputValue;
    
    private CharacterController _characterController;
    private GameInputActions _inputActions;
    private Vector3 _dir;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _inputActions = new GameInputActions();
        _inputActions.Player.Enable();
        
    }
    
    void Update()
    {
        HandleMovement();
        if(_dir.magnitude > 0.1f)
        {
            _dir *= friction;
        }else
        {
            _dir = Vector3.zero;
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _inputActions.Player.LeftStick.ReadValue<Vector2>();
        inputValue.value = inputVector;
        Vector3 force = new Vector3(inputVector.x, 0, inputVector.y);
        _dir += force * (speed);
        _characterController.Move(_dir * Time.deltaTime);
    }
}
