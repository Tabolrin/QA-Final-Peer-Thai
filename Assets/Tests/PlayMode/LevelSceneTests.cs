using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// Functional test — PlayMode.
/// Actually loads the Level2 scene (added to Build Settings) and lets it run
/// a few frames. Unity Test Framework fails the test automatically if any
/// unhandled error/exception is logged while the scene loads and starts, so
/// reaching the assertions below means every prefab reference (waves, the
/// shielded enemy, the boss) resolved correctly.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class LevelSceneTests
{
    [UnityTest]
    public IEnumerator Level2Scene_LoadsWithoutErrors_AndContainsConfiguredWaves()
    {
        var loadOp = SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        var scene = SceneManager.GetSceneByName("Level2");
        Assert.IsTrue(scene.IsValid(), "Level2 scene should have loaded.");
        Assert.IsTrue(scene.isLoaded);

        // Let Awake/Start run for everything the scene just instantiated
        // (LevelController, Player, etc.) for a few frames.
        yield return null;
        yield return null;

        var controller = Object.FindFirstObjectByType<LevelController>();
        Assert.IsNotNull(controller, "Level2 should contain a LevelController.");
        Assert.AreEqual(12, controller.enemyWaves.Length,
            "Level2's harder settings should include all 12 configured wave entries, ending with the boss.");

        yield return SceneManager.UnloadSceneAsync(scene);
    }
}
