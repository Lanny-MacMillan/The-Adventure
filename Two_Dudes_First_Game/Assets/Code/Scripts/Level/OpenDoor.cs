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

    public string SceneName { get { return sceneName; } }
    public string RequiredKey { get { return requiredKey; } }
    public Vector2 SpawnPoint { get { return doorPos != null ? (Vector2)doorPos.position : (Vector2)transform.position; } }


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

        if (playerDetected && UsePressed() && HasKey())
        {
            sceneSwitch.SwitchScene(sceneName);
        }
    }

    // blank or "None" means the door isn't locked
    public bool HasKey()
    {
        switch (requiredKey)
        {
            case null:
            case "":
            case "None":         return true;
            case "kitchenKey":   return PlayerPrefs.GetInt("kitchenDoor") == 1;
            case "bedroomKey":   return PlayerPrefs.GetInt("bedroomDoor") == 1;
            case "woodshedKey":  return PlayerPrefs.GetInt("woodshedDoor") == 1;
            case "bathroomKey":  return PlayerPrefs.GetInt("bathroomDoor") == 1;
            case "treehouseKey": return PlayerPrefs.GetInt("treehouseDoor") == 1;
            case "basementKey":  return PlayerPrefs.GetInt("basementDoor") == 1;
            default:             return false;
        }
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
