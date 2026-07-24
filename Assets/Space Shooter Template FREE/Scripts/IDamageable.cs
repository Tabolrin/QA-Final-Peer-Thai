/// <summary>
/// Anything that can take damage from a projectile or collision (Enemy, Boss, ...).
/// Lets Projectile.cs damage whatever it hit without depending on a concrete type.
/// </summary>
public interface IDamageable
{
    void GetDamage(int damage);
}
