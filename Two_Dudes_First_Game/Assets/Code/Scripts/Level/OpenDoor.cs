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
        sceneSwitch = FindObjectOfType<SceneSwitch>();
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
            if (requiredKey == "None" && Input.GetKeyDown(KeyCode.Return))
            {
                    sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bedroomKey" && bedroomKeyObtained == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "woodshedKey" && woodshedKeyObtained == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("woodshedKeyObtained: " + woodshedKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bathroomKey" && bathroomKeyObtained == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("bathroomKeyObtained: " + bathroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "treehouseKey" && treehouseKeyObtained == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("treehouseKeyObtained: " + treehouseKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "basementKey" && basementKeyObtained == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("basementKeyObtained: " + basementKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            return;
        }
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(doorPos.position, new Vector3(width, height, 1));
    }
}
