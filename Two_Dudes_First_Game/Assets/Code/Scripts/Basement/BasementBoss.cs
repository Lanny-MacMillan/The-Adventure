using System.Collections;
using UnityEngine;

// Final boss: whatever was in Frank's baggie, a living green flame. Salt kills it; holy water salt cures it instead.
public class BasementBoss : BasementEnemy
{
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private Sprite[] flameFrames;
    [SerializeField] private Sprite spitSprite;
    [SerializeField] private Material spriteMaterial;
    [SerializeField] private GameObject ghostTemplate;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private float wakeRange = 7f;
    [SerializeField] private float sway = 2.2f;
    [SerializeField] private float spitSpeed = 5f;

    [Header("Bar")]
    [SerializeField] private GameObject bar;
    [SerializeField] private RectTransform barFill;
    [SerializeField] private UnityEngine.UI.Image barFillImage;
    [SerializeField] private TMPro.TextMeshProUGUI barLabel;
    [SerializeField] private Color killColor = new Color(0.45f, 0.9f, 0.2f);
    [SerializeField] private Color cureColor = new Color(0.55f, 0.85f, 1f);

    [SerializeField] private BasementEnding ending;

    private int health;
    private bool curing;
    private bool awake;
    private bool done;
    private float t;
    private float flash;
    private Vector3 home;
    private Color tint = Color.white;
    private Coroutine fight;
    private readonly System.Collections.Generic.List<GameObject> summoned = new System.Collections.Generic.List<GameObject>();

    private void Start()
    {
        health = maxHealth;
        home = transform.position;
        curing = PlayerPrefs.GetInt("holyWater") == 1;
        if (bar != null)
        {
            bar.SetActive(false);
            barLabel.text = curing ? "CURE" : "THE STASH";
            barLabel.color = curing ? cureColor : killColor;
            barFillImage.color = curing ? cureColor : killColor;
        }
        UpdateBar();
    }

    private void Update()
    {
        if (done || Time.timeScale == 0f)
        {
            return;
        }
        t += Time.deltaTime;
        body.sprite = flameFrames[(int)(t / 0.08f) % flameFrames.Length];
        flash = Mathf.MoveTowards(flash, 0f, Time.deltaTime * 4f);
        BasementSight.Shade(body, transform.position, 0.9f, Color.Lerp(tint, Color.white, flash));
        BasementSight.Shade(face, transform.position, 0.9f, Color.white);

        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (p == null || p.Health == null || p.Health.isDead)
        {
            return;
        }
        if (!awake && FlatDistance(transform.position, p.transform.position) < wakeRange && BasementSight.Clear(transform.position, p.transform.position))
        {
            awake = true;
            if (bar != null)
            {
                bar.SetActive(true);
            }
            fight = StartCoroutine(Fight());
            Sound.Music("boss");
        }
        if (awake)
        {
            // drifts side to side across the room so you have to keep aiming
            float s = Mathf.Sin(t * (Angry() ? 0.9f : 0.6f)) * sway;
            Vector3 side = Vector3.Cross(Vector3.up, (p.transform.position - home).normalized);
            transform.position = home + side * s;
        }
    }

    private bool Angry()
    {
        return health <= maxHealth / 2;
    }

    private IEnumerator Fight()
    {
        int n = 0;
        yield return new WaitForSeconds(1f);
        while (!done)
        {
            n++;
            if (n % 4 == 0)
            {
                Summon();
            }
            else
            {
                int count = Angry() ? 5 : (n % 2 == 0 ? 3 : 1);
                yield return Spit(count);
            }
            yield return new WaitForSeconds(Angry() ? 1.0f : 1.5f);
        }
    }

