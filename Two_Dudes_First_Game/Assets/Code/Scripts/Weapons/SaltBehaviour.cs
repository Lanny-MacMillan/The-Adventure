using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltBehaviour : MonoBehaviour
{
    private float speed = 6f;
    public PlayerMovement playerMovement; // access script with player direction

    // set when thrown so turning around doesn't steer salt already in the air
    public bool facingRight = true;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

        // so on collision we will try and see if they have an Enemy Ghost Component, extract enemyComponent and apply functions
        if (collision.gameObject.TryGetComponent<EnemyGhost>(out EnemyGhost enemyComponent))
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
            enemyComponent.TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        FrankBoss frank = other.GetComponentInParent<FrankBoss>();
        if (frank != null && frank.TakeSalt(1))
        {
            Destroy(gameObject);
        }


    }

}
