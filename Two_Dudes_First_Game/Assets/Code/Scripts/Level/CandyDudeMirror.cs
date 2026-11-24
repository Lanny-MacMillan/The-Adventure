using System.Collections;
using UnityEngine;

// Say his name three times: each one cracks the mirror more, the third breaks it and starts the fight. Walking away resets it.
public class CandyDudeMirror : MonoBehaviour
{
    [SerializeField] private Transform doorPos;
    [SerializeField] private float width = 1.03f;
    [SerializeField] private float height = 1.87f;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private GameObject prompt;

    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private SpriteRenderer mirror;
    [SerializeField] private Sprite[] crackStages; // clean, cracked, more cracked
    [SerializeField] private Sprite[] breakFrames;
    [SerializeField] private float breakFrameTime = 0.25f;
    [SerializeField] private string battleScene = "TurnBasedBathroomBattle";

    private bool summoning;
    private bool talking;

    private void OnEnable()
    {
        DialogueManager.TagReceived += OnTag;
    }

    private void OnDisable()
    {
        DialogueManager.TagReceived -= OnTag;
    }

    private void Start()
    {
        SetStage(0);
    }

    private void Update()
    {
        DialogueManager manager = DialogueManager.GetInstance();
        bool busy = manager == null || manager.dialogueIsPlaying;

        if (talking && !busy)
        {
            talking = false;
            if (!summoning)
            {
                SetStage(0);
            }
        }

        bool inRange = !summoning && Physics2D.OverlapBox(doorPos.position, new Vector2(width, height), 0, whatIsPlayer);
        bool show = inRange && !busy && !PauseMenu.IsPaused;
        if (prompt != null && prompt.activeSelf != show)
        {
            prompt.SetActive(show);
        }

        if (show && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Use();
        }
    }

    public void Use()
    {
        DialogueManager manager = DialogueManager.GetInstance();
        if (manager == null || manager.dialogueIsPlaying || summoning)
        {
            return;
        }
        talking = true;
        manager.EnterDialogueMode(inkJSON);
    }

    private void OnTag(string key, string value)
    {
        if (!talking)
        {
            return;
        }
        if (key == "mirror" && int.TryParse(value, out int stage))
        {
            SetStage(stage);
        }
        else if (key == "summon" && !summoning)
        {
            summoning = true;
            StartCoroutine(Summon());
        }
    }

    private void SetStage(int stage)
    {
        if (mirror != null && crackStages.Length > 0)
        {
            mirror.sprite = crackStages[Mathf.Clamp(stage, 0, crackStages.Length - 1)];
        }
    }

    private IEnumerator Summon()
    {
        Vector3 home = mirror.transform.localPosition;
        foreach (Sprite frame in breakFrames)
        {
            mirror.sprite = frame;
            for (float t = 0; t < breakFrameTime; t += Time.deltaTime)
            {
                mirror.transform.localPosition = home + (Vector3)(Random.insideUnitCircle * 0.05f);
                yield return null;
            }
        }
        mirror.transform.localPosition = home;

        // let the last line be read, then fight
        DialogueManager manager = DialogueManager.GetInstance();
        while (manager != null && manager.dialogueIsPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        FindAnyObjectByType<SceneSwitch>().SwitchScene(battleScene);
    }

    private void OnDrawGizmosSelected()
    {
        if (doorPos != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(doorPos.position, new Vector3(width, height, 1));
        }
    }
}
