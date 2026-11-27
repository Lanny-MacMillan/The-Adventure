using UnityEngine;

// A heart hidden up in the treetops: gives one back. Left alone while you're at full health, back every time you load the map.
public class HeartPickup : MonoBehaviour
{
    [SerializeField] private float reach = 0.5f;

    private PlayerHealth player;
    private SpriteRenderer body;
    private Vector3 home;
    private bool taken;

    private void Start()
    {
        home = transform.position;
        body = GetComponent<SpriteRenderer>();
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
        if (taken)
        {
            return;
        }
        transform.position = home + Vector3.up * Mathf.Sin(Time.time * 3f) * 0.08f;
        // faded while you're at full health so it's clear it isn't doing anything yet
        bool full = player == null || player.health >= player.maxHealth;
        if (body != null)
        {
            body.color = new Color(1f, 1f, 1f, full ? 0.35f : 1f);
        }
        if (full || player.isDead)
        {
            return;
        }
        foreach (Collider2D c in player.GetComponents<Collider2D>())
        {
            if (!c.isTrigger && c.enabled && c.bounds.SqrDistance(transform.position) < reach * reach)
            {
                player.health++;
                Sound.Play("keyPickup");
                taken = true;
                StartCoroutine(Pop());
                return;
            }
        }
    }

    private System.Collections.IEnumerator Pop()
    {
        Vector3 start = transform.localScale;
        for (float t = 0f; t < 0.3f; t += Time.deltaTime)
        {
            transform.localScale = start * (1f + t * 2f);
            transform.position += Vector3.up * Time.deltaTime;
            if (body != null)
            {
                body.color = new Color(1f, 1f, 1f, 1f - t / 0.3f);
            }
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
