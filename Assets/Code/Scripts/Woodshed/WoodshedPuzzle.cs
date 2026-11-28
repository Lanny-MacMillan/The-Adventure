using System.Collections;
using UnityEngine;

// each lever flips itself + neighbours; match the wall-scratch pattern to drop the key (all up = jam + reset)
public class WoodshedPuzzle : MonoBehaviour
{
    [SerializeField] private WoodshedLever[] levers;
    [SerializeField] private bool[] target = { true, false, true, true };
    [SerializeField] private string solvedFlag = "woodshedSolved";

    [Header("Key drop")]
    [SerializeField] private Transform[] dropShelves; // top to bottom
    [SerializeField] private float floorY = -8.0f;
    [SerializeField] private float shelfDelay = 0.25f;
    [SerializeField] private Item.ItemType reward = Item.ItemType.BathroomKey;

    [Header("Jam / reset")]
    [SerializeField] private float resetDelay = 0.6f;
    [SerializeField] private float shakeAmount = 0.06f;

    private bool[] state;
    private bool busy;

    public bool Solved { get; private set; }

    private void Start()
    {
        state = new bool[levers.Length];
        Solved = PlayerPrefs.GetInt(solvedFlag) == 1;
        if (Solved)
        {
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = target[i];
            }
            foreach (Transform shelf in dropShelves)
            {
                shelf.gameObject.SetActive(false);
            }
            ItemWorld key = FindKey();
            if (key != null)
            {
                key.transform.position = new Vector3(key.transform.position.x, floorY, key.transform.position.z);
            }
        }
        Refresh();
    }

    public void Pull(int index)
    {
        if (Solved || busy)
        {
            return;
        }

        for (int i = index - 1; i <= index + 1; i++)
        {
            if (i >= 0 && i < state.Length)
            {
                state[i] = !state[i];
            }
        }
        Refresh();

        if (Matches(target))
        {
            Solved = true;
            PlayerPrefs.SetInt(solvedFlag, 1);
            PlayerPrefs.Save();
            StartCoroutine(DropKey());
        }
        else if (AllUp())
        {
            StartCoroutine(Jam());
        }
    }

    public bool LeverUp(int index)
    {
        return state[index];
    }

    private IEnumerator Jam()
    {
        busy = true;
        Vector3[] start = new Vector3[levers.Length];
        for (int i = 0; i < levers.Length; i++)
        {
            start[i] = levers[i].transform.localPosition;
        }
        float t = 0f;
        while (t < resetDelay)
        {
            for (int i = 0; i < levers.Length; i++)
            {
                levers[i].transform.localPosition = start[i] + (Vector3)(Random.insideUnitCircle * shakeAmount);
            }
            t += Time.deltaTime;
            yield return null;
        }
        for (int i = 0; i < levers.Length; i++)
        {
            levers[i].transform.localPosition = start[i];
            state[i] = false;
        }
        Refresh();
        busy = false;
    }

    private IEnumerator DropKey()
    {
        ItemWorld key = FindKey();
        foreach (Transform shelf in dropShelves)
        {
            yield return new WaitForSeconds(shelfDelay);
            shelf.gameObject.SetActive(false);
            if (key != null)
            {
                float nextY = floorY;
                foreach (Transform below in dropShelves)
                {
                    if (below.gameObject.activeSelf)
                    {
                        nextY = below.position.y + 0.25f;
                        break;
                    }
                }
                key.transform.position = new Vector3(key.transform.position.x, nextY, key.transform.position.z);
            }
        }
    }

    private ItemWorld FindKey()
    {
        foreach (ItemWorld item in FindObjectsByType<ItemWorld>(FindObjectsInactive.Exclude))
        {
            if (item.GetItem() != null && item.GetItem().itemType == reward)
            {
                return item;
            }
        }
        return null;
    }

    private void Refresh()
    {
        for (int i = 0; i < levers.Length; i++)
        {
            levers[i].Show(state[i]);
        }
    }

    private bool Matches(bool[] pattern)
    {
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] != pattern[i])
            {
                return false;
            }
        }
        return true;
    }

    private bool AllUp()
    {
        foreach (bool up in state)
        {
            if (!up)
            {
                return false;
            }
        }
        return true;
    }
}
