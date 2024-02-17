using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float friction = 0.9f;

    [Header("Serialized Fields")] 
    [SerializeField] private Vector3VariableSo inputValue;
    
    private CharacterController _characterController;
    private DefaultInputActions _inputActions;
    private Vector3 _dir;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _inputActions = new DefaultInputActions();
        
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
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 force = new Vector3(inputVector.x, 0, inputVector.y);
        inputValue.Value = force;
        _dir += force * (speed);
        _characterController.Move(_dir * Time.deltaTime);
    }


}
