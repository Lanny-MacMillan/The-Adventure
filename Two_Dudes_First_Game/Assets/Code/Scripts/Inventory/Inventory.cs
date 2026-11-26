using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public event EventHandler OnItemListChanged;

    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        //AddItem(new Item { itemType = Item.ItemType.WoodShedKey, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.BathroomKey, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.TreeHouseKey, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.BasementKey, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.BedroomKey, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Cross, amount = 1 });
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // start of a run: carry everything the save says you've picked up, minus motor pieces already in the pump
    public void LoadOwned()
    {
        foreach (Item.ItemType type in (Item.ItemType[])Enum.GetValues(typeof(Item.ItemType)))
        {
            Item item = new Item { itemType = type, amount = 1 };
            string key = item.PrefKey();
            if (PlayerPrefs.GetInt(key) != 1 || PlayerPrefs.GetInt(key + "Installed") == 1 || itemList.Exists(i => i.itemType == type))
            {
                continue;
            }
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool RemoveItem(Item.ItemType type)
    {
        int index = itemList.FindIndex(i => i.itemType == type);
        if (index < 0)
        {
            return false;
        }
        itemList.RemoveAt(index);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
