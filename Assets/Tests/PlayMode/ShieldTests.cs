using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

/// <summary>
/// Shield Defense tests — PlayMode.
/// Covers the enemy-level wiring of ShieldSystem: damage hits the shield
/// first, and only overflows to real health once the shield breaks.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class ShieldTests
{
    [UnityTest]
    public IEnumerator Enemy_WithShield_TakesNoHealthDamage_WhileShieldAbsorbsHit()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10, shieldHealth: 5);
        yield return null;

        enemy.Enemy.GetDamage(3); // fully absorbed by the 5-point shield
        yield return null;

        Assert.AreEqual(10, enemy.Enemy.CurrentHealth, "Health should be untouched while the shield can absorb the hit.");
        Assert.AreEqual(2, enemy.Enemy.CurrentShield, "Shield should have absorbed exactly the damage dealt.");
        Assert.IsTrue(enemy.Root != null, "Enemy should still be alive.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator Enemy_WithShield_ShowsHitEffect_EvenWhenHitIsFullyAbsorbed()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10, shieldHealth: 5);
        yield return null;

        enemy.Enemy.GetDamage(3); // fully absorbed by the shield
        yield return null;

        Assert.Greater(enemy.Enemy.transform.childCount, 0,
            "A shield-absorbed hit should still play the hit effect, so the player gets feedback that the shot landed.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator Enemy_WithShield_OverflowDamage_ReachesHealth_OnceShieldBreaks()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10, shieldHealth: 5);
        yield return null;

        enemy.Enemy.GetDamage(8); // 5 absorbed by shield, 3 overflows to health
        yield return null;

        Assert.AreEqual(0, enemy.Enemy.CurrentShield, "Shield should be fully depleted.");
        Assert.AreEqual(7, enemy.Enemy.CurrentHealth, "The 3 damage that overflowed the shield should reduce health.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator Enemy_WithoutShield_BehavesExactlyAsBefore()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10); // shieldHealth defaults to 0
        yield return null;

        Assert.AreEqual(0, enemy.Enemy.CurrentShield);

        enemy.Enemy.GetDamage(4);
        yield return null;

        Assert.AreEqual(6, enemy.Enemy.CurrentHealth, "With no shield, all damage should reach health directly.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator Enemy_WithShield_CanStillBeDestroyed_ByLethalOverflowDamage()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 5, shieldHealth: 5);
        var root = enemy.Root;
        yield return null;

        enemy.Enemy.GetDamage(10); // 5 absorbed by shield, 5 overflow kills the enemy
        yield return null;

        Assert.IsTrue(root == null, "Enemy should be destroyed once overflow damage exceeds its health.");
    }
}
