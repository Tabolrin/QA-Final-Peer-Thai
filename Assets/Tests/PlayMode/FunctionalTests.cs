using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

/// <summary>
/// Functional Tests — PlayMode.
/// Verify the FEATURE as a whole from the player's perspective: "given this enemy setup,
/// what should happen when it's shot?"
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class FunctionalTests
{
    [UnityTest]
    public IEnumerator Enemy_SurvivesHits_UntilCumulativeDamageReachesHealth()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        var root = enemy.Root;
        yield return null;

        enemy.Enemy.GetDamage(4); // 10 -> 6
        yield return null;
        Assert.IsTrue(root != null, "Enemy should still be alive after non-lethal damage.");
        Assert.AreEqual(6, enemy.Enemy.CurrentHealth);

        enemy.Enemy.GetDamage(4); // 6 -> 2
        yield return null;
        Assert.IsTrue(root != null, "Enemy should still be alive after second non-lethal hit.");
        Assert.AreEqual(2, enemy.Enemy.CurrentHealth);

        enemy.Enemy.GetDamage(2); // 2 -> 0, lethal
        yield return null;
        Assert.IsTrue(root == null, "Enemy should be destroyed once cumulative damage reaches its starting health.");
    }

    [UnityTest]
    public IEnumerator Enemy_OverkillDamage_StillOnlyDestroysOnce()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 5);
        var root = enemy.Root;
        yield return null;

        enemy.Enemy.GetDamage(500); // massive overkill in one hit
        yield return null;

        Assert.IsTrue(root == null, "A single lethal overkill hit should destroy the enemy.");
    }

    [UnityTest]
    public IEnumerator HighHealthEnemy_SurvivesHit_ThatKillsLowHealthEnemy()
    {
        var tough = EnemyTestSceneBuilder.CreateEnemy(health: 100);
        var weak = EnemyTestSceneBuilder.CreateEnemy(health: 5);
        yield return null;

        tough.Enemy.GetDamage(10);
        weak.Enemy.GetDamage(10);
        yield return null;

        Assert.IsTrue(tough.Root != null, "High-health enemy should survive a 10-damage hit.");
        Assert.IsTrue(weak.Root == null, "Low-health enemy should be destroyed by the same 10-damage hit.");

        EnemyTestSceneBuilder.DestroyEnemy(tough);
    }
}
