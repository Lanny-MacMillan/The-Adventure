using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Fades out after the boss and tells you how it ended, then back to the title for a fresh run.
public class BasementEnding : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private float lineTime = 3.5f;

    [TextArea] [SerializeField] private string[] killedLines =
    {
        "The green fire screams and burns itself out.",
        "All that's left is a scorched, empty baggie.",
        "Upstairs, the house finally goes quiet.",
    };
    [TextArea] [SerializeField] private string[] curedLines =
    {
        "The holy water hisses through the green fire.",
        "It shrinks into a harmless little puff of smoke.",
        "The baggie is just oregano now.",
        "The house is just a house again.",
    };
    [SerializeField] private string frankCuredLine = "Frank swears he's never smoking basement stuff again.";
    [SerializeField] private string frankDeadLine = "Frank is still a puddle in the treehouse. RIP Frank.";

    public bool Playing { get; private set; }
    public static bool Active { get; private set; }

    private void Awake()
    {
        Active = false;
        fade.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void Play(bool cured)
    {
        if (!Playing)
        {
            Playing = true;
            Active = true;
            Sound.Music("ending");
            StartCoroutine(Run(cured));
        }
    }

    private IEnumerator Run(bool cured)
    {
        fade.gameObject.SetActive(true);
        for (float k = 0f; k < 1f; k += Time.deltaTime / 1.5f)
        {
            fade.color = new Color(0f, 0f, 0f, k);
            yield return null;
        }
        fade.color = Color.black;

        var lines = new System.Collections.Generic.List<string>(cured ? curedLines : killedLines);
        lines.Add(PlayerPrefs.GetInt("frankCured") == 1 ? frankCuredLine : frankDeadLine);
        lines.Add("THE END");

        text.gameObject.SetActive(true);
        foreach (string line in lines)
        {
            text.text = line;
            yield return Hold();
        }

        GameManagerScript gm = FindAnyObjectByType<GameManagerScript>();
        if (gm != null)
        {
            gm.QuitToTitle();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        }
    }

    // each line stays up for a few seconds, Enter or Space skips ahead
    private IEnumerator Hold()
    {
        yield return null;
        for (float k = 0f; k < lineTime; k += Time.deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                break;
            }
            yield return null;
        }
    }
}
