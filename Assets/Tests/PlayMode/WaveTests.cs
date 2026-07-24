using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Wave tests — PlayMode.
/// Wave.cs's spawn coroutine used to assume every spawned GameObject has an
/// Enemy component (to configure shooting stats), which throws a
/// NullReferenceException for anything else - e.g. a Boss, which is its own
/// component and deliberately does not extend Enemy. This covers that the
/// wave can spawn a non-Enemy GameObject (as Level 2's boss wave needs to)
/// without crashing.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class WaveTests
{
    [UnityTest]
    public IEnumerator CreateEnemyWave_SpawningGameObjectWithoutEnemyComponent_DoesNotThrow()
    {
        // A minimal stand-in for a boss wave entry: has FollowThePath (which
        // every Wave-spawned GameObject needs) and Boss, but NOT Enemy.
        var template = new GameObject("BossTemplate_Test");
        template.AddComponent<FollowThePath>();
        template.AddComponent<Boss>();
        template.SetActive(false);

        var pathA = new GameObject("PathA_Test").transform;
        var pathB = new GameObject("PathB_Test").transform;
        pathA.position = new Vector3(0, 0, 0);
        pathB.position = new Vector3(0, 5, 0);

        var waveRoot = new GameObject("Wave_Test");
        var wave = waveRoot.AddComponent<Wave>();
        wave.enemy = template;
        wave.count = 1;
        wave.speed = 10;
        wave.timeBetween = 0;
        wave.pathPoints = new Transform[] { pathA, pathB };
        wave.rotationByPath = false;
        wave.Loop = false;
        wave.shooting = new Shooting { shotChance = 0, shotTimeMin = 0, shotTimeMax = 0 };

        yield return null; // Start() runs, kicking off CreateEnemyWave()
        yield return null; // let the coroutine's first spawn step run

        // Unity Test Framework fails the test automatically if an unhandled
        // exception was logged during the frames above - reaching here means
        // the spawn did not throw. Confirm it actually spawned, too.
        var spawnedBoss = Object.FindFirstObjectByType<Boss>();
        Assert.IsNotNull(spawnedBoss, "Wave should have spawned the boss-like GameObject.");
        Assert.IsTrue(spawnedBoss.gameObject.activeSelf);

        Object.Destroy(template);
        Object.Destroy(pathA.gameObject);
        Object.Destroy(pathB.gameObject);
        if (waveRoot != null) Object.Destroy(waveRoot);
        if (spawnedBoss != null) Object.Destroy(spawnedBoss.gameObject);
    }
}
