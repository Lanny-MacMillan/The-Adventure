using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    [Header("Keys")]
    [SerializeField]
    private Sprite bedroomKey; // 1st
    [SerializeField]
    private Sprite woodshedKey; // 2nd
    [SerializeField]
    private Sprite bathroomKey; // 3rd
    [SerializeField]
    private Sprite treehouseKey; // 4th
    [SerializeField]
    private Sprite basementKey; // 5th
    [SerializeField]
    private Sprite cross; // 6th



    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems ()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }


        int x = 58;
        int y = -284;
        float itemSlotCellSize = 85;

        foreach (Item item in inventory.GetItemList())
        {
            // creates new itemSlotContainer and itemSlotTemplate sets itemSlotTemplate to active
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // places newly created itemSlot Container and template based off x, y position set in this function above.
            // Places Item sprite w/in the 'image' component
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            if (image.sprite == bedroomKey)
            {
                PlayerPrefs.SetInt("bedroomDoor", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR : " + image.sprite);
            }
            else if (image.sprite == woodshedKey)
            {
                PlayerPrefs.SetInt("woodshedDoor", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR : " + image.sprite);
            }
            else if (image.sprite == bathroomKey)
            {
                PlayerPrefs.SetInt("bathroomDoor", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR : " + image.sprite);
            }
            else if (image.sprite == treehouseKey)
            {
                PlayerPrefs.SetInt("treehouseDoor", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR : " + image.sprite);
            }
            else if (image.sprite == basementKey)
            {
                PlayerPrefs.SetInt("basementDoor", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR : " + image.sprite);
            }
            else if (image.sprite == cross)
            {
                PlayerPrefs.SetInt("cross", 1);
                Debug.Log("SET PLAYER PREFS TO 1 FOR POWERUP: " + image.sprite);
            }

            // adds one to x (which just logs how many items youve picked up
            x++; 

            // if inventory is greater than 6 this changes the x, y position for another row or column, whatever the UI calls for
            if (x > 6)
            {
                x = 58;
                // adds one to y, size is based off itemSlotCellSize set above
                y++; 
            }
        }
    }
}