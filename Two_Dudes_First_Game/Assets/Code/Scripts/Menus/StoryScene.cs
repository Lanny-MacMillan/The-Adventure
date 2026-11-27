using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// NES-style cutscene: picture panels with typed-out lines. Enter/Space/click = next line, Esc = skip it all.
public class StoryScene : MonoBehaviour
{
    [System.Serializable]
    public class Panel
    {
        public Sprite picture;
        [TextArea] public string[] lines;
    }

    [SerializeField] private Image picture;
    [SerializeField] private RectTransform textBox;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Image fade;
    [SerializeField] private float charsPerSecond = 35f;
    [SerializeField] private string nextScene = "CharacterSelect";
    [SerializeField] private Panel[] panels;

    [Header("Outro only")]
    [SerializeField] private bool outro;
    [SerializeField] private Sprite driveDevAlone;
    [SerializeField] private Sprite driveDevFrank;
    [SerializeField] private Sprite driveDude1Alone;
    [SerializeField] private Sprite driveDude1Frank;
    [SerializeField] private RectTransform credits;
    [SerializeField] private float creditsSpeed = 60f;

    // set by the Basement ending right before the run is wiped
    public static bool FrankCured;

    private Vector2 boxPos;
    private bool skipAll;

    private void Start()
    {
        Cursor.visible = false;
        boxPos = textBox.anchoredPosition;
        text.text = "";
        if (credits != null)
        {
            credits.gameObject.SetActive(false);
        }
        StartCoroutine(Run());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            skipAll = true;
        }
    }

    private IEnumerator Run()
    {
        fade.color = Color.black;
        foreach (Panel p in BuildPanels())
        {
            Show(p.picture);
            text.text = "";
            yield return Fade(1f, 0f);
            for (int i = 0; i < p.lines.Length && !skipAll; i++)
            {
                yield return Type(p.lines[i]);
            }
            if (skipAll)
            {
                break;
            }
            yield return Fade(0f, 1f);
        }
        if (outro && !skipAll)
        {
            yield return Credits();
        }
        yield return Fade(fade.color.a, 1f);
        SceneManager.LoadScene(nextScene);
    }

    private List<Panel> BuildPanels()
    {
        var list = new List<Panel>(panels);
        if (outro)
        {
            bool dev = CharacterSelection.Selected == CharacterSelection.DevGuy;
            Sprite drive = FrankCured ? (dev ? driveDevFrank : driveDude1Frank) : (dev ? driveDevAlone : driveDude1Alone);
            list.Add(new Panel
            {
                picture = drive,
                lines = FrankCured
                    ? new[] { "You drove. Your buddy's seat stayed empty.", "Frank sat in the back, still damp with holy water.", "Three hours later he asked if anyone had snacks." }
                    : new[] { "You drove. Your buddy's seat stayed empty.", "So did the back seat. RIP Frank.", "You didn't stop until the gas ran out." },
            });
            list.Add(new Panel { picture = null, lines = new[] { "THE END" } });
        }
        return list;
    }

    // no picture = black screen with the text in the middle
    private void Show(Sprite s)
    {
        picture.enabled = s != null;
        picture.sprite = s;
        textBox.anchoredPosition = s != null ? boxPos : new Vector2(boxPos.x, 360f);
        textBox.GetComponent<Image>().enabled = s != null;
    }

    private IEnumerator Type(string line)
    {
        if (line == "THE END")
        {
            Sound.Play("endingSting");
        }
        text.text = line;
        text.maxVisibleCharacters = 0;
        float shown = 0f;
        while (text.maxVisibleCharacters < line.Length && !skipAll)
        {
            if (Pressed())
            {
                break;
            }
            shown += Time.deltaTime * charsPerSecond;
            int n = Mathf.Min(line.Length, (int)shown);
            if (n > text.maxVisibleCharacters && n % 3 == 0 && line[n - 1] != ' ')
            {
                Sound.Play("dialogueBlip", 0.5f);
            }
            text.maxVisibleCharacters = n;
            yield return null;
        }
        text.maxVisibleCharacters = line.Length;
        yield return null;
        while (!skipAll && !Pressed())
        {
            yield return null;
        }
        Sound.Play("menuMove", 0.5f);
    }

    private IEnumerator Credits()
    {
        Show(null);
        text.text = "";
        credits.gameObject.SetActive(true);
        float end = credits.GetComponent<TMPro.TMP_Text>().preferredHeight + 400f;
        credits.anchoredPosition = new Vector2(0f, -360f);
        yield return Fade(1f, 0f);
        while (credits.anchoredPosition.y < end && !skipAll)
        {
            float speed = Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0) ? creditsSpeed * 4f : creditsSpeed;
            credits.anchoredPosition += Vector2.up * speed * Time.deltaTime;
            yield return null;
        }
    }

    private static bool Pressed()
    {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = skipAll ? 0.01f : 0.6f;
        for (float k = 0f; k < t; k += Time.deltaTime)
        {
            fade.color = new Color(0f, 0f, 0f, Mathf.Lerp(from, to, k / t));
            yield return null;
        }
        fade.color = new Color(0f, 0f, 0f, to);
    }
}
