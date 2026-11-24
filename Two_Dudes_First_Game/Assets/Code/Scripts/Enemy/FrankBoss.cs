using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Frank / Possessed Matt, the Treehouse boss. Spits, belly-flops so goo rains from the ceiling, and gets meaner as he's hurt.
public class FrankBoss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 45;
    [SerializeField] private string defeatedFlag = "frankDefeated";

    [Header("Look")]
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Sprite calmSprite;
    [SerializeField] private Sprite possessedSprite;
    [SerializeField] private Sprite gooSprite;
    [SerializeField] private float gooScale = 8f; // the spit sprite is a small ball in a 32px image
    [SerializeField] private Color gooColor = new Color(0.55f, 1f, 0.35f, 1f);
    [SerializeField] private Color hitColor = new Color(1f, 0.3f, 0.3f, 1f);
    [SerializeField] private Color enragedColor = new Color(0.7f, 1f, 0.6f, 1f);

    [Header("Room")]
    [SerializeField] private float roomLeft = -3.3f;
    [SerializeField] private float roomRight = 3.5f;
    [SerializeField] private float ceilingY = 3.1f;

    [Header("Scene")]
    [SerializeField] private GameObject healthBar;
    [SerializeField] private RectTransform healthFill;
    [SerializeField] private GameObject reward; // key spawner, inactive until he's beaten
    [SerializeField] private Behaviour exitDoor; // locked during the fight

    private int health;
    private bool fighting;
    private bool dead;
    private float floorY;   // top of the floor
    private float feet;     // from his pivot down to his feet
    private Color tint = Color.white;
    private Transform cam;
    private Vector3 camHome;
    private Vector3 baseScale;
    private readonly List<GameObject> hazards = new List<GameObject>();
    private PlayerHealth player;

    private int Phase { get { return health > maxHealth * 2 / 3 ? 1 : health > maxHealth / 3 ? 2 : 3; } }
    private float Speed { get { return Phase == 3 ? 1.35f : Phase == 2 ? 1.15f : 1f; } }

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }
        if (PlayerPrefs.GetInt(defeatedFlag) == 1)
        {
            if (reward != null)
            {
                reward.SetActive(true);
            }
            gameObject.SetActive(false);
            return;
        }

        health = maxHealth;
        player = CarriedPlayer();
        baseScale = transform.localScale;
        body.sprite = calmSprite;
        floorY = FindFloor(transform.position.x);
        feet = transform.position.y - floorY;
        cam = Camera.main != null ? Camera.main.transform : null;
        if (cam != null)
        {
            camHome = cam.position;
        }
        StartCoroutine(Fight());
    }

    // you died: stop and get off the Game Over screen
    private void Update()
    {
        if (!dead && healthBar != null && healthBar.activeSelf && player != null && player.isDead)
        {
            StopAllCoroutines();
            fighting = false;
            healthBar.SetActive(false);
            if (cam != null)
            {
                cam.position = camHome;
            }
        }
    }

    // salt calls this; false if he can't be hurt right now
    public bool TakeSalt(int damage)
    {
        if (!fighting || dead)
        {
            return false;
        }
        health = Mathf.Max(0, health - damage);
        UpdateBar();
        StartCoroutine(HitFlash());
        if (health == 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
        return true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!fighting || dead)
        {
            return;
        }
        PlayerHealth hit = other.GetComponentInParent<PlayerHealth>();
        if (hit == null || hit.isDead)
        {
            return;
        }
        // pinned against a wall: bounce over him instead of into the wall
        float px = hit.transform.position.x;
        Vector2 from = transform.position;
        if (px <= transform.position.x && px < roomLeft + 0.4f)
        {
            from = new Vector2(px - 1f, hit.transform.position.y);
        }
        else if (px >= transform.position.x && px > roomRight - 0.4f)
        {
            from = new Vector2(px + 1f, hit.transform.position.y);
        }
        hit.TakeDamage(1, from);
    }

    private IEnumerator Fight()
    {
        yield return new WaitForSeconds(1f);

        // Matt twitches, then the thing inside him wakes up
        float homeX = transform.position.x;
        for (float t = 0f; t < 0.9f; t += Time.deltaTime)
        {
            transform.position = new Vector3(homeX + Random.Range(-0.04f, 0.04f), Ground(), transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(homeX, Ground(), transform.position.z);
        body.sprite = possessedSprite;
        StartCoroutine(Shake(0.35f, 0.15f));
        StartCoroutine(Squash(1.25f, 0.8f, 0.25f));
        if (exitDoor != null)
        {
            exitDoor.enabled = false;
        }
        if (healthBar != null)
        {
            healthBar.SetActive(true);
        }
        UpdateBar();
        yield return new WaitForSeconds(0.8f);
        fighting = true;
        yield return new WaitForSeconds(0.7f);

        int lastAttack = -1;
        int repeats = 0;
        while (!dead)
        {
            if (PlayerDead())
            {
                yield break;
            }
            if (Phase == 3 && tint != enragedColor)
            {
                tint = enragedColor;
                body.color = tint;
            }

            int attack = Random.Range(0, 2);
            if (attack == lastAttack && ++repeats >= 2)
            {
                attack = 1 - attack;
            }
            if (attack != lastAttack)
            {
                repeats = 0;
            }
            lastAttack = attack;

            if (attack == 0)
            {
                yield return Spit();
            }
            else
            {
                yield return Slam();
                if (Phase == 3 && !dead)
                {
                    yield return Slam();
                }
            }

            // breather where he shuffles toward you
            float rest = Phase == 1 ? 1.1f : Phase == 2 ? 0.8f : 0.5f;
            for (float t = 0f; t < rest; t += Time.deltaTime)
            {
                Transform player = Player();
                if (player != null)
                {
                    float x = Mathf.MoveTowards(transform.position.x, player.position.x, 0.9f * Speed * Time.deltaTime);
                    transform.position = new Vector3(Clamp(x), Ground(), transform.position.z);
                    Face(player.position.x);
                }
                yield return null;
            }
        }
    }

    private IEnumerator Spit()
    {
        Transform player = Player();
        if (player == null)
        {
            yield break;
        }
        Face(player.position.x);

        // windup: puff up and go green
        body.color = gooColor;
        yield return Squash(0.85f, 1.2f, 0.45f / Speed);
        body.color = tint;

        player = Player();
        if (player == null)
        {
            yield break;
        }
        Vector2 mouth = MouthPosition();
        if (Phase == 1 || Phase == 3)
        {
            Vector2 aim = ((Vector2)player.position - mouth).normalized;
            SpawnGoo(mouth, aim * 6.5f, 0f, 0.45f, false);
        }
        if (Phase >= 2)
        {
            if (Phase == 3)
            {
                yield return new WaitForSeconds(0.25f);
            }
            float[] offsets = { 0f, -1.3f, 1.3f };
            foreach (float off in offsets)
            {
                Lob(mouth, Clamp(player.position.x + off), 0.95f / Speed);
            }
        }
        yield return Squash(1.1f, 0.9f, 0.15f);
        yield return new WaitForSeconds(0.3f / Speed);
    }

    private IEnumerator Slam()
    {
        Transform player = Player();
        if (player == null)
        {
            yield break;
        }
        Face(player.position.x);

        // crouch
        yield return Squash(1.25f, 0.7f, 0.4f / Speed);

        player = Player();
        float startX = transform.position.x;
        float targetX = Clamp(player != null ? player.position.x : startX);
        float air = 0.8f / Speed;
        float height = 2.2f;
        transform.localScale = new Vector3(baseScale.x * 0.85f * Mathf.Sign(transform.localScale.x), baseScale.y * 1.2f, baseScale.z);
        for (float t = 0f; t < air; t += Time.deltaTime)
        {
            float k = t / air;
            float x = Mathf.Lerp(startX, targetX, k);
            float y = Ground() + 4f * height * k * (1f - k);
            transform.position = new Vector3(x, y, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(targetX, Ground(), transform.position.z);

        // land: shake, goo off the ceiling, shockwave later on
        StartCoroutine(Shake(0.25f, 0.18f));
        StartCoroutine(Squash(1.35f, 0.65f, 0.2f));
        if (Phase >= 2)
        {
            Shockwave(-1f);
            Shockwave(1f);
        }
        int drips = Phase == 1 ? 3 : Phase == 2 ? 5 : 6;
        DropGoo(drips);
        yield return new WaitForSeconds(0.5f / Speed);
    }

    private void DropGoo(int count)
    {
        Transform player = Player();
        List<float> xs = new List<float>();
        if (player != null)
        {
            xs.Add(Clamp(player.position.x));
        }
        int tries = 0;
        while (xs.Count < count && tries++ < 50)
        {
            float x = Random.Range(roomLeft, roomRight);
            bool clear = true;
            foreach (float other in xs)
            {
                if (Mathf.Abs(other - x) < 0.8f)
                {
                    clear = false;
                    break;
                }
            }
            if (clear)
            {
                xs.Add(x);
            }
        }
        for (int i = 0; i < xs.Count; i++)
        {
            StartCoroutine(Drip(xs[i], 0.1f * i));
        }
    }

    private IEnumerator Drip(float x, float delay)
    {
        yield return new WaitForSeconds(delay);
        FrankHazard goo = SpawnGoo(new Vector2(x, ceilingY), Vector2.zero, 0f, 0.4f, false);
        goo.enabled = false;
        Collider2D col = goo.GetComponent<Collider2D>();
        col.enabled = false;

        // swell on the ceiling so you can see it coming
        float grow = 0.7f / Speed;
        for (float t = 0f; t < grow && goo != null; t += Time.deltaTime)
        {
            float k = t / grow;
            goo.transform.localScale = new Vector3(0.4f * (0.5f + 0.5f * k), 0.4f * (0.3f + 0.9f * k), 1f) * gooScale;
            goo.transform.position = new Vector3(x + Mathf.Sin(t * 40f) * 0.02f, ceilingY - 0.1f * k, 0f);
            yield return null;
        }
        if (goo != null)
        {
            goo.transform.localScale = new Vector3(0.35f, 0.5f, 1f) * gooScale;
            goo.gravity = 18f;
            goo.enabled = true;
            col.enabled = true;
        }
    }

    private void Shockwave(float dir)
    {
        FrankHazard wave = SpawnGoo(new Vector2(transform.position.x + dir * 0.4f, floorY + 0.12f), new Vector2(dir * 5f * Speed, 0f), 0f, 0.5f, true);
        wave.transform.localScale = new Vector3(0.9f, 0.45f, 1f) * gooScale;
        wave.spin = 0f;
    }

    private void Lob(Vector2 from, float targetX, float time)
    {
        float g = 14f;
        float vx = (targetX - from.x) / time;
        float vy = (floorY - from.y + 0.5f * g * time * time) / time;
        SpawnGoo(from, new Vector2(vx, vy), g, 0.4f, false);
    }

    private FrankHazard SpawnGoo(Vector2 pos, Vector2 velocity, float gravity, float size, bool rolls)
    {
        GameObject g = new GameObject("FrankGoo");
        g.transform.position = new Vector3(pos.x, pos.y, 0f);
        g.transform.localScale = new Vector3(size, size, 1f) * gooScale;
        SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
        sr.sprite = gooSprite;
        sr.sortingOrder = 5;
        Rigidbody2D rb = g.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        CircleCollider2D col = g.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.055f; // just the visible ball
        FrankHazard h = g.AddComponent<FrankHazard>();
        h.velocity = velocity;
        h.gravity = gravity;
        h.floorY = floorY + 0.08f;
        h.left = roomLeft - 0.6f;
        h.right = roomRight + 0.6f;
        h.rollsOnFloor = rolls;
        h.spin = rolls ? 0f : 360f;
        hazards.Add(g);
        return h;
    }

    private IEnumerator Die()
    {
        dead = true;
        fighting = false;
        foreach (GameObject g in hazards)
        {
            if (g != null)
            {
                Destroy(g);
            }
        }
        transform.position = new Vector3(transform.position.x, Ground(), transform.position.z);
        StartCoroutine(Shake(0.6f, 0.12f));

        // flicker, then melt into a puddle
        for (float t = 0f; t < 1f; t += Time.deltaTime)
        {
            body.color = (Mathf.FloorToInt(t / 0.07f) % 2 == 0) ? hitColor : gooColor;
            transform.position = new Vector3(transform.position.x + Random.Range(-0.03f, 0.03f), Ground(), transform.position.z);
            yield return null;
        }
        body.color = gooColor;
        Vector3 s = transform.localScale;
        for (float t = 0f; t < 1.4f; t += Time.deltaTime)
        {
            float k = t / 1.4f;
            transform.localScale = new Vector3(s.x * (1f + 0.8f * k), s.y * (1f - 0.92f * k), s.z);
            transform.position = new Vector3(transform.position.x, Ground(), transform.position.z);
            yield return null;
        }

        PlayerPrefs.SetInt(defeatedFlag, 1);
        PlayerPrefs.Save();
        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }
        if (exitDoor != null)
        {
            exitDoor.enabled = true;
        }
        if (reward != null)
        {
            reward.transform.position = new Vector3(Clamp(transform.position.x), reward.transform.position.y, reward.transform.position.z);
            reward.SetActive(true);
        }
        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator HitFlash()
    {
        body.color = hitColor;
        yield return new WaitForSeconds(0.08f);
        if (!dead)
        {
            body.color = tint;
        }
    }

    private IEnumerator Squash(float x, float y, float time)
    {
        float sign = Mathf.Sign(transform.localScale.x);
        transform.localScale = new Vector3(baseScale.x * x * sign, baseScale.y * y, baseScale.z);
        transform.position = new Vector3(transform.position.x, Ground(), transform.position.z);
        yield return new WaitForSeconds(time);
        transform.localScale = new Vector3(baseScale.x * Mathf.Sign(transform.localScale.x), baseScale.y, baseScale.z);
        transform.position = new Vector3(transform.position.x, Ground(), transform.position.z);
    }

    private IEnumerator Shake(float time, float strength)
    {
        if (cam == null)
        {
            yield break;
        }
        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            float k = 1f - t / time;
            cam.position = camHome + (Vector3)(Random.insideUnitCircle * strength * k);
            yield return null;
        }
        cam.position = camHome;
    }

    private void UpdateBar()
    {
        if (healthFill != null)
        {
            healthFill.localScale = new Vector3((float)health / maxHealth, 1f, 1f);
        }
    }

    private void OnDisable()
    {
        if (cam != null)
        {
            cam.position = camHome;
        }
    }

    // where his pivot sits when he's standing on the floor at his current size
    private float Ground()
    {
        return floorY + feet * (transform.localScale.y / (baseScale.y == 0f ? 1f : baseScale.y));
    }

    private void Face(float x)
    {
        float sign = x < transform.position.x ? -1f : 1f;
        // the art faces left
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -sign, transform.localScale.y, transform.localScale.z);
    }

    private Vector2 MouthPosition()
    {
        float dir = transform.localScale.x < 0 ? 1f : -1f;
        return new Vector2(transform.position.x + dir * 0.35f, body.bounds.center.y + body.bounds.extents.y * 0.35f);
    }

    private float Clamp(float x)
    {
        return Mathf.Clamp(x, roomLeft, roomRight);
    }

    private float FindFloor(float x)
    {
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(new Vector2(x, transform.position.y + 0.5f), Vector2.down, 5f))
        {
            if (hit.collider.isTrigger || hit.collider.transform.IsChildOf(transform) || hit.collider.GetComponentInParent<PlayerHealth>() != null)
            {
                continue;
            }
            return hit.point.y;
        }
        return body.bounds.min.y;
    }

    private Transform Player()
    {
        return player != null && !player.isDead ? player.transform : null;
    }

    private bool PlayerDead()
    {
        return player == null || player.isDead;
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
        return FindAnyObjectByType<PlayerHealth>(); // Play started right in this scene
    }
}
