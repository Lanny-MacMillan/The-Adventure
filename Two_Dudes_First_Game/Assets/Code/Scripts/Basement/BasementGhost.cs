using System.Collections;
using UnityEngine;

// Drifts through walls toward you once it sees you. Two salt hits to kill, like outside.
public class BasementGhost : BasementEnemy
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Sprite[] idleFrames;
    [SerializeField] private Sprite[] deathFrames;
    [SerializeField] private int health = 2;
    [SerializeField] private float speed = 1.4f;
    [SerializeField] private float sightRange = 8f;
    [SerializeField] private float touchRange = 0.6f;

    private bool awake;
    private bool dead;
    private float baseY;
    private float t;
    private Color tint = Color.white;

    private void Start()
    {
        baseY = transform.position.y;
        t = Random.value * 10f;
    }

    private void Update()
    {
        if (dead || Time.timeScale == 0f)
        {
            return;
        }
        t += Time.deltaTime;
        body.sprite = idleFrames[(int)(t * 8f) % idleFrames.Length];

        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (p == null || p.Health == null || p.Health.isDead)
        {
            return;
        }
        float dist = FlatDistance(transform.position, p.transform.position);
        if (!awake && dist < sightRange && BasementSight.Clear(transform.position, p.transform.position))
        {
            awake = true;
        }

        Vector3 pos = transform.position;
        if (awake)
        {
            Vector3 target = new Vector3(p.transform.position.x, pos.y, p.transform.position.z);
            pos = Vector3.MoveTowards(pos, target, speed * Time.deltaTime);
        }
        pos.y = baseY + Mathf.Sin(t * 2.2f) * 0.12f;
        transform.position = pos;

        if (dist < touchRange)
        {
            p.Hurt(transform.position);
        }
        BasementSight.Shade(body, transform.position, 0.45f, tint);
    }

    public override bool TakeSalt(bool holy)
    {
        if (dead)
        {
            return false;
        }
        awake = true;
        health -= holy ? 2 : 1;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(Flinch());
        }
        return true;
    }

    private IEnumerator Flinch()
    {
        tint = new Color(1f, 0.45f, 0.45f);
        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (p != null)
        {
            Vector3 away = transform.position - p.transform.position;
            away.y = 0f;
            transform.position += away.normalized * 0.8f;
        }
        yield return new WaitForSeconds(0.15f);
        tint = Color.white;
    }

    private IEnumerator Die()
    {
        dead = true;
        All.Remove(this);
        for (int i = 0; i < deathFrames.Length; i++)
        {
            body.sprite = deathFrames[i];
            yield return new WaitForSeconds(0.07f);
        }
        gameObject.SetActive(false);
    }
}
