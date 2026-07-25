using UnityEngine;

// Pure spread-pattern math for a multi-projectile volley, kept separate from
// BossShooting so the shape of the volley (how many orbs, how far apart) can
// be verified without Play Mode. Same idea as PlayerShooting's fixed weapon-power
// rotation offsets, generalized to any orb count.
public static class OrbVolley
{
    // Returns the Z-axis rotation offset (degrees) for each orb in an odd-numbered,
    // symmetric volley, evenly spaced by spreadDegrees around the centerline.
    // orbCount <= 1 fires straight down the centerline (offset 0).
    public static float[] SpreadAngles(int orbCount, float spreadDegrees)
    {
        if (orbCount <= 1)
            return new float[] { 0f };

        float[] angles = new float[orbCount];
        float start = -spreadDegrees * (orbCount - 1) / 2f;
        for (int i = 0; i < orbCount; i++)
            angles[i] = start + i * spreadDegrees;
        return angles;
    }
}
