using UnityEngine;

// Keeps this light on the carried-over player, for dark rooms.
public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.3f, 0f);

    private Transform target;

    private void LateUpdate()
    {
        if (target == null)
        {
            foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
            {
                if (target == null || p.gameObject.scene.name == "DontDestroyOnLoad")
                {
                    target = p.transform;
                }
            }
            if (target == null)
            {
                return;
            }
        }
        transform.position = target.position + offset;
    }
}
