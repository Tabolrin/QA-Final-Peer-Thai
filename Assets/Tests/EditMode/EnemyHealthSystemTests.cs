using NUnit.Framework;

public class EnemyHealthSystemTests
{
    [Test]
    public void Constructor_SetsMaxHealth_AndStartsAtFullHealth()
    {
        var health = new EnemyHealthSystem(25);
        Assert.AreEqual(25, health.MaxHealth);
        Assert.AreEqual(25, health.CurrentHealth);
    }

    [Test]
    public void TakeDamage_ReducesCurrentHealth_ByAmount()
    {
        var health = new EnemyHealthSystem(10);
        health.TakeDamage(3);
        Assert.AreEqual(7, health.CurrentHealth);
    }

    [Test]
    public void TakeDamage_ClampsAtZero_WhenDamageExceedsHealth()
    {
        var health = new EnemyHealthSystem(10);
        health.TakeDamage(999);
        Assert.AreEqual(0, health.CurrentHealth);
    }

    [Test]
    public void TakeDamage_NegativeAmount_IsIgnored()
    {
        var health = new EnemyHealthSystem(10);
        health.TakeDamage(-5);
        Assert.AreEqual(10, health.CurrentHealth);
    }

    [Test]
    public void IsDead_ReturnsFalse_WhileHealthAboveZero()
    {
        var health = new EnemyHealthSystem(10);
        health.TakeDamage(9);
        Assert.IsFalse(health.IsDead());
    }

    [Test]
    public void IsDead_ReturnsTrue_WhenHealthReachesZero()
    {
        var health = new EnemyHealthSystem(10);
        health.TakeDamage(10);
        Assert.IsTrue(health.IsDead());
    }
}
