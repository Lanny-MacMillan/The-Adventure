using UnityEngine;

// Secret: after the Treehouse key, bring back the 3 motor pieces, the pump sprays the roof cross and its water cures Frank.
public class FountainSecret : MonoBehaviour
{
    [SerializeField] private Vector2 size = new Vector2(2.2f, 2.6f);
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private GameObject prompt;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private FountainSpray spray;
    [SerializeField] private string unlockFlag = "treehouseDoor";
    [SerializeField] private string fixedFlag = "fountainFixed";

    private static readonly Item.ItemType[] pieces = { Item.ItemType.MotorGear, Item.ItemType.MotorPlug, Item.ItemType.MotorBelt };
    private bool talking;

    private void OnEnable()
    {
        DialogueManager.TagReceived += OnTag;
    }

    private void OnDisable()
    {
        DialogueManager.TagReceived -= OnTag;
    }

    private void Update()
    {
        DialogueManager manager = DialogueManager.GetInstance();
        bool busy = manager == null || manager.dialogueIsPlaying;
        if (talking && !busy)
        {
            talking = false;
        }

        bool show = !busy && !PauseMenu.IsPaused && Physics2D.OverlapBox(transform.position, size, 0, whatIsPlayer);
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
        if (manager == null || manager.dialogueIsPlaying)
        {
            return;
        }
        talking = true;
        manager.EnterDialogueMode(inkJSON, Knot());
    }

    private string Knot()
    {
        if (PlayerPrefs.GetInt(fixedFlag) == 1)
        {
            bool needsWater = PlayerPrefs.GetInt("holyWater") == 0 && PlayerPrefs.GetInt("frankDefeated") == 0;
            return needsWater ? "fill" : "flowing";
        }
        if (PlayerPrefs.GetInt(unlockFlag) == 0)
        {
            return "dry";
        }

        // pieces you're carrying go into the pump
        Inventory inventory = CarriedInventory();
        int found = 0;
        foreach (Item.ItemType piece in pieces)
        {
            if (PlayerPrefs.GetInt(new Item { itemType = piece }.PrefKey()) == 1)
            {
                found++;
                if (inventory != null)
                {
                    inventory.RemoveItem(piece);
                }
            }
        }
        return found == pieces.Length ? "fix" : "parts_" + found;
    }

    private void OnTag(string key, string value)
    {
        if (!talking)
        {
            return;
        }
        if (key == "fountain")
        {
            PlayerPrefs.SetInt(fixedFlag, 1);
            PlayerPrefs.Save();
            Sound.Play("fountainPump");
            if (spray != null)
            {
                spray.Begin();
            }
        }
        else if (key == "holywater")
        {
            Inventory inventory = CarriedInventory();
            if (inventory != null)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.HolyWater, amount = 1 });
                Sound.Play("keyPickup");
            }
            PlayerPrefs.SetInt("holyWater", 1);
            PlayerPrefs.Save();
        }
    }

    // the player carried over from earlier scenes, not this scene's placeholder copy
    private static Inventory CarriedInventory()
    {
        PlayerMovement found = null;
        foreach (PlayerMovement p in FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude))
        {
            if (found == null || p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                found = p;
            }
        }
        return found != null ? found.GetInventory() : null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
