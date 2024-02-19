using System;
using CryoStorage;
using UnityEngine;

[SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float friction = 0.9f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Serialized References")] 
    [SerializeField] private Vector2VariableSo inputValue;
    
    private CharacterController _characterController;
    private Vector3 _dir;
    private PlayerInputHandler _playerInputHandler;
    private Vector2 _inputVector;
    private bool _inLightHouse;
    private Vector3 _savedPosition;

    private enum PlayerStates
    {
        MoveState,
        LightHouseState,
        CarryState,
        BuildState
    }
    
    private PlayerStates _currentState;

    void Start()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        _inputVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
        inputValue.value = _inputVector;    
        switch (_currentState)
        {
            case PlayerStates.MoveState:
                if (!_characterController.enabled)break;
                HandleTranslation();
                HandleRotation();
                break;
            case PlayerStates.LightHouseState:
                // LightHouseState();
                break;
            case PlayerStates.CarryState:
                // CarryState();
                break;
            case PlayerStates.BuildState:
                // BuildState();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void HandleTranslation()
    {
        Vector3 force = new Vector3(_inputVector.x, 0, _inputVector.y);
        _dir += force * (speed);
        _characterController.Move(_dir * Time.deltaTime);
        // Apply Friction
        if(_dir.magnitude > 0.1f)
        {
            _dir *= friction;
        }else
        {
            _dir = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (_inputVector == Vector2.zero) return;
        var pos = transform.position;
        var targetRotation = CryoMath.AimAtDirection(pos, pos + new Vector3(_inputVector.x, 0, _inputVector.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void LockMovement()
    {
        _characterController.enabled = false;
        var t = transform;
        _savedPosition = t.position;
        t.position = Vector3.zero;
    }
    
    public void UnlockMovement()
    {
        transform.position = _savedPosition;
        _characterController.enabled = true;
    }
    
    

}
