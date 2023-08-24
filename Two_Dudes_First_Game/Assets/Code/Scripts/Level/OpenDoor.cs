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

    //// bool states will open doors in OpenDoor.cs
    [Header("Keys Obtained")]
    public bool bedroomKeyObtained;
    public bool woodshedKeyObtained;
    public bool bathroomKeyObtained;
    public bool treehouseKeyObtained;
    public bool basementKeyObtained;

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
            Debug.Log("INVENTORY_BEDROOMKEY " + bedroomKeyObtained);
            Debug.Log("INVENTORY_WOODSHEDKEY " + woodshedKeyObtained);
            //Debug.Log("INVENTORY_BATHROOMKEY " + bathroomKeyObtained);
            //Debug.Log("INVENTORY_TREEHOUSEKEY " + treehouseKeyObtained);
            //Debug.Log("INVENTORY_BASEMENTKEY " + basementKeyObtained);
            if (requiredKey == "None" && Input.GetKeyDown(KeyCode.Return))
            {
                    sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bedroomKey" && bedroomKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("bedroomKeyObtained: " + bedroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "woodshedKey" && woodshedKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("woodshedKeyObtained: " + woodshedKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "bathroomKey" && bathroomKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("bathroomKeyObtained: " + bathroomKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "treehouseKey" && treehouseKeyObtained && Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("treehouseKeyObtained: " + treehouseKeyObtained);
                sceneSwitch.SwitchScene(sceneName);
            }
            else if
                (requiredKey == "basementKey" && basementKeyObtained && Input.GetKeyDown(KeyCode.Return))
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
