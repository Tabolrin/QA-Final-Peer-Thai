using NUnit.Framework;

/// <summary>
/// Stress Tests — EditMode.
///
/// Asks: "what happens at the extremes and past them?" Not about speed
/// (that's Performance/Load) - about correctness under abusive input:
/// maximum values, rapid repeated calls, and edge-case construction
/// parameters that a normal playthrough would never produce but that
/// defensive code should still handle without throwing.
///
/// Place in: Assets/Tests/EditMode/NonFunctional/
/// </summary>
public class StressTests
{
    [Test]
    public void EnemyHealthSystem_MaximumIntDamage_DoesNotThrow_AndClampsCorrectly()
    {
        var health = new EnemyHealthSystem(10);
        Assert.DoesNotThrow(() => health.TakeDamage(int.MaxValue));
        Assert.AreEqual(0, health.CurrentHealth);
        Assert.IsTrue(health.IsDead());
    }

    [Test]
    public void ShieldSystem_MaximumIntDamage_DoesNotThrow_AndClampsCorrectly()
    {
        var shield = new ShieldSystem(10);
        int overflow = 0;
        Assert.DoesNotThrow(() => overflow = shield.AbsorbDamage(int.MaxValue));
        Assert.AreEqual(0, shield.CurrentShield);
        Assert.IsTrue(shield.IsBroken());
        Assert.Greater(overflow, 0, "A near-infinite hit should still report a large overflow past the shield.");
    }

    [Test]
    public void EnemyHealthSystem_HundredThousandRapidHits_StaysConsistent()
    {
        var health = new EnemyHealthSystem(50_000);
        for (int i = 0; i < 100_000; i++)
            health.TakeDamage(1);

        Assert.AreEqual(0, health.CurrentHealth, "Health should clamp at 0, not go negative from repeated hits past death.");
        Assert.IsTrue(health.IsDead());
    }

    [Test]
    public void EnemyHealthSystem_ZeroMaxHealth_ConstructsAsAlreadyDead()
    {
        var health = new EnemyHealthSystem(0);
        Assert.IsTrue(health.IsDead(), "An enemy authored with 0 health should be considered dead from the start, not crash.");
    }

    [Test]
    public void EnemyHealthSystem_NegativeMaxHealth_DoesNotThrow_AndIsDead()
    {
        // Not a value the game ever produces through normal content authoring,
        // but a misconfigured prefab shouldn't be able to crash the game over it.
        Assert.DoesNotThrow(() =>
        {
            var health = new EnemyHealthSystem(-5);
            Assert.IsTrue(health.IsDead());
        });
    }

    [Test]
    public void ShieldSystem_ZeroMaxShield_IsImmediatelyBroken()
    {
        var shield = new ShieldSystem(0);
        Assert.IsTrue(shield.IsBroken());
        Assert.AreEqual(10, shield.AbsorbDamage(10), "A zero-capacity shield should pass all damage straight through.");
    }
}
