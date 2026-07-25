using NUnit.Framework;

public class OrbVolleyTests
{
    [Test]
    public void SpreadAngles_ThreeOrbs_SymmetricAroundCenter()
    {
        float[] angles = OrbVolley.SpreadAngles(3, 15f);

        Assert.AreEqual(3, angles.Length);
        Assert.AreEqual(-15f, angles[0], 0.001f);
        Assert.AreEqual(0f, angles[1], 0.001f);
        Assert.AreEqual(15f, angles[2], 0.001f);
    }

    [Test]
    public void SpreadAngles_OneOrb_FiresStraightDownCenterline()
    {
        float[] angles = OrbVolley.SpreadAngles(1, 15f);

        Assert.AreEqual(1, angles.Length);
        Assert.AreEqual(0f, angles[0], 0.001f);
    }

    [Test]
    public void SpreadAngles_FiveOrbs_EvenlySpacedAndSymmetric()
    {
        float[] angles = OrbVolley.SpreadAngles(5, 10f);

        Assert.AreEqual(5, angles.Length);
        Assert.AreEqual(-20f, angles[0], 0.001f);
        Assert.AreEqual(-10f, angles[1], 0.001f);
        Assert.AreEqual(0f, angles[2], 0.001f);
        Assert.AreEqual(10f, angles[3], 0.001f);
        Assert.AreEqual(20f, angles[4], 0.001f);
    }
}
