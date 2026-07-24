// Caps the raw camera-viewport bounds to the play field the game's content
// (enemy wave paths) was actually authored for. On a wide/landscape aspect
// ratio the camera viewport is significantly wider than that content area,
// which otherwise lets the player - and anything that reuses the player's
// bounds, like power-up spawning - wander into empty space where no enemies
// ever appear.
public static class PlayFieldBounds
{
    public static float ClampMin(float viewportMin, float contentHalfWidth)
    {
        return System.Math.Max(viewportMin, -contentHalfWidth);
    }

    public static float ClampMax(float viewportMax, float contentHalfWidth)
    {
        return System.Math.Min(viewportMax, contentHalfWidth);
    }
}
