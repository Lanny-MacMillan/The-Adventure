using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsScript : MonoBehaviour
{
    public int bedroomDoorUnlocked;

    // Start is called before the first frame update
    void Start()
    {

        PlayerPrefs.SetInt("bedroomDoor", 0);
        bedroomDoorUnlocked = PlayerPrefs.GetInt("bedroomDoor");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
