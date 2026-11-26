using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    //public CharacterController controller;
    public Animator animator;
    //public PlayerHealth playerHealth;

    [SerializeField] private string firstLevelScene = "WorldMap";
    [SerializeField] private string titleScene = "TitleScreen";

    private static GameManagerScript instance;
    private static readonly string[] doorKeys = { "kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "treehouseDoor", "basementDoor", "cross", "bathroomCleared", "closetDemonDone", "closetDemonTalks", "woodshedSolved", "frankDefeated", "motorGear", "motorPlug", "motorBelt", "fountainFixed", "holyWater", "frankCured", "basementCleared", "stashCured" };

    private void Awake()
    {
        // carried between scenes with the player so Game Over works indoors
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Game Over screen starts inactive so its DontDestroy never runs
        if (gameOverUI != null && gameOverUI.transform.parent == null)
        {
            DontDestroyOnLoad(gameOverUI);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //private void Awake()
    //{
    //    DontDestroyOnLoad(gameOverUI);
    //}

    void Update()
    {
        // the turn based fight, the pause menu and dialogue choices need the mouse
        DialogueManager dialogue = DialogueManager.GetInstance();
        if (DisableGlobals.InBattle || PauseMenu.IsPaused || (dialogue != null && dialogue.dialogueIsPlaying))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (gameOverUI != null && gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Sound.Music("gameover");
        Sound.Play("gameOver");
    }

    // Game Over: keep your keys, start the room again from where you came in
    public void TryAgain()
    {
        Time.timeScale = 1f;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Include))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                p.Revive();
            }
        }
        World.Retrying = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // fresh start: reset keys, clear everything carried between scenes, reload the World Map
    public void restart()
    {
        EndRun(firstLevelScene);
    }

    public void QuitToTitle()
    {
        EndRun(titleScene);
    }

    private void EndRun(string nextScene)
    {
        Time.timeScale = 1f;

        foreach (string key in doorKeys)
        {
            PlayerPrefs.SetInt(key, 0);
        }
        PlayerPrefs.Save();

        HashSet<GameObject> carriedOver = new HashSet<GameObject>();
        foreach (DontDestroy d in FindObjectsByType<DontDestroy>(FindObjectsInactive.Include))
        {
            carriedOver.Add(d.transform.root.gameObject);
        }
        foreach (SaveMe s in FindObjectsByType<SaveMe>(FindObjectsInactive.Include))
        {
            carriedOver.Add(s.transform.root.gameObject);
        }

        // deactivate before Destroy or the new copies see these and delete themselves
        foreach (GameObject go in carriedOver)
        {
            go.SetActive(false);
            Destroy(go);
        }

        instance = null;
        gameObject.SetActive(false);
        Destroy(gameObject);

        SceneManager.LoadScene(nextScene);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
