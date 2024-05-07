using System;
using CryoStorage;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class PlayerMovement : MonoBehaviour, IAimTarget
{
    
    public Vector3 Position => transform.position;
    public Vector3 Velocity => _characterController.velocity;
    public PlayerState State => _playerState;
    
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float strafeSpeed = 5f;
    [SerializeField] private float friction = 0.9f;
    [SerializeField]private float boostCooldown = 6;
    [SerializeField] private float boostForce = 10f;

    [Header("Player Aiming")] 
    [SerializeField] private float lerpSpeed = 10f;
    [SerializeField] private float aimCircleRadius = 5f;
        
    private CharacterController _characterController;
    private PlayerInputHandler _playerInputHandler;
    
    private Vector3 _dir;
    private Vector2 _leftStickVector;
    private Vector2 _rightStickVector;
    private float _inputAngle;
    private float _boostTimer;
    
    private Camera _mainCam;
    [SerializeField]private InputStyle _inputStyle;
    private PlayerState _playerState;
    
    private enum InputStyle{Keyboard, Controller}

    public enum PlayerState
    {
        Walking,
        Strafing,
        Boosting,
        Idle,
        Dead,
        None
    }
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 2;
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _characterController = GetComponent<CharacterController>();
        _playerInputHandler.InputActions.Player.BoostL.performed += BoostLeft;
        _playerInputHandler.InputActions.Player.BoostR.performed += BoostRight;
        _playerInputHandler.InputActions.Player.BoostL.canceled += ExitStrafeMode;
        _playerInputHandler.InputActions.Player.BoostR.canceled += ExitStrafeMode;
        _mainCam = Camera.main;
    }

    private void OnDisable()
    {
        _playerInputHandler.InputActions.Player.BoostL.performed -= BoostLeft;
        _playerInputHandler.InputActions.Player.BoostR.performed -= BoostRight;
        _playerInputHandler.InputActions.Player.BoostL.canceled -= ExitStrafeMode;
        _playerInputHandler.InputActions.Player.BoostR.canceled -= ExitStrafeMode;
    }

    private void Update()
    {
        switch (_playerState)
        {
            case PlayerState.Boosting:
                break;
            case PlayerState.Walking:
                _leftStickVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
                _rightStickVector = _playerInputHandler.InputActions.Player.RightStick.ReadValue<Vector2>();
                ReadBumpers();
                break;
            case PlayerState.Strafing:
                ReadBumpers();
                _rightStickVector = _playerInputHandler.InputActions.Player.RightStick.ReadValue<Vector2>();
                
                break;
            case PlayerState.Idle:
                _leftStickVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
                _rightStickVector = _playerInputHandler.InputActions.Player.RightStick.ReadValue<Vector2>();
                ReadBumpers();
                break;
            case PlayerState.Dead:
                break;
            case PlayerState.None:
                _leftStickVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
                _rightStickVector = _playerInputHandler.InputActions.Player.RightStick.ReadValue<Vector2>();
                ReadBumpers();
                break;
        }
         
        _boostTimer += Time.deltaTime;
    }
    
    private void ReadBumpers()
    {
        StrafeLeft(_playerInputHandler.InputActions.Player.BoostL.ReadValue<float>() >= .5f); 
        StrafeRight(_playerInputHandler.InputActions.Player.BoostR.ReadValue<float>() >= .5f); 
    }

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }

    private void HandleTranslation()
    {
        Vector3 force = new Vector3(_leftStickVector.x, 0, _leftStickVector.y);
        _dir += force * moveSpeed;
        _characterController.Move(_dir * Time.fixedDeltaTime);
        _playerState = PlayerState.Walking;
        // Apply Friction
        if(_dir.magnitude > 0.1f)
        {
            _dir -= _dir * (friction * Time.fixedDeltaTime);
        }else
        {
            _dir = Vector3.zero;
            _playerState = PlayerState.Idle;
        }
    }

    private void HandleRotation()
    {
        var pos = transform.position;
        switch (_inputStyle)
        {
            case InputStyle.Controller:
                if (_rightStickVector == Vector2.zero) return;
                _inputAngle = CryoMath.AngleFromOffset(_rightStickVector);
                break;
            
            case InputStyle.Keyboard:
                var worldPos = GetCursorDirection();
                _inputAngle = CryoMath.AngleFromOffset(new Vector2(worldPos.x, worldPos.z).normalized);
                break;
        }
        var point = CryoMath.PointOnRadius(transform.position, _inputAngle, aimCircleRadius);
        var targetRotation = CryoMath.AimAtDirection(pos, point);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpSpeed * Time.fixedDeltaTime);
    }
    
    private void BoostRight(InputAction.CallbackContext context)
    {
        if (_boostTimer < boostCooldown) return;
        _dir += transform.right * boostForce;
        _boostTimer = 0;
    }

    private void StrafeRight(bool pressed)
    {
        if(!pressed) return;
        _dir += transform.right * strafeSpeed;
        _playerState = PlayerState.Strafing;
    }
    
    private void BoostLeft(InputAction.CallbackContext context)
    {
        if (_boostTimer < boostCooldown) return;
        _dir -= transform.right * boostForce;
        _boostTimer = 0;
    }
    private void StrafeLeft(bool pressed)
    {
        if(!pressed) return;
        _dir -= transform.right * strafeSpeed;
        _playerState = PlayerState.Strafing;
    }
    
    private void ExitStrafeMode(InputAction.CallbackContext context)
    {
        _playerState = PlayerState.Walking;
    }
    

    private Vector3 GetCursorDirection()
    {
        var offset = _mainCam.transform.position.y - transform.position.y;
        var screenPos = Input.mousePosition;
        screenPos.z = _mainCam.nearClipPlane + offset;
        var worldPos = _mainCam.ScreenToWorldPoint(screenPos);

        var result = new Vector3(worldPos.x, worldPos.y, worldPos.z) - transform.position;
        return result;
    }

    public void LockMovement()
    {
        _characterController.enabled = false;
    }
    
    public void UnlockMovement()
    {
        _characterController.enabled = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        try
        {
            var worldPos = GetCursorDirection();
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(worldPos+ transform.position, 0.1f);
            Handles.color = Color.magenta;
            Gizmos.color = Color.magenta;
            Handles.DrawWireDisc(transform.position, Vector3.up, aimCircleRadius);
            var angle = CryoMath.AngleFromOffset(new Vector2(worldPos.x, worldPos.z).normalized);
            var point = CryoMath.PointOnRadius(transform.position, angle, aimCircleRadius);
            Gizmos.DrawSphere(point, 0.1f); 
        }
        catch
        {
            // debug. catch and ignore unassigned reference exceptions when the scene is not playing
        }
    }
#endif

}
