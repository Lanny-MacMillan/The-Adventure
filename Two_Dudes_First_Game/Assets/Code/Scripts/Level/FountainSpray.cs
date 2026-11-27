using System.Collections.Generic;
using UnityEngine;

// Once the pump's fixed: water arcs from the fountain onto the roof cross and splashes off it glowing.
public class FountainSpray : MonoBehaviour
{
    [SerializeField] private Sprite drop;
    [SerializeField] private Vector2 nozzle = new Vector2(0f, 1f); // from this object
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer glow;
    [SerializeField] private SpriteRenderer matchSorting; // drawn just in front of this
    [SerializeField] private float dropsPerSecond = 45f;
    [SerializeField] private float flightTime = 1.1f;
    [SerializeField] private float arcHeight = 1.6f;
    [SerializeField] private float dropSize = 0.9f;
    [SerializeField] private Color color = new Color(0.55f, 0.8f, 1f, 0.95f);
    [SerializeField] private Color holyColor = new Color(1f, 0.95f, 0.6f, 1f);
    [SerializeField] private string runFlag = "fountainFixed";

    private class Drop
    {
        public Transform t;
        public SpriteRenderer sr;
        public Vector2 from;
        public Vector2 to;
        public float height;
        public Vector2 velocity;
        public float age;
        public float life;
        public bool splashing;
    }

    private readonly List<Drop> drops = new List<Drop>();
    private readonly Stack<Drop> spare = new Stack<Drop>();
    private bool running;
    private float power;
    private float owed;
    private float clock;

    private void Start()
    {
        running = PlayerPrefs.GetInt(runFlag) == 1;
        power = running ? 1f : 0f;
        if (glow != null)
        {
            glow.gameObject.SetActive(running);
        }
        if (running)
        {
            for (float t = 0f; t < flightTime + 0.6f; t += 0.05f)
            {
                Tick(0.05f);
            }
        }
    }

    public void Begin()
    {
        running = true;
        if (glow != null)
        {
            glow.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        Tick(Time.deltaTime);
    }

    private void Tick(float dt)
    {
        clock += dt;
        if (running)
        {
            power = Mathf.MoveTowards(power, 1f, dt / 1.5f); // sputters up to full
            owed += dropsPerSecond * power * dt;
            while (owed >= 1f)
            {
                owed -= 1f;
                Spawn();
            }
        }

        if (glow != null && glow.gameObject.activeSelf)
        {
            Color g = holyColor;
            g.a = (0.8f + 0.2f * Mathf.Sin(clock * 3f)) * power;
            glow.color = g;
        }

        for (int i = drops.Count - 1; i >= 0; i--)
        {
            Drop d = drops[i];
            d.age += dt;
            if (!d.splashing)
            {
                float k = d.age / d.life;
                if (k >= 1f)
                {
                    // hits the cross: splashes off it blessed
                    d.splashing = true;
                    d.age = 0f;
                    d.life = Random.Range(0.4f, 0.8f);
                    d.velocity = new Vector2(Random.Range(-0.9f, 0.9f), Random.Range(0.2f, 1.1f));
                    d.sr.color = holyColor;
                    continue;
                }
                d.t.position = new Vector3(Mathf.Lerp(d.from.x, d.to.x, k), Mathf.Lerp(d.from.y, d.to.y, k) + d.height * 4f * k * (1f - k), transform.position.z);
            }
            else
            {
                if (d.age >= d.life)
                {
                    d.t.gameObject.SetActive(false);
                    drops.RemoveAt(i);
                    spare.Push(d);
                    continue;
                }
                d.velocity += Vector2.down * 7f * dt;
                d.t.position += (Vector3)(d.velocity * dt);
                Color c = holyColor;
                float f = d.age / d.life;
                c.a = holyColor.a * (1f - f * f);
                d.sr.color = c;
            }
        }
    }

    private void Spawn()
    {
        if (target == null)
        {
            return;
        }
        Drop d = spare.Count > 0 ? spare.Pop() : Create();
        d.t.gameObject.SetActive(true);
        d.from = (Vector2)transform.position + nozzle + new Vector2(Random.Range(-0.06f, 0.06f), 0f);
        d.to = (Vector2)target.position + Random.insideUnitCircle * 0.18f;
        d.height = arcHeight * Random.Range(0.85f, 1.15f) * Mathf.Lerp(0.3f, 1f, power);
        d.life = flightTime * Random.Range(0.92f, 1.08f);
        d.age = 0f;
        d.splashing = false;
        d.sr.color = color;
        d.t.position = d.from;
        float s = dropSize * Random.Range(0.75f, 1.2f);
        d.t.localScale = new Vector3(s, s, 1f);
        drops.Add(d);
    }

    private Drop Create()
    {
        GameObject g = new GameObject("Water");
        g.transform.SetParent(transform, true);
        SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
        sr.sprite = drop;
        if (matchSorting != null)
        {
            sr.sharedMaterial = matchSorting.sharedMaterial;
            sr.sortingLayerID = matchSorting.sortingLayerID;
            sr.sortingOrder = matchSorting.sortingOrder + 1;
        }
        return new Drop { t = g.transform, sr = sr };
    }
}
