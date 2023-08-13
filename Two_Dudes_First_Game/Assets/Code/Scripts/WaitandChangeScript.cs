using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitandChangeScript : MonoBehaviour
{
    private const string bathroomScene = "Bathroom";

    void Start()
    {
        Invoke(nameof(BathroomScene), 5);
    }

    void BathroomScene()
    {
        //Fade in if possible so it seems like we were knocked out
        SceneManager.LoadScene(bathroomScene);
    }

}
