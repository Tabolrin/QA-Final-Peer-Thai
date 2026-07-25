using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior.
/// Health/damage logic lives in EnemyHealthSystem (plain C#, unit-tested separately);
/// this MonoBehaviour wraps it and keeps the original public API unchanged.
/// </summary>
public class Enemy : MonoBehaviour, IDamageable {

    #region FIELDS
    [Tooltip("Health points in integer")]
    public int health;

    [Tooltip("Shield hit points that absorb damage before it reaches health. 0 = no shield.")]
    public int shieldHealth;

    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;

    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;

    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during tha path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    [Tooltip("Sprite color to reset a shielded enemy to once its shield breaks, so it visually reads as a normal enemy")]
    public Color NormalEnemyColor = Color.white;

    [Tooltip("Non-color shield cue (e.g. a ring sprite) shown while shieldHealth > 0, hidden once the shield breaks - " +
             "so shielded-vs-not doesn't rely on color alone")]
    public GameObject shieldIndicator;
    #endregion

    private EnemyHealthSystem _healthSystem;
    private ShieldSystem _shieldSystem;
    private SpriteRenderer _spriteRenderer;
    private bool _shieldBrokenVisualApplied;

    public int CurrentHealth => _healthSystem.CurrentHealth;
    public int MaxHealth => _healthSystem.MaxHealth;
    public int CurrentShield => _shieldSystem != null ? _shieldSystem.CurrentShield : 0;

    private void Awake()
    {
        _healthSystem = new EnemyHealthSystem(health);
        if (shieldHealth > 0)
            _shieldSystem = new ShieldSystem(shieldHealth);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (shieldIndicator != null)
            shieldIndicator.SetActive(_shieldSystem != null);
    }

    private void Start()
    {
        Invoke("ActivateShooting", Random.Range(shotTimeMin, shotTimeMax));
    }

    //coroutine making a shot
    void ActivateShooting()
    {
        if (Random.value < (float)shotChance / 100)                             //if random value less than shot probability, making a shot
        {
            Instantiate(Projectile,  gameObject.transform.position, Quaternion.identity);
        }
    }

    //method of getting damage for the 'Enemy'
    public void GetDamage(int damage)
    {
        if (damage <= 0)
            return; // not a real hit - nothing to apply, nothing to show

        int remainingDamage = _shieldSystem != null ? _shieldSystem.AbsorbDamage(damage) : damage;

        //the moment the shield breaks, drop the shielded tint so the enemy visually
        //reads as a normal enemy from here on
        if (_shieldSystem != null && _shieldSystem.IsBroken() && !_shieldBrokenVisualApplied)
        {
            _shieldBrokenVisualApplied = true;
            if (_spriteRenderer != null)
                _spriteRenderer.color = NormalEnemyColor;
            if (shieldIndicator != null)
                shieldIndicator.SetActive(false);
        }

        if (remainingDamage <= 0)
        {
            // shield absorbed the whole hit: health is untouched, but still
            // show the hit effect so the player gets feedback that it landed
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
            return;
        }

        _healthSystem.TakeDamage(remainingDamage);
        health = _healthSystem.CurrentHealth; //keep inspector-visible field in sync
        if (_healthSystem.IsDead())
            Destruction();
        else
            Instantiate(hitEffect,transform.position,Quaternion.identity,transform);
    }

    //if 'Enemy' collides 'Player', 'Player' gets the damage equal to projectile's damage value
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var projectile = Projectile.GetComponent<Projectile>();
            if (projectile != null)
                Player.instance.GetDamage(projectile.damage);
            else
                Player.instance.GetDamage(1);
        }
    }

    //method of destroying the 'Enemy'
    void Destruction()
    {
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
