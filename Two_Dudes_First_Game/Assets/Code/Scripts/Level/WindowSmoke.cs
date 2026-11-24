using System.Collections.Generic;
using UnityEngine;

// Smoke billowing out of the Treehouse window while Frank's in there tweaking. Stops once he's beaten.
public class WindowSmoke : MonoBehaviour
{
    [SerializeField] private Sprite puff;
    [SerializeField] private Vector2 openingSize = new Vector2(0.35f, 0.45f);
    [SerializeField] private Vector2 push = new Vector2(0.35f, 0.6f); // out of the window and up
    [SerializeField] private float puffsPerSecond = 7f;
    [SerializeField] private float life = 3.2f;
    [SerializeField] private float startSize = 0.35f;
    [SerializeField] private float endSize = 1.3f;
    [SerializeField] private Color color = new Color(0.78f, 0.8f, 0.76f, 0.75f);
    [SerializeField] private SpriteRenderer matchSorting; // drawn just in front of this
    [SerializeField] private string stopFlag = "frankDefeated";

    private class Puff
    {
        public Transform t;
        public SpriteRenderer sr;
        public Vector2 velocity;
        public float age;
        public float life;
        public float spin;
    }

    private readonly List<Puff> puffs = new List<Puff>();
    private readonly Stack<Puff> spare = new Stack<Puff>();
    private float owed;
    private float clock;

    private void Start()
    {
        if (!string.IsNullOrEmpty(stopFlag) && PlayerPrefs.GetInt(stopFlag) == 1)
        {
            enabled = false;
            return;
        }
        // start mid-billow instead of from nothing
        for (float t = 0f; t < life; t += 0.1f)
        {
            Tick(0.1f);
        }
    }

    private void Update()
    {
        Tick(Time.deltaTime);
    }

    private void Tick(float dt)
    {
        clock += dt;

        // comes out in heavy coughs, not a steady stream
        float surge = 0.55f + 0.45f * Mathf.Sin(clock * 1.7f) + (Mathf.Sin(clock * 0.43f) > 0.85f ? 1.2f : 0f);
        owed += puffsPerSecond * surge * dt;
        while (owed >= 1f)
        {
            owed -= 1f;
            Spawn(surge);
        }

        for (int i = puffs.Count - 1; i >= 0; i--)
        {
            Puff p = puffs[i];
            p.age += dt;
            float k = p.age / p.life;
            if (k >= 1f)
            {
                p.t.gameObject.SetActive(false);
                puffs.RemoveAt(i);
                spare.Push(p);
                continue;
            }
            p.velocity += new Vector2(0.12f, 0.05f) * dt;   // drifts off with the breeze
            p.velocity *= 1f - 0.35f * dt;
            p.t.position += (Vector3)(p.velocity * dt);
            p.t.Rotate(0f, 0f, p.spin * dt);
            float size = Mathf.Lerp(startSize, endSize, 1f - (1f - k) * (1f - k));
            p.t.localScale = new Vector3(size, size, 1f);
            Color c = color;
            c.a = color.a * (k < 0.1f ? k / 0.1f : 1f - (k - 0.1f) / 0.9f);
            p.sr.color = c;
        }
    }

    private void Spawn(float surge)
    {
        Puff p = spare.Count > 0 ? spare.Pop() : Create();
        p.t.gameObject.SetActive(true);
        p.t.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f) * openingSize.x, Random.Range(-0.5f, 0.5f) * openingSize.y, 0f);
        p.t.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        p.velocity = new Vector2(push.x * Random.Range(0.6f, 1.4f), push.y * Random.Range(0.7f, 1.3f)) * Mathf.Lerp(0.8f, 1.4f, surge * 0.5f);
        p.age = 0f;
        p.life = life * Random.Range(0.75f, 1.2f);
        p.spin = Random.Range(-40f, 40f);
        p.t.localScale = Vector3.one * startSize;
        puffs.Add(p);
    }

    private Puff Create()
    {
        GameObject g = new GameObject("Smoke");
        g.transform.SetParent(transform, true);
        SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
        sr.sprite = puff;
        if (matchSorting != null)
        {
            sr.sharedMaterial = matchSorting.sharedMaterial;
            sr.sortingLayerID = matchSorting.sortingLayerID;
            sr.sortingOrder = matchSorting.sortingOrder + 1;
        }
        return new Puff { t = g.transform, sr = sr };
    }

    private void OnDisable()
    {
        foreach (Puff p in puffs)
        {
            if (p.t != null)
            {
                p.t.gameObject.SetActive(false);
            }
        }
    }
}
