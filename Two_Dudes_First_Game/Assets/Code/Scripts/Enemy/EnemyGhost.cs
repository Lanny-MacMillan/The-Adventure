using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 2f;

    public bool enemyDead = false;
    public Animator animator;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage( float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0

        if (health <= 0)
        {
            Debug.Log("Dead!");
            enemyDead = true;
            //Destroy(gameObject);
            animator.SetBool("enemyDead", true);

        }
    }
}
