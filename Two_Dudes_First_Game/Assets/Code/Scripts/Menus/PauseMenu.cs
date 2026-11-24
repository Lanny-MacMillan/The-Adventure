using UnityEngine;

// Esc or P. Lives on the GameManager so it travels between scenes with it.
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameManagerScript gameManager;

    public static bool IsPaused { get; private set; }

    private void Start()
    {
        SetPaused(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (IsPaused)
            {
                Resume();
            }
            else if (CanPause())
            {
                SetPaused(true);
            }
        }
    }

    private bool CanPause()
    {
        if (DisableGlobals.InBattle || (gameManager != null && gameManager.gameOverUI != null && gameManager.gameOverUI.activeInHierarchy))
        {
            return false;
        }
        DialogueManager dialogue = DialogueManager.GetInstance();
        if (dialogue != null && dialogue.dialogueIsPlaying)
        {
            return false;
        }
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        return player == null || !player.isDead;
    }

    public void Resume()
    {
        SetPaused(false);
    }

    public void Restart()
    {
        SetPaused(false);
        gameManager.restart();
    }

    public void QuitToTitle()
    {
        SetPaused(false);
        gameManager.QuitToTitle();
    }

    private void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        if (pausePanel != null)
        {
            pausePanel.SetActive(paused);
        }
    }

    private void OnDestroy()
    {
        if (IsPaused)
        {
            IsPaused = false;
            Time.timeScale = 1f;
        }
    }
}
