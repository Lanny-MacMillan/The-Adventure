using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGlobals : MonoBehaviour
{
    //public GameObject canvas;

    //public GameObject canvasHealth;

    //public GameObject playerChar;
    //GameObject[] gameObjects;

    private void Awake()
    {
        GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Global");

        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);
        }

        Cursor.visible = true; // brings back mouse for turn based combat
        Cursor.lockState = CursorLockMode.None; // brings back mouse for turn based combat

    }

}
