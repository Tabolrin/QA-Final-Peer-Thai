using NUnit.Framework;
using UnityEngine;

public class CatmullRomPathTests
{
    // A simple straight-line loop path, built the same way FollowThePath/Wave
    // build theirs: CreatePoints pads the raw path with two control points.
    static Vector3[] BuildLoopPath()
    {
        Vector3[] raw =
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(2, 0, 0),
            new Vector3(0, 0, 0), // same as the first point - a looping path
        };
        return CatmullRomPath.CreatePoints(raw);
    }

    // REGRESSION: a boss reversing at the start of its (looping) path could hit
    // t slightly below 0 for one frame before PathLoopMotion clamps it, because
    // the clamp only runs after that frame's position has already been computed.
    // That used to throw IndexOutOfRangeException reading path[-1].
    [Test]
    public void Interpolate_SlightlyNegativeT_DoesNotThrow_AndStaysNearStart()
    {
        Vector3[] path = BuildLoopPath();

        Vector3 atZero = Vector3.zero;
        Vector3 atSlightlyNegative = Vector3.zero;
        Assert.DoesNotThrow(() => atZero = CatmullRomPath.Interpolate(path, 0f));
        Assert.DoesNotThrow(() => atSlightlyNegative = CatmullRomPath.Interpolate(path, -0.01f));

        // Clamping the segment index (not the fractional remainder) means a t just
        // below 0 lands a hair before the start point rather than exactly on it -
        // that's fine; what matters is it stays close and, above all, doesn't throw.
        Assert.Less(Vector3.Distance(atZero, atSlightlyNegative), 0.1f,
            "A t just below 0 should stay near the start of the path, not read garbage from past the array.");
    }

    [Test]
    public void Interpolate_SlightlyAboveOne_DoesNotThrow_AndStaysNearEnd()
    {
        Vector3[] path = BuildLoopPath();

        Vector3 atOne = Vector3.zero;
        Vector3 atSlightlyAbove = Vector3.zero;
        Assert.DoesNotThrow(() => atOne = CatmullRomPath.Interpolate(path, 1f));
        Assert.DoesNotThrow(() => atSlightlyAbove = CatmullRomPath.Interpolate(path, 1.01f));

        Assert.Less(Vector3.Distance(atOne, atSlightlyAbove), 0.1f,
            "A t just above 1 should stay near the end of the path, not read garbage from past the array.");
    }

    [Test]
    public void Interpolate_FarNegativeT_DoesNotThrow()
    {
        Vector3[] path = BuildLoopPath();

        Assert.DoesNotThrow(() => CatmullRomPath.Interpolate(path, -5f),
            "Even a large negative t (e.g. after several reversed frames) must not index out of range.");
    }
}
