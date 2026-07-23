using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Integration/Functional tests — PlayMode.
/// PoolingController.Start() (which builds the initial pool) only runs once the
/// GameObject enters Play Mode, so this must be a PlayMode ([UnityTest]) suite.
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class PoolingControllerTests
{
    private GameObject _poolerRoot;
    private GameObject _prefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _prefab = new GameObject("PooledThing");
        _poolerRoot = new GameObject("Pooler_Test");
        var pooler = _poolerRoot.AddComponent<PoolingController>();
        pooler.poolingObjectsClass = new PoolingObjects[]
        {
            new PoolingObjects { pooledPrefab = _prefab, count = 2 }
        };
        yield return null; // let Awake/Start build the pool
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (_poolerRoot != null) Object.Destroy(_poolerRoot);
        if (_prefab != null) Object.Destroy(_prefab);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetPoolingObject_ReturnsInactiveExistingClone_WhenAvailable()
    {
        var obj = PoolingController.instance.GetPoolingObject(_prefab);
        yield return null;

        Assert.IsNotNull(obj);
        Assert.IsTrue(obj.name.StartsWith("PooledThing"));
    }

    [UnityTest]
    public IEnumerator GetPoolingObject_CreatesNewObject_WhenPoolIsExhausted()
    {
        // Pool was seeded with count = 2 in SetUp; draw 3 to force creation of a new one.
        var first = PoolingController.instance.GetPoolingObject(_prefab);
        first.SetActive(true);
        var second = PoolingController.instance.GetPoolingObject(_prefab);
        second.SetActive(true);
        var third = PoolingController.instance.GetPoolingObject(_prefab);
        yield return null;

        Assert.IsNotNull(third, "Pool should create a new object once existing pooled instances are all active.");
        Assert.AreNotSame(first, third);
        Assert.AreNotSame(second, third);
    }
}
