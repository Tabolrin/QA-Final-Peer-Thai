public class EnemyHealthSystem
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public EnemyHealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount < 0) return;
        CurrentHealth = System.Math.Max(0, CurrentHealth - amount);
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }
}
