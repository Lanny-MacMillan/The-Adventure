using UnityEngine;

// First-person walking for the basement. The 2D player is hidden but kept, so hearts, damage and Game Over still work.
public class FirstPersonPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.6f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float radius = 0.35f;
    [SerializeField] private float eyeHeight = 1.05f;
    [SerializeField] private float bobAmount = 0.04f;
    [SerializeField] private string upstairsScene = "HouseInterior1stFloor";
    [SerializeField] private GameObject doorPrompt;

    public static FirstPersonPlayer Instance { get; private set; }
    public PlayerHealth Health { get; private set; }
    public bool Moving { get; private set; }

    private SpriteRenderer body2D;
    private PlayerMovement move2D;
    private Rigidbody2D rb2D;
    private float yaw;
    private float bob;
    private bool leaving;
    private Vector3 push;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Health = CarriedPlayer();
        if (Health != null)
        {
            body2D = Health.GetComponent<SpriteRenderer>();
            move2D = Health.GetComponent<PlayerMovement>();
            rb2D = Health.GetComponent<Rigidbody2D>();
        }
        Hide2D(true);

        BasementMaze maze = BasementMaze.Instance;
        if (maze != null)
        {
            transform.position = maze.StartPosition + Vector3.up * eyeHeight;
            yaw = maze.StartYaw;
        }
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    private void Update()
    {
        // Try Again turns the 2D player back on, so keep it hidden down here
        Hide2D(true);

        bool frozen = PauseMenu.IsPaused || Health == null || Health.isDead || leaving || Dialogue() || BasementEnding.Active;
        Moving = false;
        if (frozen)
        {
            return;
        }

        float turn = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f) + (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);
        float forward = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f) - (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1f : 0f);
        float strafe = (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f);

        yaw += turn * turnSpeed * Time.deltaTime + Input.GetAxis("Mouse X") * mouseSensitivity;
        Vector3 dir = Quaternion.Euler(0f, yaw, 0f) * new Vector3(strafe, 0f, forward);
        if (dir.sqrMagnitude > 1f)
        {
            dir.Normalize();
        }
        Walk(dir * moveSpeed * Time.deltaTime + push * Time.deltaTime);
        push = Vector3.MoveTowards(push, Vector3.zero, 24f * Time.deltaTime);

        Moving = dir.sqrMagnitude > 0.01f;
        bob = Moving ? bob + Time.deltaTime * 9f : Mathf.MoveTowards(bob, Mathf.Round(bob / Mathf.PI) * Mathf.PI, Time.deltaTime * 6f);
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, eyeHeight + Mathf.Abs(Mathf.Sin(bob)) * bobAmount, p.z);
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        bool atDoor = FacingDoor();
        if (doorPrompt != null && doorPrompt.activeSelf != atDoor)
        {
            doorPrompt.SetActive(atDoor);
        }
        if (atDoor && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            GoUpstairs();
        }
    }

    // lose a heart and get shoved away from whatever hit you
    public void Hurt(Vector3 from)
    {
        if (Health == null || Health.isDead || BasementEnding.Active)
        {
            return;
        }
        int before = Health.health;
        Health.TakeDamage(1, Vector2.zero);
        if (Health.health < before)
        {
            Vector3 away = transform.position - from;
            away.y = 0f;
            push = (away.sqrMagnitude < 0.01f ? -transform.forward : away.normalized) * 6f;
        }
    }

    // slide along walls by moving each axis on its own
    private void Walk(Vector3 delta)
    {
        BasementMaze maze = BasementMaze.Instance;
        Vector3 p = transform.position;
        Vector3 tryX = p + new Vector3(delta.x, 0f, 0f);
        if (maze == null || !maze.Blocked(tryX, radius))
        {
            p = tryX;
        }
        Vector3 tryZ = p + new Vector3(0f, 0f, delta.z);
        if (maze == null || !maze.Blocked(tryZ, radius))
        {
            p = tryZ;
        }
        transform.position = p;
    }

    private bool FacingDoor()
    {
        BasementMaze maze = BasementMaze.Instance;
        if (maze == null)
        {
            return false;
        }
        Vector3 ahead = transform.position + transform.forward * (radius + 1.0f);
        Vector2Int c = maze.ToCell(ahead);
        return maze.Cell(c.x, c.y) == 'D';
    }

    private void GoUpstairs()
    {
        leaving = true;
        Sound.Play("doorOpen");
        Hide2D(false);
        SceneSwitch sceneSwitch = FindAnyObjectByType<SceneSwitch>();
        if (sceneSwitch != null)
        {
            sceneSwitch.SwitchScene(upstairsScene);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(upstairsScene);
        }
    }

    private void OnDestroy()
    {
        Hide2D(false);
    }

    private void Hide2D(bool hide)
    {
        if (hide && leaving)
        {
            return;
        }
        if (body2D != null && body2D.enabled == hide)
        {
            body2D.enabled = !hide;
        }
        if (move2D != null && move2D.enabled == hide && (hide || !Health.isDead))
        {
            move2D.enabled = !hide;
        }
        if (rb2D != null && rb2D.simulated == hide)
        {
            rb2D.simulated = !hide;
        }
    }

    private static bool Dialogue()
    {
        DialogueManager manager = DialogueManager.GetInstance();
        return manager != null && manager.dialogueIsPlaying;
    }

    // the player carried over from earlier scenes, not this scene's placeholder copy
    private static PlayerHealth CarriedPlayer()
    {
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                return p;
            }
        }
        return FindAnyObjectByType<PlayerHealth>();
    }
}
