using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

/// <summary>
/// Smoke Tests — PlayMode.
/// Deliberately shallow: "does the system boot up and respond at all?"
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class SmokeTests
{
    [UnityTest]
    public IEnumerator System_CanCreateEnemy_WithoutErrors()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        Assert.IsNotNull(enemy.Enemy);
        Assert.AreEqual(10, enemy.Enemy.CurrentHealth);
        Assert.AreEqual(10, enemy.Enemy.MaxHealth);

        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }

    [UnityTest]
    public IEnumerator GetDamage_ReducesHealth_BasicCheck()
    {
        var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 10);
        yield return null;

        enemy.Enemy.GetDamage(4);

        Assert.AreEqual(6, enemy.Enemy.CurrentHealth);
        EnemyTestSceneBuilder.DestroyEnemy(enemy);
    }
}
