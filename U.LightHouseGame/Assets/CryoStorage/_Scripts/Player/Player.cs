using UnityEngine;

public class Player : MonoBehaviour, IAttackable
{
    [Header("Player Settings")]
    [SerializeField] private int maxHealth = 3;
    
    [Header("GameEvents")]
    [SerializeField] private GameEvent onPlayerDied;
    
    private int _currentHealth;

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0) Die();
    }

    public void Die()
    {
        onPlayerDied.Raise();
    }
}
