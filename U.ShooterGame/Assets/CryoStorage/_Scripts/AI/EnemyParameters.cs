using UnityEngine;

[CreateAssetMenu(fileName = "EnemyParameters", menuName = "Ai/EnemyParameters", order = 1)]
public class EnemyParameters : ScriptableObject
{
    
    public int maxHealth = 10;
    public int damage = 15;
    public float speed = 3.5f;
    public float attackRange = 5f;
    public float shootingError = 0.1f;
    public float attackRate = 0.5f;
    public float despawnTime = 5f;
    
}
