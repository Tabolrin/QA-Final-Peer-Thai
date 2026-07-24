using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

/// <summary>
/// Boss tests — PlayMode.
/// Boss reuses EnemyHealthSystem for its HP (already unit-tested by
/// EnemyHealthSystemTests) - these tests cover only the Boss MonoBehaviour's
/// own wiring: taking damage, dying, and implementing IDamageable so
/// Projectile.cs can damage it without knowing about the concrete type.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class BossTests
{
    [UnityTest]
    public IEnumerator System_CanCreateBoss_WithHugeHealth_WithoutErrors()
    {
        var boss = BossTestSceneBuilder.CreateBoss(health: 500);
        yield return null;

        Assert.IsNotNull(boss.Boss);
        Assert.AreEqual(500, boss.Boss.CurrentHealth);
        Assert.AreEqual(500, boss.Boss.MaxHealth);

        BossTestSceneBuilder.DestroyBoss(boss);
    }

    [UnityTest]
    public IEnumerator GetDamage_ReducesBossHealth_BasicCheck()
    {
        var boss = BossTestSceneBuilder.CreateBoss(health: 500);
        yield return null;

        boss.Boss.GetDamage(40);

        Assert.AreEqual(460, boss.Boss.CurrentHealth);
        BossTestSceneBuilder.DestroyBoss(boss);
    }

    [UnityTest]
    public IEnumerator Boss_SurvivesDamage_ThatWouldDestroyARegularEnemy()
    {
        var boss = BossTestSceneBuilder.CreateBoss(health: 500);
        yield return null;

        boss.Boss.GetDamage(50); // would one-shot most regular enemies

        Assert.IsTrue(boss.Root != null, "A boss with huge HP should survive damage that would kill a normal enemy.");
        BossTestSceneBuilder.DestroyBoss(boss);
    }

    [UnityTest]
    public IEnumerator GetDamage_DestroysBossGameObject_WhenHealthReachesZero()
    {
        var boss = BossTestSceneBuilder.CreateBoss(health: 20);
        var root = boss.Root;
        yield return null;

        boss.Boss.GetDamage(20);
        yield return null; // Destroy() is deferred to end of frame

        Assert.IsTrue(root == null, "Boss GameObject should be destroyed once health reaches zero.");
    }

    [UnityTest]
    public IEnumerator Boss_ImplementsIDamageable_SoProjectileCanDamageItGenerically()
    {
        var boss = BossTestSceneBuilder.CreateBoss(health: 100);
        yield return null;

        IDamageable damageable = boss.Boss as IDamageable;
        Assert.IsNotNull(damageable, "Boss must implement IDamageable so Projectile.cs can damage it without a concrete-type dependency.");

        damageable.GetDamage(25);
        Assert.AreEqual(75, boss.Boss.CurrentHealth);

        BossTestSceneBuilder.DestroyBoss(boss);
    }
}
