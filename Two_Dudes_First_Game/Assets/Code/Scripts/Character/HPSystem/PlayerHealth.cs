using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;

    public SpriteRenderer playerChar;
    public PlayerMovement playerMovement;

    public GameManagerScript gameManager;
    public bool isDead;

    //public CharacterController controller;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0 && !isDead)
        {
            isDead = true;
            //playerChar.enabled = false;
            playerMovement.enabled = false;

            // freeze the body so enemies can't push the corpse around
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            animator.SetBool("isDead", true);
            Invoke(nameof(GameOver), 2);
        }
    }

    private void GameOver()
    {
        gameManager.gameOver();
        Destroy(playerChar);

    }
}
