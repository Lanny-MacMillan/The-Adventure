using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    // the one carried between scenes; a room's spare copy never takes over (it gets deleted right after)
    private static ItemAssets instance;
    public static ItemAssets Instance
    {
        get
        {
            if (instance == null)
            {
                foreach (ItemAssets a in FindObjectsByType<ItemAssets>(FindObjectsInactive.Exclude))
                {
                    if (instance == null || a.gameObject.scene.name == "DontDestroyOnLoad")
                    {
                        instance = a;
                    }
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null || gameObject.scene.name == "DontDestroyOnLoad")
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public Transform pfItemWorld;

    // public Sprite calls for a sprite component with the label of woodshed, bathroom etc.
    public Sprite WoodShedKey;
    public Sprite BathroomKey;
    public Sprite TreeHouseKey;
    public Sprite BasementKey;
    public Sprite BedroomKey;
    public Sprite Cross;
    public Sprite KitchenKey;
    public Sprite MotorGear;
    public Sprite MotorPlug;
    public Sprite MotorBelt;
    public Sprite HolyWater;

}
