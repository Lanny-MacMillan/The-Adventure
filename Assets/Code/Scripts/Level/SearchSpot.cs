using System.Collections;
using UnityEngine;

// Something hidden you have to search for, like the belt behind the shower curtain. Shows a bubble, press up and it drops out.
public class SearchSpot : MonoBehaviour
{
    [SerializeField] private Vector2 size = new Vector2(1.2f, 2f);
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private GameObject prompt;
    [SerializeField] private Item item;
    [SerializeField] private string requiredFlag;
    [SerializeField] private Transform dropFrom;
    [SerializeField] private float floorY;
    [SerializeField] private string searchSound = "batFlap";

    private bool found;

    private void Start()
    {
        bool unlocked = string.IsNullOrEmpty(requiredFlag) || PlayerPrefs.GetInt(requiredFlag) == 1;
        found = !unlocked || PlayerPrefs.GetInt(item.PrefKey()) == 1;
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (found)
        {
            return;
        }
        DialogueManager dialogue = DialogueManager.GetInstance();
        bool busy = dialogue != null && dialogue.dialogueIsPlaying;
        bool show = !busy && !PauseMenu.IsPaused && Physics2D.OverlapBox(transform.position, size, 0, whatIsPlayer);
        if (prompt != null && prompt.activeSelf != show)
        {
            prompt.SetActive(show);
        }
        if (show && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            found = true;
            if (prompt != null)
            {
                prompt.SetActive(false);
            }
            Sound.Play(searchSound);
            StartCoroutine(Drop());
        }
    }

    private IEnumerator Drop()
    {
        Vector3 from = dropFrom != null ? dropFrom.position : transform.position;
        ItemWorld world = ItemWorld.SpawnItemWorld(from, item);
        Collider2D col = world.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        float speed = 0f;
        while (world != null && world.transform.position.y > floorY)
        {
            speed += 18f * Time.deltaTime;
            Vector3 p = world.transform.position;
            p.y = Mathf.Max(floorY, p.y - speed * Time.deltaTime);
            world.transform.position = p;
            yield return null;
        }
        if (col != null)
        {
            col.enabled = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, size);
    }
}
