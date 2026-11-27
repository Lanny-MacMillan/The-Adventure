using UnityEngine;

// A tossed handful of salt: arcs out of your hand, hits the first enemy it touches, and is gone when it lands.
public class SaltBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float life = 2f; // only if it never lands
    public PlayerMovement playerMovement;

    // set when thrown so turning around doesn't steer salt already in the air
    public bool facingRight = true;

    private bool spent;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // the puff starts at the left of its frame, so mirror it to come out of the hand when throwing left
            sr.flipX = !facingRight;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 6;
            // carrying holy water blesses the salt, which cures Frank instead of hurting him
            if (PlayerPrefs.GetInt("holyWater") == 1)
            {
                sr.color = new Color(0.55f, 0.85f, 1f, 1f);
            }
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.linearVelocity = new Vector2(facingRight ? speed : -speed, 0f);
        }
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spent || other.CompareTag("Player"))
        {
            return;
        }

        EnemyGhost ghost = other.GetComponentInParent<EnemyGhost>();
        if (ghost != null)
        {
            if (ghost.TakeDamage(1))
            {
                Hit();
            }
            return;
        }
        FrankBoss frank = other.GetComponentInParent<FrankBoss>();
        if (frank != null && frank.TakeSalt(1))
        {
            Hit();
            return;
        }
        EvilSpirit spirit = other.GetComponentInParent<EvilSpirit>();
        if (spirit != null && spirit.TakeSalt())
        {
            Hit();
            return;
        }
        AtticBat bat = other.GetComponentInParent<AtticBat>();
        if (bat != null && bat.TakeSalt())
        {
            Hit();
            return;
        }

        // lands on floors, branches and walls
        if (!other.isTrigger)
        {
            spent = true;
            Destroy(gameObject);
        }
    }

    private void Hit()
    {
        spent = true;
        Sound.Play("saltHit");
        Destroy(gameObject);
    }
}
