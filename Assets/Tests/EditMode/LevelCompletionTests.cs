using NUnit.Framework;

public class LevelCompletionTests
{
    [Test]
    public void IsComplete_AllWavesSpawned_NoEnemiesLeft_True()
    {
        Assert.IsTrue(LevelCompletion.IsComplete(allWavesSpawned: true, activeEnemyCount: 0));
    }

    [Test]
    public void IsComplete_AllWavesSpawned_EnemiesStillAlive_False()
    {
        Assert.IsFalse(LevelCompletion.IsComplete(allWavesSpawned: true, activeEnemyCount: 3));
    }

    [Test]
    public void IsComplete_NotAllWavesSpawnedYet_EvenWithNoEnemiesOnScreen_False()
    {
        // Guards against a false "complete" reading between waves, e.g. right
        // after one wave's enemies are all dead but a later wave hasn't spawned yet.
        Assert.IsFalse(LevelCompletion.IsComplete(allWavesSpawned: false, activeEnemyCount: 0));
    }
}
