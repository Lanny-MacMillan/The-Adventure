using UnityEngine;

// A heart hidden up in the treetops: gives one back. Left alone while you're at full health, back every time you load the map.
public class HeartPickup : MonoBehaviour
{
    [SerializeField] private float reach = 0.5f;

    private PlayerHealth player;
    private Vector3 home;

    private void Start()
    {
        home = transform.position;
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                player = p;
            }
        }
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerHealth>();
        }
    }

    private void Update()
    {
        transform.position = home + Vector3.up * Mathf.Sin(Time.time * 3f) * 0.08f;
        if (player == null || player.isDead || player.health >= player.maxHealth)
        {
            return;
        }
        foreach (Collider2D c in player.GetComponents<Collider2D>())
        {
            if (!c.isTrigger && c.enabled && c.bounds.SqrDistance(transform.position) < reach * reach)
            {
                player.health++;
                Sound.Play("keyPickup");
                gameObject.SetActive(false);
                return;
            }
        }
    }
}
