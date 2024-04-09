using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))][SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    public Vector2 Velocity{get; private set;}
    
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = .3f;
    [SerializeField] private float friction = 0.9f;

    private float _speed;
    private Vector2 _dir;
    private Vector2 _inputVector;
    private PlayerInputHandler _playerInputHandler;
    private CharacterController _characterController;
    

    void Start()
    {
        Prepare();
    }
    
    void Update()
    {
        _inputVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
        HandleTranslation();
        var v = _characterController.velocity;
        Velocity = new Vector2(v.x, v.y);
        if (IsSprint())
        {
            _speed = baseSpeed * 2;
        }
        else
        {
            _speed = baseSpeed;
        }
        
    }

    private bool IsSprint()
    {
        var value = _playerInputHandler.InputActions.Player.Sprint.ReadValue<float>();
        return value > 0.1f;
    }

    private void HandleTranslation()
    {
        var force = new Vector2(_inputVector.x, _inputVector.y);
        _dir += force.normalized * (_speed);
        _characterController.Move(_dir * Time.deltaTime);
        // Apply Friction
        if(_dir.magnitude > 0.1f)
        {
            _dir *= friction;
        }else
        {
            _dir = Vector2.zero;
        }
    }
    
    private void Prepare()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _characterController = GetComponent<CharacterController>();
    }
}
