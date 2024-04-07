// Enemy class with no pool for tutorial purposes
public class TutorialEnemy : Enemy
{
    public override void Die()
    {
        onEnemyDied.Raise();
        Destroy(gameObject);
    }
}
