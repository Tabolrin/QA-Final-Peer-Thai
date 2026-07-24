using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Load test — PlayMode.
/// PoolingController is MonoBehaviour-based (Instantiate/Destroy), so this
/// needs a real scene context rather than EditMode.
///
/// Place in: Assets/Tests/PlayMode/NonFunctional/
/// </summary>
public class PoolingLoadTests
{
    private const int LOAD_TEST_BUDGET_MS = 200;

    private GameObject _poolerRoot;
    private GameObject _prefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _prefab = new GameObject("LoadTestPooledThing");
        _poolerRoot = new GameObject("Pooler_LoadTest");
        var pooler = _poolerRoot.AddComponent<PoolingController>();
        pooler.poolingObjectsClass = new PoolingObjects[]
        {
            new PoolingObjects { pooledPrefab = _prefab, count = 20 }
        };
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (_poolerRoot != null) Object.Destroy(_poolerRoot);
        if (_prefab != null) Object.Destroy(_prefab);
        yield return null;
    }

    [UnityTest]
    public IEnumerator RequestingFiveHundredPooledObjects_CompletesUnderBudget_WithNoErrors()
    {
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < 500; i++)
        {
            var obj = PoolingController.instance.GetPoolingObject(_prefab);
            obj.SetActive(true);
        }
        sw.Stop();

        Assert.Less(sw.ElapsedMilliseconds, LOAD_TEST_BUDGET_MS,
            $"500 pool requests took {sw.ElapsedMilliseconds}ms — budget is {LOAD_TEST_BUDGET_MS}ms.");

        yield return null;
    }
}
