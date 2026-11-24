using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item {

    public enum ItemType
    {
        WoodShedKey,
        BathroomKey,
        TreeHouseKey,
        BasementKey,
        BedroomKey,
        Cross,
        KitchenKey,
        MotorGear,
        MotorPlug,
        MotorBelt,
        HolyWater,
    }

    public ItemType itemType;
    public int amount;


    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.WoodShedKey:      return ItemAssets.Instance.WoodShedKey;
            case ItemType.BathroomKey:      return ItemAssets.Instance.BathroomKey;
            case ItemType.TreeHouseKey:     return ItemAssets.Instance.TreeHouseKey;
            case ItemType.BasementKey:      return ItemAssets.Instance.BasementKey;
            case ItemType.BedroomKey:       return ItemAssets.Instance.BedroomKey;
            case ItemType.Cross:            return ItemAssets.Instance.Cross;
            case ItemType.KitchenKey:       return ItemAssets.Instance.KitchenKey;
            case ItemType.MotorGear:        return ItemAssets.Instance.MotorGear;
            case ItemType.MotorPlug:        return ItemAssets.Instance.MotorPlug;
            case ItemType.MotorBelt:        return ItemAssets.Instance.MotorBelt;
            case ItemType.HolyWater:        return ItemAssets.Instance.HolyWater;
        }
    }

    // PlayerPrefs flag set when this item is picked up
    public string PrefKey()
    {
        switch (itemType)
        {
            case ItemType.WoodShedKey:      return "woodshedDoor";
            case ItemType.BathroomKey:      return "bathroomDoor";
            case ItemType.TreeHouseKey:     return "treehouseDoor";
            case ItemType.BasementKey:      return "basementDoor";
            case ItemType.BedroomKey:       return "bedroomDoor";
            case ItemType.KitchenKey:       return "kitchenDoor";
            case ItemType.MotorGear:        return "motorGear";
            case ItemType.MotorPlug:        return "motorPlug";
            case ItemType.MotorBelt:        return "motorBelt";
            case ItemType.HolyWater:        return "holyWater";
            default:                        return "cross";
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            //case ItemType.HealthPotion:
            //    return true;
            case ItemType.WoodShedKey:
            case ItemType.BathroomKey:
            case ItemType.TreeHouseKey:
            case ItemType.BasementKey:
            case ItemType.BedroomKey:
            case ItemType.Cross:
            case ItemType.KitchenKey:
                return false;
        }
    }
}
