using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{

    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set; }

    private AudioSource audioSource;
    private float Volume = .3f;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        Volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);

        audioSource.volume = Volume;
    }

    public void ChangeVolume()
    {
        Volume += .1f;
        if (Volume > 1f)
        {
            Volume = 0f;
        }
        audioSource.volume = Volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, Volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return Volume;
    }
}
