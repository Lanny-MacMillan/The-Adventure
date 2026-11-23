using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private bool playerDetected;
    public Transform doorPos;
    public float width;
    public float height;

    public LayerMask whatIsPlayer;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private string requiredKey;

    SceneSwitch sceneSwitch;

    [SerializeField]
    GameObject playerPrefScript;


    //// bool states will open doors in OpenDoor.cs
    [Header("Keys Obtained")]
    public int bedroomKeyObtained;
    public int woodshedKeyObtained;
    public int bathroomKeyObtained;
    public int treehouseKeyObtained;
    public int basementKeyObtained;

    private void Start()
    {
        sceneSwitch = FindAnyObjectByType<SceneSwitch>();
    }

    private void Update()
    {
        playerDetected = Physics2D.OverlapBox(doorPos.position, new Vector2(width, height), 0, whatIsPlayer);

        bedroomKeyObtained = PlayerPrefs.GetInt("bedroomDoor");
        woodshedKeyObtained = PlayerPrefs.GetInt("woodshedDoor");
        bathroomKeyObtained = PlayerPrefs.GetInt("bathroomDoor");
        treehouseKeyObtained = PlayerPrefs.GetInt("treehouseDoor");
        basementKeyObtained = PlayerPrefs.GetInt("basementDoor");

        if (playerDetected == true)
        {
            if (requiredKey == "None" && UsePressed())
            {
                    sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bedroomKey" && bedroomKeyObtained == 1 && UsePressed())
            {
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "woodshedKey" && woodshedKeyObtained == 1 && UsePressed())
            {
                Debug.Log("woodshedKeyObtained: " + woodshedKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bathroomKey" && bathroomKeyObtained == 1 && UsePressed())
            {
                Debug.Log("bathroomKeyObtained: " + bathroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "treehouseKey" && treehouseKeyObtained == 1 && UsePressed())
            {
                Debug.Log("treehouseKeyObtained: " + treehouseKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "basementKey" && basementKeyObtained == 1 && UsePressed())
            {
                Debug.Log("basementKeyObtained: " + basementKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            return;
        }
        return;
    }

    private bool UsePressed()
    {
        return Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.W);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(doorPos.position, new Vector3(width, height, 1));
    }
}
