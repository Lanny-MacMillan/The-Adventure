using UnityEngine;

// Carries this object (player, HUD, inventory) from scene to scene. Each room has its own copy for testing;
// when one with the same name was already carried in, the room's copy switches off and deletes itself right away
// so nothing in the room can mistake it for the real one.
[DefaultExecutionOrder(-1000)]
public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        if (transform.parent != null)
        {
            return; // children ride along with their carried parent
        }
        foreach (DontDestroy other in FindObjectsByType<DontDestroy>(FindObjectsInactive.Include))
        {
            if (other != this && other.name == name && other.gameObject.scene.name == "DontDestroyOnLoad")
            {
                gameObject.SetActive(false);
                Destroy(gameObject);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
