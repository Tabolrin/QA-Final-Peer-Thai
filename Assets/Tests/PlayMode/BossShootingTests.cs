using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BossShootingTests
{
    [UnityTest]
    public IEnumerator BossShooting_FirstUpdate_FiresAFullVolleyOfOrbs()
    {
        // Left active, matching the real orb prefab's default state - Instantiate
        // copies the source's active state onto each clone, and an inactive
        // source would produce inactive (therefore unfindable-by-tag) clones.
        var orbPrefab = new GameObject("Orb_Test");

        var boss = new GameObject("BossShooting_Test");
        var shooting = boss.AddComponent<BossShooting>();
        shooting.orbProjectile = orbPrefab;
        shooting.orbsPerVolley = 3;
        shooting.spreadDegrees = 15f;
        shooting.fireInterval = 100f; // long enough that only the opening volley fires in this test

        yield return null; // let Update() run once

        Transform[] allTransforms = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
        int cloneCount = 0;
        foreach (Transform t in allTransforms)
        {
            if (t.gameObject.name == "Orb_Test(Clone)")
            {
                cloneCount++;
                Object.Destroy(t.gameObject);
            }
        }
        Assert.AreEqual(3, cloneCount, "A volley should spawn exactly orbsPerVolley orbs.");

        Object.Destroy(boss);
        Object.Destroy(orbPrefab);
    }
}
