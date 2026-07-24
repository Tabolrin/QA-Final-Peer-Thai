public class ShieldSystem
{
    public int MaxShield { get; private set; }
    public int CurrentShield { get; private set; }

    public ShieldSystem(int maxShield)
    {
        MaxShield = maxShield;
        CurrentShield = maxShield;
    }

    // Absorbs as much of the incoming damage as the shield has left,
    // and returns whatever damage overflowed past it.
    public int AbsorbDamage(int amount)
    {
        if (amount < 0) return 0;
        int absorbed = System.Math.Min(CurrentShield, amount);
        CurrentShield -= absorbed;
        return amount - absorbed;
    }

    public bool IsBroken()
    {
        return CurrentShield <= 0;
    }
}
