using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Integration tests — PlayMode.
/// Answers: do the COMPONENTS communicate correctly? (not whether the arithmetic is right —
/// that's EnemyHealthSystemTests' job).
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class IntegrationTests
{
    [UnityTest]
    public IEnumerator GetDamage_InstantiatesHitEffect_WhenEnemySurvives()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        enemy.Enemy.GetDamage(3);
        yield return null;

        // A clone of hitEffect should now exist as a child of the enemy transform
        Assert.Greater(enemy.Enemy.transform.childCount, 0,
            "GetDamage on a surviving hit should Instantiate hitEffect as a child of the enemy.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator GetDamage_DestroysEnemyGameObject_WhenHealthReachesZero()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 5);
        var root = enemy.Root;
        yield return null;

        enemy.Enemy.GetDamage(5);
        yield return null; // Destroy() is deferred to end of frame

        Assert.IsTrue(root == null, "Enemy GameObject should be destroyed once health reaches zero.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator GetDamage_HealthSystem_StaysInSyncWith_PublicHealthField()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        enemy.Enemy.GetDamage(4);

        Assert.AreEqual(enemy.Enemy.CurrentHealth, enemy.Enemy.health,
            "The legacy public 'health' field must stay in sync with EnemyHealthSystem.CurrentHealth.");

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }
}
