using UnityEngine;

// Bedroom: the demon drops the woodshed key out of the closet, and the room turns bloody once you have it.
public class ClosetDemonRoom : MonoBehaviour
{
    [SerializeField] private string doneFlag = "closetDemonDone";
    [SerializeField] private Item.ItemType reward = Item.ItemType.WoodShedKey;
    [SerializeField] private Transform keyDropPoint;

    [Header("Room after you get the key")]
    [SerializeField] private SpriteRenderer roomArt;
    [SerializeField] private Sprite roomArtAfter;

    private bool swapped;
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
        // left the room before grabbing the key - put it back
        if (PlayerPrefs.GetInt(doneFlag) == 1 && !HasReward())
        {
            DropKey();
        }
    }

    private void Update()
    {
        DialogueManager dialogue = DialogueManager.GetInstance();
        bool now = dialogue != null && dialogue.dialogueIsPlaying;
        if (now && !talking)
        {
            Sound.Play("closetDemon");
        }
        talking = now;

        if (!swapped && roomArt != null && roomArtAfter != null && HasReward())
        {
            roomArt.sprite = roomArtAfter;
            swapped = true;
        }
    }

    private void OnTag(string key, string value)
    {
        if (key != "give" || PlayerPrefs.GetInt(doneFlag) == 1)
        {
            return;
        }
        PlayerPrefs.SetInt(doneFlag, 1);
        PlayerPrefs.Save();
        DropKey();
    }

    private bool HasReward()
    {
        return PlayerPrefs.GetInt(new Item { itemType = reward }.PrefKey()) == 1;
    }

    private void DropKey()
    {
        ItemWorld.SpawnItemWorld(keyDropPoint != null ? keyDropPoint.position : transform.position, new Item { itemType = reward, amount = 1 });
    }
}
