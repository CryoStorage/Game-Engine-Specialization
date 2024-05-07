using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;

    private static readonly int moving = Animator.StringToHash("moving");
    private static readonly int destoyed = Animator.StringToHash("destroyed");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_playerMovement.State)
        {
            case PlayerMovement.PlayerState.Walking:
                _animator.SetBool(moving, true);
                _animator.SetBool(destoyed, false);
                break;
            case PlayerMovement.PlayerState.Idle:
                _animator.SetBool(moving, false);
                _animator.SetBool(destoyed, false);
                break;
            case PlayerMovement.PlayerState.Dead:
                _animator.SetBool(moving, false);
                _animator.SetBool(destoyed, true);
                break;
        }
    }
}
