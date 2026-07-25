using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class FollowThePathTests
{
    // REGRESSION: a boss on a looping path used to throw IndexOutOfRangeException
    // the moment it reversed direction and reached its starting point again,
    // because currentPathPercent could dip slightly below 0 for one frame before
    // PathLoopMotion clamped it - and that frame's (still negative) value was
    // already used to compute a position. Unity Test Framework fails a PlayMode
    // test on any unhandled exception logged during a frame, so simply running
    // enough frames to reverse direction several times over is the regression check.
    [UnityTest]
    public IEnumerator FollowThePath_LoopingPath_ReversesAtEachEnd_WithoutThrowing()
    {
        Vector3[] positions =
        {
            new Vector3(-5, 5, 0),
            new Vector3(0, 5, 0),
            new Vector3(5, 5, 0),
            new Vector3(-5, 5, 0), // loops back to the first point
        };

        var pathPointObjects = new GameObject[positions.Length];
        var pathTransforms = new Transform[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            pathPointObjects[i] = new GameObject("PathPoint_" + i);
            pathPointObjects[i].transform.position = positions[i];
            pathTransforms[i] = pathPointObjects[i].transform;
        }

        var runner = new GameObject("FollowThePath_Test");
        var follow = runner.AddComponent<FollowThePath>();
        follow.path = pathTransforms;
        follow.speed = 400f; // fast enough to reverse direction several times within the test
        follow.rotationByPath = true; // also exercises the lookAheadPercent branch near the ends
        follow.loop = true;
        follow.SetPath();

        // Enough frames to complete several forward/reverse laps.
        for (int frame = 0; frame < 300; frame++)
            yield return null;

        Assert.IsTrue(follow.movingIsActive, "A looping path should still be running, never destroy itself.");

        Object.Destroy(runner);
        foreach (GameObject p in pathPointObjects)
            Object.Destroy(p);
    }
}
