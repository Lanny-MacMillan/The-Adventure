using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SceneSwitch
{
    public Transform player;
    public float posX;
    public float posY;
    public string previous;

    public override void Start()
    {
        base.Start(); // this calls whats inside the start method from its base script (SceneSwitch) while running its own start method

        Transform target = CarriedPlayer();
        if (target == null)
        {
            target = player;
        }
        if (target == null)
        {
            return;
        }

        // arrive at the door that leads back to where we came from, otherwise use the spawn point
        OpenDoor backDoor = null;
        foreach (OpenDoor door in FindObjectsByType<OpenDoor>(FindObjectsInactive.Exclude))
        {
            if (door.SceneName == prevScene)
            {
                backDoor = door;
                break;
            }
        }

        if (backDoor != null)
        {
            Place(target, backDoor.SpawnPoint);
        }
        else if (prevScene == previous)
        {
            Place(target, new Vector2(posX, posY));
        }
    }

    private static void Place(Transform target, Vector2 position)
    {
        target.position = new Vector3(position.x, position.y, target.position.z);
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = position;
            rb.linearVelocity = Vector2.zero;
        }
    }

    // the player carried over from earlier scenes, not this scene's placeholder copy (which deletes itself)
    private static Transform CarriedPlayer()
    {
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                return p.transform;
            }
        }
        return null;
    }
}
