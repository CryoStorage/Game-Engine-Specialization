using System.Collections;
using UnityEngine;
using CryoStorage;

[SelectionBase]
public class LightHouse : Interactable, IAttackable
{
    [Header("LightHouse Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float damageCooldown = .5f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private bool visualDebug;
    [Tooltip("The radius of the overlap sphere that will be cast from the light house. This is used to detect enemies.")]
    [SerializeField] private float lightSphereRadius;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onEnterLightHouse;
    [SerializeField] private GameEvent onExitLightHouse;
    [SerializeField] private GameEvent onFirstTimeEnter;
    [SerializeField] private GameEvent onLoseCondition;
    
    [Header("Serialized Fields")]
    [SerializeField] private Vector2VariableSo inputDirSo;
    [SerializeField] private GameObject lightObject;
    
    private bool _isActive;
    private float _currentLookAngle;
    private float _targetLookAngle;
    private Vector3 _lookPos;
    private Vector3 _hitPoint;    
    private bool _firstTime = true;
    private int _currentHealth;
    private bool _canTakeDamage = true;

    
    protected override void OnEnable()
    {
        base.OnEnable();
        lightObject.SetActive(false);
        _currentHealth = maxHealth;
    }
    
    void Update()
    {
        if(!_isActive)return;
        GetLookDirection(); 
        CastLightSphere();
    }

    private void CastLightSphere()
    {
        Vector3 castDirection = lightObject.transform.forward + new Vector3(0, -.75f,0);
        if(! Physics.Raycast(lightObject.transform.position, castDirection, out var hit, 50)) return;
        _hitPoint = hit.point;
        Collider[] sphereHits = Physics.OverlapSphere(hit.point, lightSphereRadius);
        foreach (var hitCollider in sphereHits)
        {
            if(! hitCollider.gameObject.TryGetComponent<ILightVulnerable>(out var enemy)) continue;
            enemy.TakeDamage(1);
        }
    }

    private void GetLookDirection()
    {
        if (inputDirSo.value == Vector2.zero) return;
        var position = lightObject.transform.position;
        
        _targetLookAngle = CryoMath.AngleFromOffset(inputDirSo.value);
        _lookPos = CryoMath.PointOnRadius(position, _currentLookAngle);
        _currentLookAngle = CryoMath.LerpAngle(_currentLookAngle, _targetLookAngle, rotationSpeed);
        lightObject.transform.rotation = CryoMath.AimAtDirection(position, _lookPos);  
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(!visualDebug)return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(lightObject.transform.position, 1);
        Gizmos.DrawSphere(_lookPos, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_hitPoint, lightSphereRadius);
    }
    
    public override void Interact()
    {
        if (_isActive)
        {
            _isActive = false;
            onExitLightHouse.Raise();
            lightObject.SetActive(false); 
        }
        else
        {
            if(_firstTime) onFirstTimeEnter.Raise();
            _firstTime = false;
            _isActive = true;
            onEnterLightHouse.Raise();
            lightObject.SetActive(true);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage)return;
        _currentHealth -= damage;
        if(_currentHealth <= 0) Die();
        StartCoroutine(CorDamageCooldown());

    }

    private IEnumerator CorDamageCooldown()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        _canTakeDamage = true;
    }

    public void Die()
    {
        onLoseCondition.Raise();
    }
}
