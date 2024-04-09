using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimHandler : MonoBehaviour
{
    
    [Header("Serialized References")]
    [SerializeField] private PlayerAnimControllers playerAnimControllers;

    [Header("GameVariables")] 
    [SerializeField] private IntVariableSo animControllerIndex; 
    
    private Vector2 _velocity;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private static readonly int Speed = Animator.StringToHash("speed");

        int id = 0;
    private void Start()
    {
        Prepare();
    }

    private void Update()
    {
        _velocity = _playerMovement.Velocity;
        SetSpeed();
        HandleFlip();
        PlayerInputHandler handler = GetComponent<PlayerInputHandler>();

        handler.InputActions.Player.Sprint.performed += ctx => DoSwap();
    }
    
    private void SetSpeed()
    {
        _animator.SetFloat(Speed, _velocity.magnitude);
    }

    private void DoSwap()
    {
        id++;
        if (id > playerAnimControllers.animatorControllers.Length - 1)
        {
            id = 0;
        }
        SwapController(id);
        Debug.Log(id);
    }
    
    private void SwapController(int index)
    {
        _animator.runtimeAnimatorController = playerAnimControllers.animatorControllers[index];
    }
    
    private void HandleFlip()
    {
        switch (_velocity.x)
        {
            case > 0:
                _spriteRenderer.flipX = false;
                break;
            case < 0:
                _spriteRenderer.flipX = true;
                break;
        }
    }

    private void Prepare()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
    }
}
