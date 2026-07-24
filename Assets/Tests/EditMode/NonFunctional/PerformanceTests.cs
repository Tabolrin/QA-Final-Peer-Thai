using System.Diagnostics;
using NUnit.Framework;

/// <summary>
/// Performance Tests — EditMode.
///
/// Not "does it work?" but "does it work FAST ENOUGH?" Uses
/// System.Diagnostics.Stopwatch, same approach as the course reference material.
///
/// Defined performance budgets for this system:
///   - Single EnemyHealthSystem construction : under 1ms
///   - 10,000 TakeDamage calls               : under 50ms
///   - Single ShieldSystem construction       : under 1ms
///   - 10,000 AbsorbDamage calls              : under 50ms
///
/// Place in: Assets/Tests/EditMode/NonFunctional/
/// </summary>
public class PerformanceTests
{
    private const int SINGLE_OP_BUDGET_MS = 1;
    private const int BULK_10K_BUDGET_MS = 50;

    // ---- EnemyHealthSystem ----

    [Test]
    public void EnemyHealthSystemConstruction_SingleInstance_UnderBudget()
    {
        var sw = Stopwatch.StartNew();
        var health = new EnemyHealthSystem(100);
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, SINGLE_OP_BUDGET_MS,
            $"Construction took {sw.ElapsedMilliseconds}ms — budget is {SINGLE_OP_BUDGET_MS}ms.");
    }

    [Test]
    public void EnemyHealthSystem_TakeDamage_10000Calls_UnderBudget()
    {
        var health = new EnemyHealthSystem(int.MaxValue);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 10_000; i++)
            health.TakeDamage(1);
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, BULK_10K_BUDGET_MS,
            $"10,000 TakeDamage calls took {sw.ElapsedMilliseconds}ms — budget is {BULK_10K_BUDGET_MS}ms.");
    }

    // ---- ShieldSystem ----

    [Test]
    public void ShieldSystemConstruction_SingleInstance_UnderBudget()
    {
        var sw = Stopwatch.StartNew();
        var shield = new ShieldSystem(100);
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, SINGLE_OP_BUDGET_MS,
            $"Construction took {sw.ElapsedMilliseconds}ms — budget is {SINGLE_OP_BUDGET_MS}ms.");
    }

    [Test]
    public void ShieldSystem_AbsorbDamage_10000Calls_UnderBudget()
    {
        var shield = new ShieldSystem(int.MaxValue);

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 10_000; i++)
            shield.AbsorbDamage(1);
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, BULK_10K_BUDGET_MS,
            $"10,000 AbsorbDamage calls took {sw.ElapsedMilliseconds}ms — budget is {BULK_10K_BUDGET_MS}ms.");
    }

    // ---- Combined Enemy damage path (shield + health together, as GetDamage does at runtime) ----

    [Test]
    public void CombinedShieldAndHealth_10000FullDamagePasses_UnderBudget()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 10_000; i++)
        {
            var shield = new ShieldSystem(5);
            var health = new EnemyHealthSystem(2);
            int overflow = shield.AbsorbDamage(3);
            if (overflow > 0) health.TakeDamage(overflow);
        }
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, BULK_10K_BUDGET_MS,
            $"10,000 combined shield+health damage passes took {sw.ElapsedMilliseconds}ms — budget is {BULK_10K_BUDGET_MS}ms.");
    }
}
