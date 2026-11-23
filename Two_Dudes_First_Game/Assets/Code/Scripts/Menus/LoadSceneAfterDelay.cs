using UnityEngine;
using UnityEngine.SceneManagement;

// Used on the CreatedBy splash: moves on to the title screen after a few seconds,
// or right away if a key or mouse button is pressed.
public class LoadSceneAfterDelay : MonoBehaviour
{
    [SerializeField] private string sceneName = "TitleScreen";
    [SerializeField] private float delay = 3f;
    [SerializeField] private bool skipWithAnyKey = true;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= delay || (skipWithAnyKey && timer > 0.5f && Input.anyKeyDown))
        {
            enabled = false;
            SceneManager.LoadScene(sceneName);
        }
    }
}
