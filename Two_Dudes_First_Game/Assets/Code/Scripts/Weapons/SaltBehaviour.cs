using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltBehaviour : MonoBehaviour
{
    private float speed = 6f;
    public PlayerMovement playerMovement; // access script with player direction

    // set when thrown so turning around doesn't steer salt already in the air
    public bool facingRight = true;

    private void Start()
    {
        // carrying holy water blesses the salt, which cures Frank instead of hurting him
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && PlayerPrefs.GetInt("holyWater") == 1)
        {
            sr.color = new Color(0.55f, 0.85f, 1f, 1f);
        }
    }

    private void Update()
    {
        if (facingRight)
        {
            // facing right, tranform salt to right side of player
            transform.position += speed * Time.deltaTime * transform.right;
        } else {
            // facing left, tranform salt to left side of player
            transform.position += speed * Time.deltaTime * -transform.right;

        }
    }

    // breaks on anything solid (floors, walls, ghosts), not just objects tagged Ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (collision.gameObject.TryGetComponent<EnemyGhost>(out EnemyGhost enemyComponent))
        {
            Sound.Play("saltHit");
            enemyComponent.TakeDamage(1);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FrankBoss frank = other.GetComponentInParent<FrankBoss>();
        if (frank != null && frank.TakeSalt(1))
        {
            Sound.Play("saltHit");
            Destroy(gameObject);
        }

        EvilSpirit spirit = other.GetComponentInParent<EvilSpirit>();
        if (spirit != null && spirit.TakeSalt())
        {
            Sound.Play("saltHit");
            Destroy(gameObject);
        }

        AtticBat bat = other.GetComponentInParent<AtticBat>();
        if (bat != null && bat.TakeSalt())
        {
            Sound.Play("saltHit");
            Destroy(gameObject);
        }
    }

}
