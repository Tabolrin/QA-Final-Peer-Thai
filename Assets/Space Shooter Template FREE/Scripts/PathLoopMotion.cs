// Decides what a path-following object should do once it reaches either end
// of its path: destroy itself (default, non-looping), or turn around and
// retrace the same path backward (looping) instead of jumping straight back
// to the start - which read as a different enemy teleporting in, not the
// same one returning.
public static class PathLoopMotion
{
    public static float ClampAndReverseIfNeeded(float percent, bool loop, ref bool movingForward, out bool shouldDestroy)
    {
        shouldDestroy = false;

        if (percent > 1f)
        {
            if (loop)
            {
                movingForward = false;
                return 1f;
            }
            shouldDestroy = true;
            return percent;
        }

        if (percent < 0f && loop)
        {
            movingForward = true;
            return 0f;
        }

        return percent;
    }
}
