using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public Animator animator;

    public float transitionTime = 1f;
    public static string prevScene;
    public static string currentScene;


    public virtual void Start() // start method is overriden or used in World.cs
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void SwitchScene(string sceneName)
    {

        prevScene = currentScene;
        SceneManager.LoadScene(sceneName);
    }

}
