using UnityEngine;
using UnityEngine.SceneManagement;

// "Not Good Enough..." after losing to Candy Dude, then back to the car like any Game Over (keys and items kept)
public class WaitandChangeScript : MonoBehaviour
{
    private const string startScene = "WorldMap";

    void Start()
    {
        Invoke(nameof(BackToCar), 5);
    }

    void BackToCar()
    {
        World.BackToStart = true;
        SceneSwitch.prevScene = null;
        SceneManager.LoadScene(startScene);
    }
}
