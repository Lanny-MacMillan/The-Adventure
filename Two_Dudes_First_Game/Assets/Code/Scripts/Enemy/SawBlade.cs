using UnityEngine;

// Runs back and forth along a slot in the floor between two x positions. Can't be killed, jump it. Grinds louder the closer you are.
public class SawBlade : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameTime = 0.05f;
    [SerializeField] private float leftX = -2f;
    [SerializeField] private float rightX = 2f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float pause = 0.4f;
    [SerializeField] private float hitRadius = 0.3f;
    [SerializeField] private float hearRange = 9f;

    private SpriteRenderer body;
    private AudioSource grind;
    private PlayerHealth player;
    private int dir = 1;
    private float wait;

    private void Start()
    {
        body = GetComponent<SpriteRenderer>();
        grind = GetComponent<AudioSource>();
        player = CarriedPlayer();
    }

    private void Update()
    {
        if (frames != null && frames.Length > 0)
        {
            body.sprite = frames[(int)(Time.time / frameTime) % frames.Length];
        }
        transform.Rotate(0f, 0f, -dir * 720f * Time.deltaTime);

        if (wait > 0f)
        {
            wait -= Time.deltaTime;
        }
        else
        {
            Vector3 p = transform.position;
            p.x += dir * speed * Time.deltaTime;
            if (p.x >= rightX || p.x <= leftX)
            {
                p.x = Mathf.Clamp(p.x, leftX, rightX);
                dir = -dir;
                wait = pause;
            }
            transform.position = p;
        }

        if (player == null)
        {
            return;
        }
        float d = Vector2.Distance(player.transform.position, transform.position);
        if (grind != null)
        {
            grind.volume = Mathf.Clamp01(1f - d / hearRange) * 0.5f * Sound.SfxVolume;
        }
        if (!player.isDead && Touching())
        {
            player.TakeDamage(1, transform.position);
        }
    }

    private bool Touching()
    {
        foreach (Collider2D c in player.GetComponents<Collider2D>())
        {
            if (!c.isTrigger && c.enabled && c.bounds.SqrDistance(transform.position) < hitRadius * hitRadius)
            {
                return true;
            }
        }
        return false;
    }

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
        Gizmos.color = Color.red;
        Vector3 p = transform.position;
        Gizmos.DrawLine(new Vector3(leftX, p.y), new Vector3(rightX, p.y));
    }
}
