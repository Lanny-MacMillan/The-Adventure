using System.Collections;
using UnityEngine;

// A short line in a little box over your head that doesn't stop you moving, e.g. PlayerBark.Say("Huh.").
public class PlayerBark : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_FontAsset font;
    [SerializeField] private Sprite box;           // 9-sliced
    [SerializeField] private Material unlit;       // so the dark outdoor lighting doesn't swallow it
    [SerializeField] private float fontSize = 1.6f;
    [SerializeField] private float height = 1.5f;  // above the player's pivot
    [SerializeField] private float duration = 2.8f;

    private static PlayerBark instance;
    private Transform bubble;
    private SpriteRenderer back;
    private TMPro.TextMeshPro text;
    private Coroutine showing;

    private void Awake()
    {
        if (gameObject.scene.name == "DontDestroyOnLoad" || instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (gameObject.scene.name == "DontDestroyOnLoad")
        {
            instance = this;
        }
    }

    public static void Say(string line)
    {
        if (instance != null)
        {
            instance.Show(line);
        }
    }

    private void Show(string line)
    {
        Build();
        if (showing != null)
        {
            StopCoroutine(showing);
        }
        showing = StartCoroutine(Run(line));
    }

    // lives outside the player so it doesn't mirror when you turn around
    private void Build()
    {
        if (bubble != null)
        {
            return;
        }
        bubble = new GameObject("PlayerBark").transform;
        // drawn at 2x so the box's pixel border is chunky like the dialogue box
        bubble.localScale = Vector3.one * 2f;
        DontDestroyOnLoad(bubble.gameObject);
        back = bubble.gameObject.AddComponent<SpriteRenderer>();
        back.sprite = box;
        back.drawMode = SpriteDrawMode.Sliced;
        if (unlit != null)
        {
            back.sharedMaterial = unlit;
        }
        back.sortingOrder = 30;
        var t = new GameObject("Text");
        t.transform.SetParent(bubble, false);
        t.transform.localScale = Vector3.one * 0.5f;
        text = t.AddComponent<TMPro.TextMeshPro>();
        text.font = font;
        text.fontSize = fontSize;
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.textWrappingMode = TMPro.TextWrappingModes.NoWrap;
        text.color = Color.white;
        text.sortingOrder = 31;
        bubble.gameObject.SetActive(false);
    }

    private IEnumerator Run(string line)
    {
        text.text = line;
        back.color = Color.clear;
        text.color = Color.clear;
        bubble.gameObject.SetActive(true);
        // a freshly made text isn't laid out until a frame has passed, so size the box after that
        yield return null;
        text.ForceMeshUpdate();
        back.size = text.GetRenderedValues(false) * 0.5f + new Vector2(0.35f, 0.3f);
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float a = Mathf.Clamp01((duration - t) / 0.3f);
            back.color = new Color(1f, 1f, 1f, a);
            text.color = new Color(1f, 1f, 1f, a);
            yield return null;
        }
        bubble.gameObject.SetActive(false);
        showing = null;
    }

    private void LateUpdate()
    {
        if (bubble != null && bubble.gameObject.activeSelf)
        {
            bubble.position = transform.position + Vector3.up * height;
        }
    }

    private void OnDestroy()
    {
        if (bubble != null)
        {
            Destroy(bubble.gameObject);
        }
    }
}
