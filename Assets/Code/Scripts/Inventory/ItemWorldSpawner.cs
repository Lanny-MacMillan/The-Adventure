using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;
    [SerializeField] private string requiredFlag; // only shows up once this is set (e.g. motor pieces after the Treehouse key)

    private void Awake()
    {
        // don't respawn keys the player already has when coming back to a room
        bool unlocked = string.IsNullOrEmpty(requiredFlag) || PlayerPrefs.GetInt(requiredFlag) == 1;
        if (unlocked && PlayerPrefs.GetInt(item.PrefKey(), 0) == 0)
        {
            ItemWorld.SpawnItemWorld(transform.position, item);
        }
        Destroy(gameObject);
    }


}
