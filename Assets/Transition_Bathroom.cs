using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Bathroom : MonoBehaviour
{
    private const string bathroomScene = "PostBathroom";

    void Start()
    {
        Invoke(nameof(BathroomScene), 5);
        // insert sound of key hitting the floor
    }

    void BathroomScene()
    {
        SceneManager.LoadScene(bathroomScene);
    }
}
