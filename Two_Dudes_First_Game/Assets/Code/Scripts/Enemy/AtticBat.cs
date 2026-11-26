using System.Collections;
using UnityEngine;

// Attic: hangs from the rafters, wakes when you walk near and swoops at where you're heading. One salt hit kills it.
public class AtticBat : MonoBehaviour
{
    [SerializeField] private Sprite hangSprite;
    [SerializeField] private Sprite[] flyFrames;
    [SerializeField] private SpriteRenderer eyes;
    [SerializeField] private float hangEyesY = -0.2f;
    [SerializeField] private float wakeRange = 3.2f;
    [SerializeField] private float wakeTime = 0.4f;
    [SerializeField] private float swoopTime = 1.1f;
    [SerializeField] private float restTime = 2.5f;
    [SerializeField] private float lead = 0.35f;
    [SerializeField] private float roomLeft = 29f;
    [SerializeField] private float roomRight = 59f;
    [SerializeField] private float floorY = -8.3f;
    [SerializeField] private Vector2 hitSize = new Vector2(0.8f, 0.5f);

    private SpriteRenderer body;
    private PlayerHealth player;
    private Rigidbody2D playerBody;
    private float roostY;
    private bool swooping;
    private bool dead;

    private IEnumerator Start()
    {
        body = GetComponent<SpriteRenderer>();
        player = CarriedPlayer();
        playerBody = player != null ? player.GetComponent<Rigidbody2D>() : null;
        roostY = transform.position.y;
        Hang();

        while (!dead)
        {
            yield return null;
            if (player == null || player.isDead || Mathf.Abs(player.transform.position.x - transform.position.x) > wakeRange)
            {
                continue;
            }

            // shiver so you get a split second of warning
            Vector3 roost = transform.position;
            Sound.Play("batScreech", 0.7f);
            for (float t = 0f; t < wakeTime; t += Time.deltaTime)
            {
                transform.position = roost + (Vector3)(Random.insideUnitCircle * 0.05f);
                eyes.enabled = Mathf.Repeat(t, 0.1f) < 0.05f;
                yield return null;
            }
            transform.position = roost;
            eyes.enabled = true;

            Sound.Play("batFlap", 0.6f);
            yield return Swoop();
            if (!dead)
            {
                yield return new WaitForSeconds(restTime);
            }
        }
    }

    // a U-shaped dive that bottoms out where you'll be, then back up to the rafters on the other side
    private IEnumerator Swoop()
    {
        Vector2 aim = player.transform.position;
        if (playerBody != null)
        {
            aim.x += playerBody.linearVelocity.x * lead;
        }
        Vector2 p0 = transform.position;
        Vector2 p2 = new Vector2(Mathf.Clamp(2f * aim.x - p0.x, roomLeft, roomRight), roostY);
        Vector2 p1 = 2f * aim - (p0 + p2) / 2f;

        swooping = true;
        body.flipX = p2.x > p0.x;
        eyes.transform.localPosition = Vector3.zero;
        for (float t = 0f; t < swoopTime && !dead; t += Time.deltaTime)
        {
            float u = t / swoopTime;
            transform.position = (1 - u) * (1 - u) * p0 + 2 * (1 - u) * u * p1 + u * u * p2;
            body.sprite = flyFrames[(int)(t / 0.08f) % flyFrames.Length];
            if (Touching())
            {
                player.TakeDamage(1, transform.position);
            }
            yield return null;
        }
        swooping = false;
        if (!dead)
        {
            transform.position = p2;
            Hang();
        }
    }

    public bool TakeSalt()
    {
        if (dead)
        {
            return false;
        }
        dead = true;
        StopAllCoroutines();
        StartCoroutine(Fall());
        return true;
    }

    private IEnumerator Fall()
    {
        swooping = false;
        eyes.enabled = false;
        body.sprite = flyFrames[0];
        body.flipY = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        float vy = 0f;
        float bottom = floorY + body.bounds.extents.y;
        while (transform.position.y > bottom)
        {
            vy -= 20f * Time.deltaTime;
            transform.position += new Vector3(0f, vy * Time.deltaTime, 0f);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, bottom, transform.position.z);

        Color c = body.color;
        for (float t = 0f; t < 1f; t += Time.deltaTime)
        {
            body.color = new Color(c.r, c.g, c.b, 1f - t);
            yield return null;
        }
        gameObject.SetActive(false);
    }

    private void Hang()
    {
        body.sprite = hangSprite;
        body.flipX = false;
        eyes.transform.localPosition = new Vector3(0f, hangEyesY, 0f);
    }

    private bool Touching()
    {
        if (!swooping)
        {
            return false;
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + Vector3.left * wakeRange, transform.position + Vector3.right * wakeRange);
    }
}
