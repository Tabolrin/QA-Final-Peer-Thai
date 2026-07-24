using System.Diagnostics;
using NUnit.Framework;

/// <summary>
/// Load Tests — EditMode.
///
/// Asks: "does the system hold up under a realistic-to-heavy number of
/// simultaneous objects?" Level 2's harder wave settings top out around 14
/// enemies per wave (Wave_4) with several waves able to overlap in flight -
/// these tests go well beyond that to leave headroom.
///
/// Place in: Assets/Tests/EditMode/NonFunctional/
/// </summary>
public class LoadTests
{
    private const int LOAD_TEST_BUDGET_MS = 100;

    [Test]
    public void OneThousandEnemies_AllTakeDamage_CompletesUnderBudget_AndAllReachExpectedState()
    {
        var enemies = new EnemyHealthSystem[1000];
        for (int i = 0; i < enemies.Length; i++)
            enemies[i] = new EnemyHealthSystem(2);

        var sw = Stopwatch.StartNew();
        foreach (var enemy in enemies)
            enemy.TakeDamage(2);
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, LOAD_TEST_BUDGET_MS,
            $"Damaging 1000 enemies took {sw.ElapsedMilliseconds}ms — budget is {LOAD_TEST_BUDGET_MS}ms.");

        foreach (var enemy in enemies)
            Assert.IsTrue(enemy.IsDead(), "Every enemy should have died from exactly-lethal damage.");
    }

    [Test]
    public void TwoHundredShieldedEnemies_FullDamageSequence_CompletesUnderBudget()
    {
        // 200 is well beyond what Level 2 ever has on screen (max 8 shielded
        // enemies per wave), used here purely to confirm there's no hidden
        // scaling problem waiting further down the line.
        var shields = new ShieldSystem[200];
        var healths = new EnemyHealthSystem[200];
        for (int i = 0; i < 200; i++)
        {
            shields[i] = new ShieldSystem(5);
            healths[i] = new EnemyHealthSystem(2);
        }

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 200; i++)
        {
            for (int hit = 0; hit < 7; hit++)
            {
                int overflow = shields[i].AbsorbDamage(1);
                if (overflow > 0) healths[i].TakeDamage(overflow);
            }
        }
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, LOAD_TEST_BUDGET_MS,
            $"Full 7-hit sequence on 200 shielded enemies took {sw.ElapsedMilliseconds}ms — budget is {LOAD_TEST_BUDGET_MS}ms.");

        foreach (var health in healths)
            Assert.IsTrue(health.IsDead(), "Every shielded enemy should be dead after its full 7-hit sequence.");
    }
}
