using NUnit.Framework;

public class PathLoopMotionTests
{
    [Test]
    public void ClampAndReverseIfNeeded_WithinRange_NoChange()
    {
        bool movingForward = true;
        float result = PathLoopMotion.ClampAndReverseIfNeeded(0.5f, loop: true, movingForward: ref movingForward, shouldDestroy: out bool destroy);

        Assert.AreEqual(0.5f, result);
        Assert.IsTrue(movingForward);
        Assert.IsFalse(destroy);
    }

    [Test]
    public void ClampAndReverseIfNeeded_ReachesEnd_NotLooping_MarksForDestruction()
    {
        bool movingForward = true;
        float result = PathLoopMotion.ClampAndReverseIfNeeded(1.05f, loop: false, movingForward: ref movingForward, shouldDestroy: out bool destroy);

        Assert.IsTrue(destroy);
        Assert.IsTrue(movingForward, "Non-looping objects don't reverse - direction is irrelevant once destroyed.");
    }

    [Test]
    public void ClampAndReverseIfNeeded_ReachesEnd_Looping_TurnsAroundInsteadOfTeleporting()
    {
        bool movingForward = true;
        float result = PathLoopMotion.ClampAndReverseIfNeeded(1.05f, loop: true, movingForward: ref movingForward, shouldDestroy: out bool destroy);

        Assert.AreEqual(1f, result, "Should clamp to the end of the path, not jump back to 0.");
        Assert.IsFalse(movingForward, "Should now be retracing the path backward.");
        Assert.IsFalse(destroy);
    }

    [Test]
    public void ClampAndReverseIfNeeded_ReachesStartWhileReversing_Looping_TurnsForwardAgain()
    {
        bool movingForward = false;
        float result = PathLoopMotion.ClampAndReverseIfNeeded(-0.05f, loop: true, movingForward: ref movingForward, shouldDestroy: out bool destroy);

        Assert.AreEqual(0f, result, "Should clamp to the start of the path, not jump ahead.");
        Assert.IsTrue(movingForward, "Should now be moving forward again.");
        Assert.IsFalse(destroy);
    }

    [Test]
    public void ClampAndReverseIfNeeded_BelowZero_NotLooping_NoChange()
    {
        // Shouldn't normally happen (percent starts at 0 and only goes negative once
        // looping/reversing is active), but guard against it doing anything surprising.
        bool movingForward = true;
        float result = PathLoopMotion.ClampAndReverseIfNeeded(-0.05f, loop: false, movingForward: ref movingForward, shouldDestroy: out bool destroy);

        Assert.AreEqual(-0.05f, result);
        Assert.IsFalse(destroy);
    }
}
