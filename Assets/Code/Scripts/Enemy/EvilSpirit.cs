using System.Collections;
using UnityEngine;

// Hovers until you get close, then chases you through walls. A bit slower than you, takes 3 salt hits, gives up if you get far enough away.
public class EvilSpirit : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameTime = 0.12f;
    [SerializeField] private float sightRange = 6f;
    [SerializeField] private float giveUpRange = 11f;
    [SerializeField] private float speed = 3.4f;
    [SerializeField] private int health = 3;
    [SerializeField] private Vector2 hitSize = new Vector2(0.7f, 0.8f);

    private SpriteRenderer body;
    private PlayerHealth player;
    private Vector3 home;
    private bool chasing;
    private bool dead;
    private float stunned;

    private IEnumerator Start()
    {
        body = GetComponent<SpriteRenderer>();
        player = CarriedPlayer();
        home = transform.position;

        while (!dead)
        {
            Animate();
            if (player == null || player.isDead)
            {
                chasing = false;
            }
            else
            {
                float d = Vector2.Distance(player.transform.position, transform.position);
                if (!chasing && d < sightRange)
                {
                    yield return Notice();
                    chasing = true;
                }
                else if (chasing && d > giveUpRange)
                {
                    chasing = false;
                }
            }

            if (stunned > 0f)
            {
                stunned -= Time.deltaTime;
            }
            else if (chasing && !dead)
            {
                Vector3 target = player.transform.position + Vector3.up * 0.4f;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                body.flipX = target.x < transform.position.x;
                if (Touching())
                {
                    player.TakeDamage(1, transform.position);
                    yield return Recoil(1.6f);
                }
            }
            else
            {
                // drift home and bob
                Vector3 bob = home + Vector3.up * Mathf.Sin(Time.time * 2f) * 0.15f;
                transform.position = Vector3.MoveTowards(transform.position, bob, speed * 0.5f * Time.deltaTime);
            }
            yield return null;
        }
    }

    // shiver and moan so you get a beat to react
    private IEnumerator Notice()
    {
        Sound.Play("ghostMoan", 0.9f);
        Vector3 p = transform.position;
        for (float t = 0f; t < 0.5f && !dead; t += Time.deltaTime)
        {
            transform.position = p + (Vector3)(Random.insideUnitCircle * 0.06f);
            Animate();
            yield return null;
        }
        transform.position = p;
    }

    private IEnumerator Recoil(float distance)
    {
        Vector3 away = (transform.position - player.transform.position).normalized;
        Vector3 end = transform.position + away * distance;
        for (float t = 0f; t < 0.25f && !dead; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(transform.position, end, t / 0.25f);
            Animate();
            yield return null;
        }
        stunned = 0.8f;
    }

    public bool TakeSalt()
    {
        if (dead)
        {
            return false;
        }
        health--;
        if (health <= 0)
        {
            dead = true;
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(Flash());
            stunned = 0.6f;
            if (player != null)
            {
                StartCoroutine(Recoil(0.8f));
            }
        }
        return true;
    }

    private IEnumerator Flash()
    {
        body.color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.12f);
        body.color = Color.white;
    }

    private IEnumerator Die()
    {
        Sound.Play("ghostMoan", 0.7f);
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        for (float t = 0f; t < 1f; t += Time.deltaTime)
        {
            transform.position += Vector3.up * Time.deltaTime * 1.2f;
            body.color = new Color(1f, 1f, 1f, 1f - t);
            Animate();
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void Animate()
    {
        if (frames != null && frames.Length > 0)
        {
            body.sprite = frames[(int)(Time.time / frameTime) % frames.Length];
        }
    }

    private bool Touching()
    {
        Bounds hit = new Bounds(transform.position, new Vector3(hitSize.x, hitSize.y, 10f));
        foreach (Collider2D c in player.GetComponents<Collider2D>())
        {
            if (!c.isTrigger && c.enabled && c.bounds.Intersects(hit))
            {
                return true;
            }
        }
        return false;
    }

    // the player carried over from earlier scenes, not this scene's placeholder copy
    private static PlayerHealth CarriedPlayer()
    {
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                return p;
            }
        }
        return FindAnyObjectByType<PlayerHealth>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
