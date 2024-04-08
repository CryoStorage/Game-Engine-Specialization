using System;
using System.Collections;
using System.Collections.Generic;
using CryoStorage;
using UnityEngine;

[RequireComponent(typeof(CharacterController))][SelectionBase]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float friction = 0.9f;

    private Vector2 _dir;
    private Vector2 _inputVector;
    private PlayerInputHandler _playerInputHandler;
    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        Prepare();
    }

    
    void Update()
    {
        _inputVector = _playerInputHandler.InputActions.Player.LeftStick.ReadValue<Vector2>();
        HandleTranslation();
    }
    
    private void HandleTranslation()
    {
        var force = new Vector2(_inputVector.x, _inputVector.y);
        _dir += force.normalized * (speed);
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

    // private void HandleRotation()
    // {
    //     if (_inputVector == Vector2.zero) return;
    //     var pos = transform.position;
    //     var targetRotation = CryoMath.AimAtDirection(pos, pos + new Vector3(_inputVector.x, 0, _inputVector.y));
    //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    // }
    
    private void Prepare()
    {
        _playerInputHandler = GetComponent<PlayerInputHandler>();
        _characterController = GetComponent<CharacterController>();
    }
}
