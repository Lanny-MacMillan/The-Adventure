using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Plays music and sound effects from the SoundBank. Creates itself on startup and survives scene loads.
// Sound.Play("jump") for effects, Sound.Music("boss") to override the scene's track, Sound.SceneMusic() to go back.
public class Sound : MonoBehaviour
{
    private const float FadeTime = 1.2f;

    private static Sound instance;
    private SoundBank bank;
    private AudioSource[] musicSources;
    private int activeMusic;
    private string currentTrack = "";
    private string overrideTrack;
    private AudioSource[] sfxSources;
    private int nextSfx;
    private Coroutine fade;
    private float trackVolume = 1f;

    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat("musicVolume", 0.8f);
        set
        {
            PlayerPrefs.SetFloat("musicVolume", Mathf.Clamp01(value));
            PlayerPrefs.Save();
            if (instance != null)
            {
                instance.ApplyMusicVolume();
            }
        }
    }

    public static float SfxVolume
    {
        get => PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        set
        {
            PlayerPrefs.SetFloat("sfxVolume", Mathf.Clamp01(value));
            PlayerPrefs.Save();
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Boot()
    {
        SoundBank b = Resources.Load<SoundBank>("SoundBank");
        if (b == null)
        {
            return;
        }
        GameObject go = new GameObject("Sound");
        DontDestroyOnLoad(go);
        instance = go.AddComponent<Sound>();
        instance.bank = b;
    }

    private void Awake()
    {
        musicSources = new AudioSource[2];
        for (int i = 0; i < 2; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].playOnAwake = false;
            musicSources[i].ignoreListenerPause = true;
        }
        sfxSources = new AudioSource[10];
        for (int i = 0; i < sfxSources.Length; i++)
        {
            sfxSources[i] = gameObject.AddComponent<AudioSource>();
            sfxSources[i].playOnAwake = false;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        overrideTrack = null;
        if (bank.TryGetSceneTrack(scene.name, out string track))
        {
            SwitchTo(track);
        }
    }

    public static void Play(string key, float volumeScale = 1f)
    {
        if (instance == null)
        {
            return;
        }
        SoundBank.Effect e = instance.bank.FindEffect(key);
        if (e == null || e.clips == null || e.clips.Length == 0)
        {
            return;
        }
        AudioSource s = instance.sfxSources[instance.nextSfx];
        instance.nextSfx = (instance.nextSfx + 1) % instance.sfxSources.Length;
        s.pitch = 1f + Random.Range(-e.pitchJitter, e.pitchJitter);
        s.PlayOneShot(e.clips[Random.Range(0, e.clips.Length)], e.volume * volumeScale * SfxVolume);
    }

    // boss fights, Game Over and the ending swap the track until the next scene or SceneMusic()
    public static void Music(string key)
    {
        if (instance == null)
        {
            return;
        }
        instance.overrideTrack = key;
        instance.SwitchTo(key);
    }

    public static void SceneMusic()
    {
        if (instance == null)
        {
            return;
        }
        instance.overrideTrack = null;
        if (instance.bank.TryGetSceneTrack(SceneManager.GetActiveScene().name, out string track))
        {
            instance.SwitchTo(track);
        }
    }

    private void SwitchTo(string key)
    {
        key ??= "";
        if (key == currentTrack)
        {
            return;
        }
        currentTrack = key;
        SoundBank.Track t = bank.FindTrack(key);
        if (fade != null)
        {
            StopCoroutine(fade);
        }
        fade = StartCoroutine(Crossfade(t));
    }

    private IEnumerator Crossfade(SoundBank.Track t)
    {
        AudioSource from = musicSources[activeMusic];
        activeMusic = 1 - activeMusic;
        AudioSource to = musicSources[activeMusic];
        float fromStart = from.volume;
        trackVolume = t != null ? t.volume : 0f;
        if (t != null && t.clip != null)
        {
            to.clip = t.clip;
            to.volume = 0f;
            to.Play();
        }
        for (float k = 0f; k < FadeTime; k += Time.unscaledDeltaTime)
        {
            float p = k / FadeTime;
            from.volume = fromStart * (1f - p);
            if (to.isPlaying)
            {
                to.volume = trackVolume * MusicVolume * p;
            }
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        ApplyMusicVolume();
        fade = null;
    }

    private void ApplyMusicVolume()
    {
        if (fade == null && musicSources != null)
        {
            musicSources[activeMusic].volume = trackVolume * MusicVolume;
        }
    }
}
