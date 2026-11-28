using UnityEngine;

// walk into it to change scenes (e.g. the attic floor dropping into the bedroom)
[RequireComponent(typeof(Collider2D))]
public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private bool triggered;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || other.GetComponentInParent<PlayerHealth>() == null)
        {
            return;
        }
        triggered = true;

        SceneSwitch sceneSwitch = FindAnyObjectByType<SceneSwitch>();
        if (sceneSwitch != null)
        {
            sceneSwitch.SwitchScene(sceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
