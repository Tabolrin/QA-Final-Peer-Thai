using UnityEngine;

/// <summary>
/// Boss-specific shooting: fires a volley of orb projectiles on a repeating
/// timer, instead of the single random shot regular enemies use (see
/// Enemy.ActivateShooting). Reuses OrbVolley for the spread pattern.
/// </summary>
public class BossShooting : MonoBehaviour
{
    [Tooltip("Orb projectile prefab fired by the boss")]
    public GameObject orbProjectile;

    [Tooltip("Number of orbs fired per volley")]
    public int orbsPerVolley = 3;

    [Tooltip("Angle (degrees) between adjacent orbs in a volley")]
    public float spreadDegrees = 15f;

    [Tooltip("Seconds between volleys")]
    public float fireInterval = 2.5f;

    float nextFireTime;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireVolley();
            nextFireTime = Time.time + fireInterval;
        }
    }

    void FireVolley()
    {
        if (orbProjectile == null)
            return;

        foreach (float angle in OrbVolley.SpreadAngles(orbsPerVolley, spreadDegrees))
            Instantiate(orbProjectile, transform.position, Quaternion.Euler(0, 0, angle));
    }
}
