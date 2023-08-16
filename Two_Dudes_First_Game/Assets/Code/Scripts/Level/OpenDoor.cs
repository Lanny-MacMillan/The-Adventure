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

    UI_Inventory ui_inventory;

    [SerializeField]
    GameObject inventory;

    private void Awake()
    {
        // this is used to create a gameobject to reference script with inventory
        ui_inventory = inventory.GetComponent<UI_Inventory>();
    }

    private void Start()
    {
        sceneSwitch = FindObjectOfType<SceneSwitch>();
    }

    private void Update()
    {
        playerDetected = Physics2D.OverlapBox(doorPos.position, new Vector2(width, height), 0, whatIsPlayer);

        if (playerDetected == true)
        {
            if (requiredKey == "None" && Input.GetKeyDown(KeyCode.Return))
            {
                    sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bedroomKey" && ui_inventory.bedroomKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("bedroomKeyObtained: " + ui_inventory.bedroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "woodshedKey" && ui_inventory.woodshedKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("woodshedKeyObtained: " + ui_inventory.woodshedKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bathroomKey" && ui_inventory.bathroomKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("bathroomKeyObtained: " + ui_inventory.bathroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "treehouseKey" && ui_inventory.treehouseKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("treehouseKeyObtained: " + ui_inventory.treehouseKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "basementKey" && ui_inventory.basementKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("basementKeyObtained: " + ui_inventory.basementKeyObtained);
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
