using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAnimHandler : MonoBehaviour
{
    
    [FormerlySerializedAs("playerAnimControllers")]
    [Header("Serialized References")]
    [SerializeField] private PlayerAnimControllersSo playerAnimControllersSo;

    [Header("GameVariables")] 
    [SerializeField] private IntVariableSo intSelectedCharacterSo;
    [SerializeField] private IntVariableSo intSelectedSkinSo;
    
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
        _animator.runtimeAnimatorController = playerAnimControllersSo.characters[intSelectedCharacterSo.value].baseController;
        Debug.Log(intSelectedCharacterSo.value);
    }

    public void SwapSkin()
    {
        _animator.runtimeAnimatorController = playerAnimControllersSo.characters[intSelectedCharacterSo.value].skins[intSelectedSkinSo.value].controller;
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
