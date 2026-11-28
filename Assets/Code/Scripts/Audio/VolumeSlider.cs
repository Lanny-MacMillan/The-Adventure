using UnityEngine;
using UnityEngine.UI;

// Pause menu slider for music or sound effect volume. Saved in PlayerPrefs.
[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private bool music = true;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(Changed);
    }

    private void OnEnable()
    {
        slider.SetValueWithoutNotify(music ? Sound.MusicVolume : Sound.SfxVolume);
    }

    private void Changed(float v)
    {
        if (music)
        {
            Sound.MusicVolume = v;
        }
        else
        {
            Sound.SfxVolume = v;
            Sound.Play("menuMove");
        }
    }
}
