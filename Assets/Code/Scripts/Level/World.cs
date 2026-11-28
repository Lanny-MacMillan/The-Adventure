using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SceneSwitch
{
    public Transform player;
    public float posX;
    public float posY;
    public string previous;

    // Try Again sends you back to the car (StartPoint on the World Map) to fight your way back
    public static bool BackToStart;

    public override void Start()
    {
        base.Start(); // this calls whats inside the start method from its base script (SceneSwitch) while running its own start method

        DisableGlobals.RestoreGlobals();

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

        if (BackToStart)
        {
            BackToStart = false;
            GameObject start = GameObject.Find("StartPoint");
            if (start != null)
            {
                Place(target, start.transform.position);
            }
        }

        // indoors the camera doesn't follow you, so never leave you standing outside the room
        RoomCamera room = FindAnyObjectByType<RoomCamera>();
        if (room != null && !room.Contains(target.position))
        {
            OpenDoor anyDoor = backDoor != null ? backDoor : FindAnyObjectByType<OpenDoor>();
            Debug.LogWarning("Arrived in " + gameObject.scene.name + " outside the room (came from '" + prevScene + "'), moved to the door.");
            if (anyDoor != null)
            {
                Place(target, anyDoor.SpawnPoint);
            }
        }
    }

    private static void Place(Transform target, Vector2 position)
    {
        position = OnGround(target, position);
        target.position = new Vector3(position.x, position.y, target.position.z);
        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = position;
            rb.linearVelocity = Vector2.zero;
        }
    }

    // stand on whatever floor is below the spawn point instead of dropping onto it
    private static Vector2 OnGround(Transform target, Vector2 position)
    {
        Collider2D body = null;
        foreach (Collider2D c in target.GetComponents<Collider2D>())
        {
            if (!c.isTrigger)
            {
                body = c;
                break;
            }
        }
        if (body == null)
        {
            return position;
        }

        float feet = body.bounds.min.y - target.position.y;
        Physics2D.SyncTransforms();
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(position + Vector2.up * 0.5f, Vector2.down, 6f))
        {
            // skip players too: a room's own placeholder player can stand right at the door
            if (hit.collider.isTrigger || hit.collider.transform.IsChildOf(target) || hit.collider.gameObject.layer == target.gameObject.layer)
            {
                continue;
            }
            return new Vector2(position.x, hit.point.y - feet + 0.01f);
        }
        return position;
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
