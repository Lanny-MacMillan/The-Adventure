using UnityEngine;

// one lever on a shelf; stand next to it and press Enter/Up/W
[RequireComponent(typeof(Collider2D))]
public class WoodshedLever : MonoBehaviour
{
    [SerializeField] private WoodshedPuzzle puzzle;
    [SerializeField] private int index;
    [SerializeField] private SpriteRenderer leverSprite;
    [SerializeField] private GameObject prompt;

    private bool playerInRange;

    private void Start()
    {
        if (prompt != null)
        {
            prompt.SetActive(false);
        }
    }

    private void Update()
    {
        bool canUse = playerInRange && !PauseMenu.IsPaused && puzzle != null && !puzzle.Solved;
        if (prompt != null && prompt.activeSelf != canUse)
        {
            prompt.SetActive(canUse);
        }

        if (canUse && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            Pull();
        }
    }

    public void Pull()
    {
        puzzle.Pull(index);
    }

    // up = handle tilted right (as drawn), down = mirrored
    public void Show(bool up)
    {
        leverSprite.flipX = !up;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerHealth>() != null)
        {
            playerInRange = false;
        }
    }
}
