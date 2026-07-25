using UnityEngine;

/// <summary>
/// Static helper that creates a fully wired Enemy GameObject for use in PlayMode tests.
/// Mirrors the PlayerHealth/DeathHandler TestSceneBuilder pattern.
///
/// Usage:
///   var enemy = EnemyTestSceneBuilder.CreateEnemy(health: 5);
///   EnemyTestSceneBuilder.DestroyEnemy(enemy);
/// </summary>
public static class EnemyTestSceneBuilder
{
    public struct EnemyComponents
    {
        public GameObject Root;
        public Enemy Enemy;
        public GameObject HitEffect;
        public GameObject DestructionVFX;
        public GameObject ProjectilePrefab;
        public GameObject ShieldIndicator;
    }

    public static EnemyComponents CreateEnemy(int health = 10, int shieldHealth = 0)
    {
        var hitEffect = new GameObject("HitEffect_Test");
        var destructionVFX = new GameObject("DestructionVFX_Test");
        var projectilePrefab = new GameObject("Projectile_Test");

        // Build the GameObject inactive so AddComponent<Enemy>() does NOT fire
        // Awake() yet — Awake constructs EnemyHealthSystem (and ShieldSystem) from
        // the `health`/`shieldHealth` fields, so those must be set BEFORE Awake
        // runs, not after.
        var root = new GameObject("Enemy_Test");
        root.SetActive(false);
        root.AddComponent<SpriteRenderer>();
        var shieldIndicator = new GameObject("ShieldIndicator_Test");
        shieldIndicator.transform.SetParent(root.transform);
        shieldIndicator.AddComponent<SpriteRenderer>();
        var enemy = root.AddComponent<Enemy>();
        enemy.health = health;
        enemy.shieldHealth = shieldHealth;
        enemy.hitEffect = hitEffect;
        enemy.destructionVFX = destructionVFX;
        enemy.Projectile = projectilePrefab;
        enemy.shieldIndicator = shieldIndicator;
        root.SetActive(true); // now Awake() runs, reading the correct health/shield values

        return new EnemyComponents
        {
            Root = root,
            Enemy = enemy,
            HitEffect = hitEffect,
            DestructionVFX = destructionVFX,
            ProjectilePrefab = projectilePrefab,
            ShieldIndicator = shieldIndicator
        };
    }

    public static void DestroyEnemy(EnemyComponents enemy)
    {
        if (enemy.HitEffect != null) Object.Destroy(enemy.HitEffect);
        if (enemy.DestructionVFX != null) Object.Destroy(enemy.DestructionVFX);
        if (enemy.ProjectilePrefab != null) Object.Destroy(enemy.ProjectilePrefab);
        if (enemy.Root != null) Object.Destroy(enemy.Root); // destroys ShieldIndicator too (it's a child)
    }
}