    private IEnumerator Spit(int count)
    {
        Sound.Play("stashSpit");
        face.transform.localScale = new Vector3(1.1f, 0.9f, 1f);
        yield return new WaitForSeconds(0.35f);
        face.transform.localScale = Vector3.one;
        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (done || p == null)
        {
            yield break;
        }
        Vector3 from = transform.position + Vector3.up * 0.9f;
        Vector3 aim = p.transform.position - Vector3.up * 0.15f - from;
        aim.y = 0f;
        for (int i = 0; i < count; i++)
        {
            float angle = (i - (count - 1) / 2f) * 14f;
            GameObject go = new GameObject("Spit");
            go.transform.position = from;
            go.AddComponent<BossSpit>().Launch(Quaternion.Euler(0f, angle, 0f) * aim, spitSpeed * (Angry() ? 1.25f : 1f), spitSprite, spriteMaterial);
        }
    }

    private void Summon()
    {
        summoned.RemoveAll(g => g == null || !g.activeSelf);
        if (ghostTemplate == null || summoned.Count >= 2)
        {
            return;
        }
        Sound.Play("stashSummon");
        for (int i = -1; i <= 1; i += 2)
        {
            Vector3 at = home + Vector3.Cross(Vector3.up, Vector3.forward) * (i * 2.5f);
            GameObject g = Instantiate(ghostTemplate, at + ghostTemplate.transform.localPosition, Quaternion.identity, transform.parent);
            g.SetActive(true);
            summoned.Add(g);
        }
    }

    public override bool TakeSalt(bool holy)
    {
        if (done || !awake)
        {
            return !done;
        }
        health--;
        flash = 1f;
        if (curing)
        {
            tint = Color.Lerp(Color.white, cureColor, 1f - (float)health / maxHealth);
        }
        UpdateBar();
        if (health <= 0)
        {
            StartCoroutine(curing ? Cured() : Die());
        }
        return true;
    }

    private void UpdateBar()
    {
        if (barFill != null)
        {
            float k = (float)health / maxHealth;
            barFill.localScale = new Vector3(curing ? 1f - k : k, 1f, 1f);
        }
    }

    private void EndFight()
    {
        done = true;
        if (fight != null)
        {
            StopCoroutine(fight);
        }
        All.Remove(this);
        foreach (BossSpit s in FindObjectsByType<BossSpit>(FindObjectsInactive.Exclude))
        {
            Destroy(s.gameObject);
        }
        foreach (GameObject g in summoned)
        {
            if (g != null)
            {
                g.SetActive(false);
            }
        }
        if (bar != null)
        {
            bar.SetActive(false);
        }
    }

    // burns itself out and shrinks to nothing
    private IEnumerator Die()
    {
        EndFight();
        Sound.Play("stashDie");
        Vector3 start = transform.localScale;
        for (float k = 0f; k < 2f; k += Time.deltaTime)
        {
            body.sprite = flameFrames[(int)(k / 0.05f) % flameFrames.Length];
            float s = 1f - k / 2f;
            transform.localScale = new Vector3(start.x * (s + Mathf.Sin(k * 40f) * 0.05f), start.y * s, start.z);
            body.color = Color.Lerp(Color.white, new Color(0.3f, 0.3f, 0.3f), k / 2f);
            yield return null;
        }
        body.enabled = false;
        face.enabled = false;
        Finish(false);
    }

    // the holy water drains the green out until it's just a puff of smoke
    private IEnumerator Cured()
    {
        EndFight();
        Sound.Play("stashCure");
        Vector3 start = transform.localScale;
        for (float k = 0f; k < 2.5f; k += Time.deltaTime)
        {
            body.sprite = flameFrames[(int)(k / 0.1f) % flameFrames.Length];
            body.color = Color.Lerp(cureColor, new Color(1f, 1f, 1f, 0f), k / 2.5f);
            face.color = new Color(1f, 1f, 1f, 1f - k / 1.2f);
            transform.localScale = start * Mathf.Lerp(1f, 0.25f, k / 2.5f);
            yield return null;
        }
        body.enabled = false;
        face.enabled = false;
        Finish(true);
    }

    private void Finish(bool cured)
    {
        PlayerPrefs.SetInt("basementCleared", 1);
        PlayerPrefs.SetInt("stashCured", cured ? 1 : 0);
        PlayerPrefs.Save();
        if (ending != null)
        {
            ending.Play(cured);
        }
    }
}
