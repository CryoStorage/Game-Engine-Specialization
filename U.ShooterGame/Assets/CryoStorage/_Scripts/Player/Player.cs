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
    
    
    private int _currentHealth;
    private int _currentShield;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentShield = maxShield;
    }

    public void TakeDamage(int damage)
    {
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
