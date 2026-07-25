// Pure rule for when a level counts as cleared: every configured wave has had
// its full spawn window pass, and nothing tagged "Enemy" is left alive.
// Kept separate from LevelController so the rule itself can be unit tested
// without spinning up a scene and waiting on real coroutine timers.
public static class LevelCompletion
{
    public static bool IsComplete(bool allWavesSpawned, int activeEnemyCount)
    {
        return allWavesSpawned && activeEnemyCount <= 0;
    }
}
