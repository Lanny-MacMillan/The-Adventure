using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    private void Awake()
    {
        // don't respawn keys the player already has when coming back to a room
        if (PlayerPrefs.GetInt(item.PrefKey(), 0) == 0)
        {
            ItemWorld.SpawnItemWorld(transform.position, item);
        }
        Destroy(gameObject);
    }


}
