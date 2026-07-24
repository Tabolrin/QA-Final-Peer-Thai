using NUnit.Framework;

public class ShieldSystemTests
{
    [Test]
    public void Constructor_SetsMaxShield_AndStartsAtFullShield()
    {
        var shield = new ShieldSystem(20);
        Assert.AreEqual(20, shield.MaxShield);
        Assert.AreEqual(20, shield.CurrentShield);
    }

    [Test]
    public void AbsorbDamage_ReducesShield_ByAmount_WhenDamageIsLessThanShield()
    {
        var shield = new ShieldSystem(20);
        int overflow = shield.AbsorbDamage(5);
        Assert.AreEqual(15, shield.CurrentShield);
        Assert.AreEqual(0, overflow);
    }

    [Test]
    public void AbsorbDamage_ReturnsOverflow_WhenDamageExceedsShield()
    {
        var shield = new ShieldSystem(10);
        int overflow = shield.AbsorbDamage(15);
        Assert.AreEqual(0, shield.CurrentShield);
        Assert.AreEqual(5, overflow);
    }

    [Test]
    public void AbsorbDamage_NegativeAmount_IsIgnored_AndReturnsZero()
    {
        var shield = new ShieldSystem(10);
        int overflow = shield.AbsorbDamage(-5);
        Assert.AreEqual(10, shield.CurrentShield);
        Assert.AreEqual(0, overflow);
    }

    [Test]
    public void IsBroken_ReturnsFalse_WhileShieldAboveZero()
    {
        var shield = new ShieldSystem(10);
        shield.AbsorbDamage(9);
        Assert.IsFalse(shield.IsBroken());
    }

    [Test]
    public void IsBroken_ReturnsTrue_WhenShieldReachesZero()
    {
        var shield = new ShieldSystem(10);
        shield.AbsorbDamage(10);
        Assert.IsTrue(shield.IsBroken());
    }

    [Test]
    public void AbsorbDamage_OnceBroken_PassesAllFurtherDamageThrough()
    {
        var shield = new ShieldSystem(10);
        shield.AbsorbDamage(10); // breaks it
        int overflow = shield.AbsorbDamage(7);
        Assert.AreEqual(7, overflow);
        Assert.AreEqual(0, shield.CurrentShield);
    }
}
