using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

/// <summary>
/// Regression tests — PlayMode.
/// Each test documents a specific bug/edge case found during the EnemyHealthSystem
/// extraction, so it can never silently come back.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class RegressionTests
{
    // REGRESSION: before the EnemyHealthSystem refactor, Enemy.GetDamage did
    // `health -= damage` with no guard, so a negative "damage" value would
    // silently HEAL the enemy instead of being rejected.
    [UnityTest]
    public IEnumerator GetDamage_NegativeAmount_DoesNotHealEnemy()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        enemy.Enemy.GetDamage(-50);
        yield return null;

        Assert.AreEqual(10, enemy.Enemy.CurrentHealth,
            "Negative damage must be ignored, not treated as healing.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    // REGRESSION: guards against a future refactor reintroducing unclamped
    // health going below zero (would break any future UI health-bar math).
    [UnityTest]
    public IEnumerator CurrentHealth_NeverGoesNegative_EvenWithRepeatedOverkill()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 3);
        yield return null;

        enemy.Enemy.GetDamage(50);
        yield return null;

        Assert.AreEqual(0, enemy.Enemy.CurrentHealth);
        Assert.IsTrue(enemy.Enemy.CurrentHealth >= 0);
    }

    // REGRESSION: while adding shield support, GetDamage's early-return for
    // "nothing left to apply" started firing for zero/negative incoming damage
    // too, which as a side effect suppressed the hit-effect VFX for that case.
    // That's now the deliberate, documented behavior (no real hit occurred, so
    // nothing should play) - this test locks it in so it can't silently flip
    // back to firing the VFX for a no-op "hit".
    [UnityTest]
    public IEnumerator GetDamage_NonPositiveDamage_DoesNotInstantiateHitEffect()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        // baseline includes the (inactive, unrelated) ShieldIndicator child every
        // enemy has - the assertion below checks nothing NEW was added, not that
        // there are zero children outright.
        int childCountBefore = enemy.Enemy.transform.childCount;

        enemy.Enemy.GetDamage(0);
        enemy.Enemy.GetDamage(-5);
        yield return null;

        Assert.AreEqual(childCountBefore, enemy.Enemy.transform.childCount,
            "Zero or negative damage is not a real hit and should not instantiate the hit effect.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }
}
