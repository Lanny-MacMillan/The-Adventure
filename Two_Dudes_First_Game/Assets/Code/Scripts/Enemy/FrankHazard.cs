using System.Collections;
using UnityEngine;

// Frank's spit, ceiling goo and floor shockwaves: hurts the player on touch, splats when it lands
public class FrankHazard : MonoBehaviour
{
    public Vector2 velocity;
    public float gravity;
    public float floorY;
    public float left = -10f;
    public float right = 10f;
    public bool rollsOnFloor;
    public float spin;

    private bool landed;

    private void Update()
    {
        if (landed)
        {
            return;
        }

        velocity.y -= gravity * Time.deltaTime;
        transform.position += (Vector3)(velocity * Time.deltaTime);
        if (spin != 0f)
        {
            transform.Rotate(0f, 0f, spin * Time.deltaTime);
        }

        if (!rollsOnFloor && transform.position.y <= floorY)
        {
            transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
            Land();
        }
        else if (transform.position.x < left || transform.position.x > right)
        {
            if (rollsOnFloor)
            {
                Destroy(gameObject);
            }
            else
            {
                Land();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (landed)
        {
            return;
        }
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null || player.isDead)
        {
            return;
        }
        player.TakeDamage(1, transform.position);
        if (!rollsOnFloor)
        {
            Land();
        }
    }

    public void Land()
    {
        if (landed)
        {
            return;
        }
        landed = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        StartCoroutine(Splat());
    }

    private IEnumerator Splat()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector3 start = transform.localScale;
        transform.rotation = Quaternion.identity;
        for (float t = 0f; t < 0.5f; t += Time.deltaTime)
        {
            float k = t / 0.5f;
            transform.localScale = new Vector3(start.x * (1f + k), start.y * (1f - 0.7f * k), start.z);
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - k;
                sr.color = c;
            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
