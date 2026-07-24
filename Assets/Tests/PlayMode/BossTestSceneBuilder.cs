using UnityEngine;

/// <summary>
/// Static helper that creates a fully wired Boss GameObject for use in PlayMode tests.
/// Mirrors EnemyTestSceneBuilder's pattern.
///
/// Usage:
///   var boss = BossTestSceneBuilder.CreateBoss(health: 500);
///   BossTestSceneBuilder.DestroyBoss(boss);
/// </summary>
public static class BossTestSceneBuilder
{
    public struct BossComponents
    {
        public GameObject Root;
        public Boss Boss;
        public GameObject HitEffect;
        public GameObject DestructionVFX;
    }

    public static BossComponents CreateBoss(int health = 500)
    {
        var hitEffect = new GameObject("HitEffect_Test");
        var destructionVFX = new GameObject("DestructionVFX_Test");

        // Build inactive so AddComponent<Boss>() does not fire Awake() until
        // `health` is already set — same ordering constraint as Enemy.
        var root = new GameObject("Boss_Test");
        root.SetActive(false);
        var boss = root.AddComponent<Boss>();
        boss.health = health;
        boss.hitEffect = hitEffect;
        boss.destructionVFX = destructionVFX;
        root.SetActive(true);

        return new BossComponents
        {
            Root = root,
            Boss = boss,
            HitEffect = hitEffect,
            DestructionVFX = destructionVFX
        };
    }

    public static void DestroyBoss(BossComponents boss)
    {
        if (boss.HitEffect != null) Object.Destroy(boss.HitEffect);
        if (boss.DestructionVFX != null) Object.Destroy(boss.DestructionVFX);
        if (boss.Root != null) Object.Destroy(boss.Root);
    }
}
