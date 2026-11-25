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

    [Header("Getting Hit")]
    public Vector2 knockbackForce = new Vector2(6f, 6f);
    public float knockbackTime = 0.25f;
    public float invincibleTime = 1f;
    public Color hurtColor = new Color(1f, 0.35f, 0.35f, 1f);

    private bool invincible;
    private RigidbodyConstraints2D aliveConstraints = RigidbodyConstraints2D.FreezeRotation;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, transform.position);
    }

    // knocks the player away from hitFrom
    public void TakeDamage(int amount, Vector2 hitFrom)
    {
        if (isDead || invincible)
        {
            return;
        }

        health -= amount;
        if (health <= 0)
        {
            Die();
            return;
        }

        float direction = transform.position.x >= hitFrom.x ? 1f : -1f;
        if (Mathf.Approximately(transform.position.x, hitFrom.x))
        {
            direction = playerMovement != null && playerMovement.isFacingRight ? -1f : 1f;
        }
        if (playerMovement != null)
        {
            playerMovement.Knockback(new Vector2(knockbackForce.x * direction, knockbackForce.y), knockbackTime);
        }

        StartCoroutine(HurtFlash());
    }

    // used by the death zone
    public void Kill()
    {
        if (isDead)
        {
            return;
        }
        health = 0;
        Die();
    }

    private void Die()
    {
        isDead = true;
        //playerChar.enabled = false;
        playerMovement.enabled = false;

        // freeze so enemies can't push the body around
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            aliveConstraints = rb.constraints;
            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        animator.SetBool("isDead", true);
        Invoke(nameof(GameOver), 2);
    }

    // blink red and stay invincible for a moment after a hit
    private IEnumerator HurtFlash()
    {
        invincible = true;
        float timer = 0f;
        while (timer < invincibleTime && !isDead)
        {
            if (playerChar != null)
            {
                playerChar.color = (Mathf.FloorToInt(timer / 0.1f) % 2 == 0) ? hurtColor : Color.white;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (playerChar != null)
        {
            playerChar.color = Color.white;
        }
        invincible = false;
    }

    private void GameOver()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManagerScript>();
        }
        if (gameManager != null)
        {
            gameManager.gameOver();
        }
        else
        {
            // no Game Over screen when Play is started straight in a room, so just get back up
            Revive();
            return;
        }
        if (playerChar != null)
        {
            playerChar.enabled = false;
        }
    }

    // Try Again from Game Over: back on your feet with full hearts
    public void Revive()
    {
        CancelInvoke(nameof(GameOver));
        StopAllCoroutines();
        isDead = false;
        invincible = false;
        health = maxHealth;
        if (playerChar != null)
        {
            playerChar.enabled = true;
            playerChar.color = Color.white;
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = aliveConstraints;
            rb.linearVelocity = Vector2.zero;
        }
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }
    }
}
