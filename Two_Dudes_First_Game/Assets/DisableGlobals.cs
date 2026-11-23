using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGlobals : MonoBehaviour
{
    // HUD, inventory and the player are hidden during the turn based fight and brought back by World.cs in the next room
    private static readonly List<GameObject> hidden = new List<GameObject>();

    public static bool InBattle { get; private set; }

    private void Awake()
    {
        InBattle = true;

        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Global");
        foreach (GameObject go in gameObjectArray)
        {
            Hide(go);
        }

        // otherwise the player keeps falling through the battle scene
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                Hide(p.gameObject);
            }
        }

        Cursor.visible = true; // brings back mouse for turn based combat
        Cursor.lockState = CursorLockMode.None; // brings back mouse for turn based combat
    }

    private static void Hide(GameObject go)
    {
        go.SetActive(false);
        if (!hidden.Contains(go))
        {
            hidden.Add(go);
        }
    }

    public static void RestoreGlobals()
    {
        InBattle = false;
        foreach (GameObject go in hidden)
        {
            if (go != null)
            {
                go.SetActive(true);
            }
        }
        hidden.Clear();
    }
}
