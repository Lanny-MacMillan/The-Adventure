using System.Collections;
using UnityEngine;

public class EnemyGhost : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 2f;

    public bool enemyDead = false;
    public Animator animator;

    private SpriteRenderer body;

    private void Start()
    {
        health = maxHealth;
        body = GetComponent<SpriteRenderer>();
    }

    // false once it's already dying, so salt flies on through
    public bool TakeDamage(float damageAmount)
    {
        if (enemyDead)
        {
            return false;
        }
        health -= damageAmount;

        if (health <= 0)
        {
            enemyDead = true;
            Sound.Play("ghostMoan", 0.6f);
            animator.SetBool("enemyDead", true);
            // can't hurt you while it fades out
            foreach (Collider2D c in GetComponents<Collider2D>())
            {
                c.enabled = false;
            }
            EnemyDamage damage = GetComponent<EnemyDamage>();
            if (damage != null)
            {
                damage.enabled = false;
            }
        }
        else if (body != null)
        {
            StartCoroutine(Flash());
        }
        return true;
    }

    private IEnumerator Flash()
    {
        body.color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(0.12f);
        body.color = Color.white;
    }
}
