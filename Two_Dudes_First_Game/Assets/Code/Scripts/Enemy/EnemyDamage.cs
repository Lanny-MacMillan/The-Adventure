using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitPlayer(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HitPlayer(collision.gameObject);
    }

    private void HitPlayer(GameObject other)
    {
        if (!enabled || other.tag != "Player")
        {
            return;
        }

        // inspector reference can go stale between scenes
        PlayerHealth target = other.GetComponent<PlayerHealth>();
        if (target == null)
        {
            target = playerHealth;
        }
        if (target != null)
        {
            target.TakeDamage(damage, transform.position);
        }
    }
}
