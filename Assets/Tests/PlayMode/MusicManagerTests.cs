using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// MusicManager tests — PlayMode (needs an AudioSource component, which only
/// behaves correctly once the GameObject is actually in the scene).
/// Place in: Assets/Tests/PlayMode/
/// </summary>
public class MusicManagerTests
{
    private GameObject _root;
    private AudioClip _levelClip;
    private AudioClip _bossClip;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        _levelClip = AudioClip.Create("LevelClip_Test", 1, 1, 44100, false);
        _bossClip = AudioClip.Create("BossClip_Test", 1, 1, 44100, false);

        _root = new GameObject("MusicManager_Test");
        _root.AddComponent<AudioSource>();
        var manager = _root.AddComponent<MusicManager>();
        manager.levelMusic = _levelClip;
        manager.bossMusic = _bossClip;
        yield return null; // let Awake/Start run
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (_root != null) Object.Destroy(_root);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Start_PlaysLevelMusic_Looping()
    {
        var source = _root.GetComponent<AudioSource>();
        Assert.AreEqual(_levelClip, source.clip);
        Assert.IsTrue(source.loop);
        yield break;
    }

    [UnityTest]
    public IEnumerator PlayBossMusic_SwitchesToBossClip()
    {
        MusicManager.instance.PlayBossMusic();
        var source = _root.GetComponent<AudioSource>();
        Assert.AreEqual(_bossClip, source.clip);
        yield break;
    }

    [UnityTest]
    public IEnumerator PlayLevelMusic_AfterBossMusic_SwitchesBackToLevelClip()
    {
        MusicManager.instance.PlayBossMusic();
        MusicManager.instance.PlayLevelMusic();
        var source = _root.GetComponent<AudioSource>();
        Assert.AreEqual(_levelClip, source.clip);
        yield break;
    }
}
