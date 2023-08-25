using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefScript : MonoBehaviour
{
    public int bedroomDoorUnlocked;
    public int woodshedDoorUnlocked;
    public int bathroomDoorUnlocked;
    public int treehouseDoorUnlocked;
    public int basementDoorUnlocked;
    public int crossAquired;


    void Start()
    {
        //if(bedroomDoorUnlocked != 1)
        //{
        //    PlayerPrefs.SetInt("bedroomDoor", 0);
        //}

        //if (woodshedDoorUnlocked != 1)
        //{
        //    PlayerPrefs.SetInt("woodshedDoor", 0);
        //}

        //if (bathroomDoorUnlocked != 1)
        //{
        //    PlayerPrefs.SetInt("bathroomDoor", 0);
        //}

        //if (treehouseDoorUnlocked != 1)
        //{
        //    PlayerPrefs.SetInt("treehouseDoor", 0);
        //}

        //if (basementDoorUnlocked != 1)
        //{
        //    PlayerPrefs.SetInt("basementDoor", 0);
        //}

        //if (crossAquired != 1)
        //{
        //    PlayerPrefs.SetInt("cross", 0);
        //}

        bedroomDoorUnlocked = PlayerPrefs.GetInt("bedroomDoor");
        woodshedDoorUnlocked = PlayerPrefs.GetInt("woodshedDoor");
        bathroomDoorUnlocked = PlayerPrefs.GetInt("bathroomDoor");
        treehouseDoorUnlocked = PlayerPrefs.GetInt("treehouseDoor");
        basementDoorUnlocked = PlayerPrefs.GetInt("basementDoor");
        crossAquired = PlayerPrefs.GetInt("cross");

    }
}
