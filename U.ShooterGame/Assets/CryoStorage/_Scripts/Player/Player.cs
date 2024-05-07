using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IAttackable
{
    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxShield = 3;
    [SerializeField] private float shieldRechargeRate = 1f;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onPlayerDied;
    [SerializeField] private GameEvent onPlayerHit;
    [SerializeField] private GameEvent onHealthReplenished;
    
    [FormerlySerializedAs("playerHealth")]
    [Header("GameVariables")]
    [SerializeField] private IntVariableSo playerHealthSo;
    [SerializeField] private IntVariableSo playerShield;
    
    private int _currentHealth;
    private int _currentShield;
    private bool _regenInterrupted;
    private float elapsedTime;

    private void OnEnable()
    {
        _currentHealth = maxHealth;
        _currentShield = maxShield;
        playerHealthSo.value = _currentHealth;
        playerShield.value = _currentShield;
    }
    
    private void Update()
    {
        ShieldRegen();
    }

    private void ShieldRegen()
    {
        playerShield.value = _currentShield;
        elapsedTime += Time.deltaTime;
        if(_regenInterrupted) return;
        if(_currentShield >= maxShield) return;
        if(elapsedTime < shieldRechargeRate) return;
        _currentShield += 1;
        elapsedTime = 0;
    }
    
    
    public void TakeDamage(int damage)
    {
        _regenInterrupted = true;
        StartCoroutine(CorShieldInterrupt());
        switch (damage)
        {
            //incoming damage is greater than current shield
            case int when (damage > _currentShield):
                _currentHealth -= damage - _currentShield;
                _currentShield = 0;
                break;
            //incoming damage is less than current shield
            case int when (damage < _currentShield):
                _currentShield -= damage;
                break;
            //incoming damage is equal to current shield
            case int when (damage == _currentShield):
                _currentShield = 0;
                break;
            
        }
        onPlayerHit.Raise();
        playerHealthSo.value = _currentHealth;
        playerShield.value = _currentShield;
        
        if(_currentHealth <= 0) Die();
    }
    
    IEnumerator CorShieldInterrupt()
    {
        yield return new WaitForSeconds(7f);
        _regenInterrupted = false;
    }

    public void Die()
    {
        onPlayerDied.Raise();
        playerHealthSo.value = maxHealth;
        playerHealthSo.value = maxShield;
    }

    public void Replenish()
    {
        _currentHealth = maxHealth;
        onHealthReplenished.Raise();
    }

   
}
