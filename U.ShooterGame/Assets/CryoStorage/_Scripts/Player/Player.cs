using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IAttackable
{
    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int maxShield = 3;
    [SerializeField] private float shieldRechargeRate = 1f;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onPlayerDied;
    [SerializeField] private GameEvent onHealthReplenished;
    
    [Header("GameVariables")]
    [SerializeField] private IntVariableSo playerHealth;
    [SerializeField] private IntVariableSo playerShield;
    
    private int _currentHealth;
    private int _currentShield;
    private bool _regenInterrupted;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentShield = maxShield;
    }
    
    private void Update()
    {
        ShieldRegen();
    }

    private void ShieldRegen()
    {
        if(_regenInterrupted) return;
        if (_currentShield >= maxShield) return;
        _currentShield += Mathf.FloorToInt(shieldRechargeRate * Time.deltaTime);
        playerShield.value = _currentShield;
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
        
        if(_currentHealth <= 0) Die();
    }
    
    IEnumerator CorShieldInterrupt()
    {
        yield return new WaitForSeconds(3f);
        _regenInterrupted = false;
    }

    public void Die()
    {
        onPlayerDied.Raise();
    }

    public void Replenish()
    {
        _currentHealth = maxHealth;
        onHealthReplenished.Raise();
    }

   
}
