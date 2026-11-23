using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Dark stage with both characters. Hover (or left/right arrows) puts a stage light on one
// and darkens the other. Click (or Enter) picks that character and starts the game.
public class CharacterSelectMenu : MonoBehaviour
{
    [SerializeField] private CharacterOption[] options;
    [SerializeField] private string firstLevelScene = "WorldMap";
    [SerializeField] private float startDelay = 0.6f;

    private CharacterOption hovered;
    private bool starting;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Refresh();
    }

    private void Update()
    {
        if (starting)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Step(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Step(1);
        }

        if (hovered != null && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            Choose(hovered);
        }
    }

    private void Step(int direction)
    {
        int index = System.Array.IndexOf(options, hovered);
        if (index < 0)
        {
            index = direction > 0 ? 0 : options.Length - 1;
        }
        else
        {
            index = Mathf.Clamp(index + direction, 0, options.Length - 1);
        }
        Hover(options[index]);
    }

    public void Hover(CharacterOption option)
    {
        if (starting)
        {
            return;
        }
        hovered = option;
        Refresh();
    }

    public void Unhover(CharacterOption option)
    {
        if (starting || hovered != option)
        {
            return;
        }
        hovered = null;
        Refresh();
    }

    public void Choose(CharacterOption option)
    {
        if (starting)
        {
            return;
        }
        starting = true;
        hovered = option;
        Refresh();

        CharacterSelection.Selected = option.characterId;
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(startDelay);
        SceneManager.LoadScene(firstLevelScene);
    }

    private void Refresh()
    {
        foreach (CharacterOption option in options)
        {
            if (hovered == null)
            {
                option.SetState(CharacterOption.State.Idle);
            }
            else if (option == hovered)
            {
                option.SetState(CharacterOption.State.Lit);
            }
            else
            {
                option.SetState(CharacterOption.State.Dark);
            }
        }
    }
}
