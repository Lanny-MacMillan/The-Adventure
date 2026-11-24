using UnityEngine;

// Kitchen: shows up behind you, only moves while your back is turned. Touching him hurts, so jump over him to get out.
public class SkinnyDude : MonoBehaviour
{
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float appearX = 29.9f;
    [SerializeField] private float speed = 2.2f;
    [SerializeField] private float wakeDelay = 0.35f;
    [SerializeField] private float recoilDistance = 2.2f;
    [SerializeField] private float roomLeft = 27.9f;
    [SerializeField] private float roomRight = 36.2f;
    [SerializeField] private Vector2 hitSize = new Vector2(0.45f, 1.4f);
    [SerializeField] private float stillFrameTime = 10f;
    [SerializeField] private float moveFrameTime = 0.07f;
    [SerializeField] private string doneFlag = "bedroomDoor";

    private SpriteRenderer body;
    private PlayerHealth player;
    private PlayerMovement playerMove;
    private bool appeared;
    private bool passive;
    private float unseenFor;
    private float frameTimer;
    private int frame;
    private float baseX;

    private void Start()
    {
        body = GetComponent<SpriteRenderer>();
        player = CarriedPlayer();
        playerMove = player != null ? player.GetComponent<PlayerMovement>() : null;
        baseX = transform.position.x;

        // already have the key: he just stands there and stares
        passive = PlayerPrefs.GetInt(doneFlag) == 1;
        appeared = passive;
        body.enabled = appeared;
    }

    private void Update()
    {
        if (player == null || player.isDead || Time.timeScale == 0f)
        {
            return;
        }

        bool watched = Watched();
        if (!appeared)
        {
            if (player.transform.position.x < appearX && !watched)
            {
                appeared = true;
                body.enabled = true;
            }
            return;
        }

        unseenFor = watched ? 0f : unseenFor + Time.deltaTime;
        bool moving = !passive && unseenFor > wakeDelay;

        if (moving)
        {
            baseX = Mathf.MoveTowards(baseX, Mathf.Clamp(player.transform.position.x, roomLeft, roomRight), speed * Time.deltaTime);
        }
        // twitchy while he moves, dead still the moment you look
        float jitter = moving ? Random.Range(-0.04f, 0.04f) : 0f;
        transform.position = new Vector3(baseX + jitter, transform.position.y, transform.position.z);
        Animate(moving);

        if (!passive && Touching())
        {
            player.TakeDamage(1, transform.position);
            Recoil();
        }
    }

    // snaps back toward whichever side has more room so you're not pinned in a corner
    private void Recoil()
    {
        float px = player.transform.position.x;
        float dir = (roomRight - px) > (px - roomLeft) ? 1f : -1f;
        baseX = Mathf.Clamp(px + dir * recoilDistance, roomLeft, roomRight);
        unseenFor = 0f;
        transform.position = new Vector3(baseX, transform.position.y, transform.position.z);
    }

    private bool Watched()
    {
        if (playerMove == null)
        {
            return false;
        }
        bool heIsRight = transform.position.x >= player.transform.position.x;
        return heIsRight == playerMove.isFacingRight;
    }

    private void Animate(bool moving)
    {
        if (frames == null || frames.Length == 0)
        {
            return;
        }
        frameTimer += Time.deltaTime;
        if (frameTimer >= (moving ? moveFrameTime : stillFrameTime))
        {
            frameTimer = 0f;
            frame = (frame + 1) % frames.Length;
            body.sprite = frames[frame];
        }
    }

    private bool Touching()
    {
        Bounds hit = new Bounds(new Vector3(baseX, body.bounds.min.y + hitSize.y / 2f, 0f), new Vector3(hitSize.x, hitSize.y, 10f));
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
        Gizmos.color = Color.red;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float bottom = sr != null ? sr.bounds.min.y : transform.position.y;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, bottom + hitSize.y / 2f, 0f), hitSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(appearX, bottom, 0f), new Vector3(appearX, bottom + 3f, 0f));
    }
}
