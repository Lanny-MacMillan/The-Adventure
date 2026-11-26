using UnityEngine;

// Every music track and sound effect in one place. Lives at Resources/SoundBank so the SoundManager can find it from any scene.
[CreateAssetMenu(fileName = "SoundBank", menuName = "Franks Revenge/Sound Bank")]
public class SoundBank : ScriptableObject
{
    [System.Serializable]
    public class Track
    {
        public string key;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 0.7f;
    }

    [System.Serializable]
    public class SceneMusic
    {
        public string scene;
        public string track;
    }

    [System.Serializable]
    public class Effect
    {
        public string key;
        public AudioClip[] clips;
        [Range(0f, 1f)] public float volume = 0.8f;
        [Range(0f, 0.3f)] public float pitchJitter = 0.05f;
    }

    public Track[] music;
    // blank track = silence in that scene; scenes not listed keep whatever is playing
    public SceneMusic[] scenes;
    public Effect[] effects;

    public Track FindTrack(string key)
    {
        foreach (Track t in music)
        {
            if (t.key == key)
            {
                return t;
            }
        }
        return null;
    }

    public bool TryGetSceneTrack(string scene, out string track)
    {
        foreach (SceneMusic s in scenes)
        {
            if (s.scene == scene)
            {
                track = s.track;
                return true;
            }
        }
        track = null;
        return false;
    }

    public Effect FindEffect(string key)
    {
        foreach (Effect e in effects)
        {
            if (e.key == key)
            {
                return e;
            }
        }
        return null;
    }
}
