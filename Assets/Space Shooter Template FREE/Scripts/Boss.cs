using UnityEngine;

/// <summary>
/// A boss enemy with huge HP. Reuses EnemyHealthSystem (same plain-C# model
/// as Enemy) rather than duplicating health/damage logic. Movement is not
/// handled here: attach the existing FollowThePath component (already used
/// by every other enemy wave) to the same GameObject/prefab with a
/// multi-directional path to satisfy "moves in various directions" - Boss
/// itself only owns health, damage, and destruction.
/// </summary>
public class Boss : MonoBehaviour, IDamageable
{
    [Tooltip("Boss health points - should be set much higher than a regular enemy's")]
    public int health;

    [Tooltip("Damage dealt to the player on direct collision with the boss")]
    public int contactDamage = 1;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    private EnemyHealthSystem _healthSystem;

    public int CurrentHealth => _healthSystem.CurrentHealth;
    public int MaxHealth => _healthSystem.MaxHealth;

    private void Awake()
    {
        _healthSystem = new EnemyHealthSystem(health);
    }

    public void GetDamage(int damage)
    {
        if (damage <= 0)
            return; // not a real hit - nothing to apply, nothing to show

        _healthSystem.TakeDamage(damage);
        health = _healthSystem.CurrentHealth; // keep inspector-visible field in sync
        if (_healthSystem.IsDead())
            Destruction();
        else
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Player.instance.GetDamage(contactDamage);
    }

    private void Destruction()
    {
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
