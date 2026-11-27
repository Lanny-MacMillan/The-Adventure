using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// The Two Dudes splash: fade in, the logo builds, the dudes hop and high five (slap + flash), names fade in, then fade to the title.
// Any key skips to the title with a quick fade.
public class SplashIntro : MonoBehaviour
{
    [SerializeField] private SpriteRenderer logo;
    [SerializeField] private Sprite[] frames;              // spritesheet_0 .. 21 in order
    [SerializeField] private TMPro.TextMeshPro names;
    [SerializeField] private string nextScene = "TitleScreen";
    [SerializeField] private Vector2 handsPixel = new Vector2(467f, 236f); // where the hands meet, from the frame's top-left
    [SerializeField] private int crouchFrame = 2;
    [SerializeField] private int slapFrame = 4;
    [SerializeField] private int landFrame = 7;
    [SerializeField] private float hopHeight = 0.35f;

    private SpriteRenderer fade;
    private SpriteRenderer burst;
    private Vector3 home;
    private bool leaving;

    private void Start()
    {
        home = logo.transform.position;
        fade = Quad("Fade", new Color(0f, 0f, 0f, 1f), 100);
        fade.transform.localScale = new Vector3(400f, 400f, 1f);
        burst = new GameObject("HighFiveBurst").AddComponent<SpriteRenderer>();
        burst.sprite = BurstSprite();
        burst.sortingOrder = logo.sortingOrder + 1;
        burst.enabled = false;
        if (names != null)
        {
            names.alpha = 0f;
        }
        StartCoroutine(Run());
    }

    private void Update()
    {
        if (!leaving && Time.timeSinceLevelLoad > 0.3f && Input.anyKeyDown)
        {
            StopAllCoroutines();
            StartCoroutine(Leave(0.2f));
        }
    }

    private IEnumerator Run()
    {
        logo.sprite = frames[0];
        yield return Fade(1f, 0f, 0.4f);

        // stand, wind up, crouch... hop, slap, land
        float[] hold = { 0.25f, 0.12f, 0.12f, 0.10f, 0.12f, 0.12f, 0.10f };
        for (int i = 0; i < landFrame; i++)
        {
            logo.sprite = frames[i];
            if (i == crouchFrame)
            {
                Sound.Play("jump");
                StartCoroutine(Hop(HoldUntil(hold, crouchFrame, landFrame)));
            }
            if (i == slapFrame)
            {
                Sound.Play("highFive");
                StartCoroutine(Slap());
            }
            yield return new WaitForSeconds(hold[i]);
        }
        Sound.Play("land", 0.6f);

        // idle on the finished logo while the names come up
        StartCoroutine(FadeNames(0.5f));
        for (int i = landFrame; i < frames.Length; i++)
        {
            logo.sprite = frames[i];
            yield return new WaitForSeconds(0.12f);
        }
        yield return new WaitForSeconds(0.6f);
        yield return Leave(0.45f);
    }

    private static float HoldUntil(float[] hold, int from, int to)
    {
        float t = 0f;
        for (int i = from; i < to && i < hold.Length; i++)
        {
            t += hold[i];
        }
        return t;
    }

    private IEnumerator Hop(float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float k = t / duration;
            logo.transform.position = home + Vector3.up * (4f * hopHeight * k * (1f - k));
            yield return null;
        }
        logo.transform.position = home;
        // tiny squash on landing
        Vector3 s = logo.transform.localScale;
        for (float t = 0f; t < 0.12f; t += Time.deltaTime)
        {
            float k = Mathf.Sin(t / 0.12f * Mathf.PI);
            logo.transform.localScale = new Vector3(s.x * (1f + 0.04f * k), s.y * (1f - 0.05f * k), s.z);
            yield return null;
        }
        logo.transform.localScale = s;
    }

    // white burst at the hands and a little shake
    private IEnumerator Slap()
    {
        burst.enabled = true;
        for (float t = 0f; t < 0.28f; t += Time.deltaTime)
        {
            float k = t / 0.28f;
            burst.transform.position = HandsWorld() + Vector3.back * 0.1f;
            burst.transform.localScale = Vector3.one * Mathf.Lerp(0.8f, 2.6f, k);
            burst.color = new Color(1f, 1f, 0.75f, k < 0.35f ? 1f : 1f - (k - 0.35f) / 0.65f);
            Camera.main.transform.position = new Vector3(0f, 0f, -10f) + (Vector3)(Random.insideUnitCircle * 0.06f * (1f - k));
            yield return null;
        }
        burst.enabled = false;
        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
    }

    private Vector3 HandsWorld()
    {
        Sprite s = logo.sprite;
        float k = s.rect.width / 900f;
        Vector2 px = new Vector2(handsPixel.x * k, s.rect.height - handsPixel.y * k);
        Vector2 local = (px - s.pivot) / s.pixelsPerUnit;
        return logo.transform.TransformPoint(local);
    }

    private IEnumerator FadeNames(float time)
    {
        if (names == null)
        {
            yield break;
        }
        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            names.alpha = t / time;
            yield return null;
        }
        names.alpha = 1f;
    }

    private IEnumerator Leave(float time)
    {
        leaving = true;
        yield return Fade(fade.color.a, 1f, time);
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator Fade(float from, float to, float time)
    {
        for (float t = 0f; t < time; t += Time.deltaTime)
        {
            fade.color = new Color(0f, 0f, 0f, Mathf.Lerp(from, to, t / time));
            yield return null;
        }
        fade.color = new Color(0f, 0f, 0f, to);
    }

    private SpriteRenderer Quad(string name, Color c, int order)
    {
        var sr = new GameObject(name).AddComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 4, 4), new Vector2(0.5f, 0.5f), 4f);
        sr.color = c;
        sr.sortingOrder = order;
        return sr;
    }

    // eight chunky rays, drawn once at runtime
    private static Sprite BurstSprite()
    {
        const int n = 32;
        var tex = new Texture2D(n, n, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
        var clear = new Color32(0, 0, 0, 0);
        var px = new Color32[n * n];
        for (int i = 0; i < px.Length; i++)
        {
            px[i] = clear;
        }
        for (int a = 0; a < 8; a++)
        {
            float ang = a * Mathf.PI / 4f;
            for (float r = 6f; r < 15f; r += 0.5f)
            {
                int x = Mathf.RoundToInt(n / 2 + Mathf.Cos(ang) * r);
                int y = Mathf.RoundToInt(n / 2 + Mathf.Sin(ang) * r);
                if (x >= 0 && x < n && y >= 0 && y < n)
                {
                    px[y * n + x] = new Color32(255, 255, 255, 255);
                }
            }
        }
        tex.SetPixels32(px);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 32f);
    }
}
