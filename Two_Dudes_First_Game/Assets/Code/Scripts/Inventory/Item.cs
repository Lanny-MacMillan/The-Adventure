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
        }
    }
}
