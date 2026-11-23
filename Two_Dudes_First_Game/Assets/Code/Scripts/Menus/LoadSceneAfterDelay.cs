using UnityEngine;
using UnityEngine.SceneManagement;

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
