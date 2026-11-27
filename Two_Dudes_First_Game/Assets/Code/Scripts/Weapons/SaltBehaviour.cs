using UnityEngine;

// A thrown handful of salt: flies straight out of your hand, hits the first enemy it touches, breaks on walls and floors, and is gone once the puff fades.
public class SaltBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float life = 0.6f;
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
            rb.gravityScale = 0f;
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

        // walls stop it; the floor under it, one-way tree branches and pickups don't (the hitbox is tall so bobbing ghosts still count)
        if (!other.isTrigger && !other.usedByEffector && other.bounds.max.y > transform.position.y - 0.3f)
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
