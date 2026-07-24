using NUnit.Framework;

public class PlayFieldBoundsTests
{
    [Test]
    public void ClampMin_ReturnsViewportValue_WhenViewportIsNarrowerThanContent()
    {
        // Viewport min (-6) is inside the content area (-9..9), so it should win.
        Assert.AreEqual(-6f, PlayFieldBounds.ClampMin(-6f, 9f));
    }

    [Test]
    public void ClampMin_ReturnsContentLimit_WhenViewportIsWiderThanContent()
    {
        // Viewport min (-14.5) extends past the designed content area (-9..9).
        Assert.AreEqual(-9f, PlayFieldBounds.ClampMin(-14.5f, 9f));
    }

    [Test]
    public void ClampMax_ReturnsViewportValue_WhenViewportIsNarrowerThanContent()
    {
        Assert.AreEqual(6f, PlayFieldBounds.ClampMax(6f, 9f));
    }

    [Test]
    public void ClampMax_ReturnsContentLimit_WhenViewportIsWiderThanContent()
    {
        Assert.AreEqual(9f, PlayFieldBounds.ClampMax(14.5f, 9f));
    }
}
