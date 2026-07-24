using UnityEngine;

/// <summary>
/// Plays this level's background music, and swaps to boss music for as long as
/// the boss is alive - reverting to the level's own music once the boss dies.
/// If the player dies while the boss is still alive, nothing tells this to
/// switch back, so boss music naturally just keeps playing.
/// </summary>
public class MusicManager : MonoBehaviour
{
    [Tooltip("This level's regular background music")]
    public AudioClip levelMusic;

    [Tooltip("Music that plays for as long as the boss is alive")]
    public AudioClip bossMusic;

    public static MusicManager instance;

    private AudioSource _source;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayLevelMusic();
    }

    public void PlayLevelMusic()
    {
        Play(levelMusic);
    }

    public void PlayBossMusic()
    {
        Play(bossMusic);
    }

    private void Play(AudioClip clip)
    {
        if (clip == null || _source == null || _source.clip == clip)
            return;
        _source.clip = clip;
        _source.loop = true;
        _source.Play();
    }
}
