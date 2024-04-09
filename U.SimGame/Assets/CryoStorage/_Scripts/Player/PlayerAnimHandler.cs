using UnityEngine;

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

    private void Start()
    {
        Prepare();
        SwapController();
    }

    private void Update()
    {
        _velocity = _playerMovement.Velocity;
        SetSpeed();
        HandleFlip();
    }
    
    private void SetSpeed()
    {
        _animator.SetFloat(Speed, _velocity.magnitude);
    }

    public void SwapController()
    {
        _animator.runtimeAnimatorController = playerAnimControllers.animatorControllers[animControllerIndex.value];
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
