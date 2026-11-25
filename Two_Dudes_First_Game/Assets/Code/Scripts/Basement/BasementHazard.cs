using UnityEngine;

// Timed floor traps: spikes, the crusher on a chain, and green flame vents. Hurts you if you're in its spot while it's live.
public class BasementHazard : MonoBehaviour
{
    [System.Serializable]
    public class Phase
    {
        public string name;
        public Sprite[] frames;
        public float duration = 1f;
        public bool deadly;
        public bool loop;
    }

    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Phase[] phases;
    [SerializeField] private float reach = 0.8f;
    [SerializeField] private bool randomStart = true;
    [SerializeField] private float glow;

    private int phase;
    private float timer;

    private void Start()
    {
        if (randomStart && phases != null)
        {
            float total = 0f;
            foreach (Phase p in phases)
            {
                total += p.duration;
            }
            timer = Random.value * total;
        }
    }

    private void Update()
    {
        if (phases == null || phases.Length == 0 || Time.timeScale == 0f)
        {
            return;
        }
        timer += Time.deltaTime;
        while (timer >= phases[phase].duration)
        {
            timer -= phases[phase].duration;
            phase = (phase + 1) % phases.Length;
        }

        Phase p = phases[phase];
        if (p.frames != null && p.frames.Length > 0)
        {
            int f = p.loop ? (int)(timer / 0.08f) % p.frames.Length : Mathf.Min(p.frames.Length - 1, (int)(timer / p.duration * p.frames.Length));
            body.sprite = p.frames[f];
        }
        body.enabled = p.frames != null && p.frames.Length > 0;

        Camera cam = Camera.main;
        if (cam != null)
        {
            body.transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);
        }
        BasementSight.Shade(body, transform.position, glow, Color.white);

        FirstPersonPlayer player = FirstPersonPlayer.Instance;
        if (p.deadly && player != null)
        {
            Vector3 d = player.transform.position - transform.position;
            if (d.x * d.x + d.z * d.z < reach * reach)
            {
                player.Hurt(transform.position);
            }
        }
    }
}
