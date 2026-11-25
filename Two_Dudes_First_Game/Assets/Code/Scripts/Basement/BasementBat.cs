using UnityEngine;

// Hangs in the dark until it sees you, then weaves in for a bite and flaps back up. One salt hit kills it.
public class BasementBat : BasementEnemy
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer eyes;
    [SerializeField] private Sprite hangSprite;
    [SerializeField] private Sprite[] flyFrames;
    [SerializeField] private float sightRange = 7f;
    [SerializeField] private float diveSpeed = 4.2f;
    [SerializeField] private float restTime = 1.6f;
    [SerializeField] private float touchRange = 0.55f;

    private Vector3 roost;
    private bool awake;
    private bool diving;
    private bool dead;
    private float rest;
    private float t;
    private float fallSpeed;

    private void Start()
    {
        roost = transform.position;
        body.sprite = hangSprite;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        if (dead)
        {
            // drops to the floor and fades
            fallSpeed += 12f * Time.deltaTime;
            Vector3 d = transform.position;
            d.y = Mathf.Max(0.25f, d.y - fallSpeed * Time.deltaTime);
            transform.position = d;
            Color c = body.color;
            c.a -= Time.deltaTime;
            body.color = c;
            if (c.a <= 0f)
            {
                gameObject.SetActive(false);
            }
            return;
        }

        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (p == null || p.Health == null || p.Health.isDead)
        {
            return;
        }
        t += Time.deltaTime;
        if (!awake && FlatDistance(transform.position, p.transform.position) < sightRange && BasementSight.Clear(transform.position, p.transform.position))
        {
            awake = true;
            diving = true;
        }
        if (!awake)
        {
            BasementSight.Shade(body, transform.position, 0f, Color.white);
            return;
        }

        body.sprite = flyFrames[(int)(t / 0.08f) % flyFrames.Length];
        Vector3 target = diving ? p.transform.position - Vector3.up * 0.15f : roost;
        Vector3 pos = Vector3.MoveTowards(transform.position, target, diveSpeed * Time.deltaTime);
        // zig-zag so it's harder to hit
        Vector3 side = Vector3.Cross(Vector3.up, (target - transform.position).normalized);
        pos += side * Mathf.Sin(t * 9f) * 1.6f * Time.deltaTime;
        transform.position = pos;

        if (diving && Vector3.Distance(pos, target) < touchRange)
        {
            p.Hurt(transform.position);
            diving = false;
        }
        else if (!diving && Vector3.Distance(pos, roost) < 0.1f)
        {
            rest += Time.deltaTime;
            body.sprite = hangSprite;
            if (rest > restTime)
            {
                rest = 0f;
                diving = BasementSight.Clear(transform.position, p.transform.position) && FlatDistance(transform.position, p.transform.position) < sightRange;
            }
        }
        BasementSight.Shade(body, transform.position, 0f, Color.white);
    }

    public override bool TakeSalt(bool holy)
    {
        if (dead)
        {
            return false;
        }
        dead = true;
        All.Remove(this);
        body.sprite = flyFrames[0];
        body.flipY = true;
        if (eyes != null)
        {
            eyes.enabled = false;
        }
        return true;
    }
}
