using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.WoodShedKey, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.BathroomKey, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.TreeHouseKey, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.BasementKey, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.BedroomKey, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Cross, amount = 1 });

        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
