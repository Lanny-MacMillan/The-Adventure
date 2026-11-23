using UnityEngine;

// marks progress when this scene is reached (e.g. PostBathroom = bathroom boss beaten)
public class SetProgressFlag : MonoBehaviour
{
    [SerializeField] private string flag;

    private void Start()
    {
        if (!string.IsNullOrEmpty(flag))
        {
            PlayerPrefs.SetInt(flag, 1);
            PlayerPrefs.Save();
        }
    }
}
