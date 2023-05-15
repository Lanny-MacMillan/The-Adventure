using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    // singleton
    public static ItemAssets Instance { get; private set; }

    // awake is one of Unitys startup calls, Awake is called when the script object is initialised, even if script's function  isnt currently being called on
    private void Awake()
    {
        Instance = this;
    }


    // public Sprite calls for a sprite component with the label of woodshed, bathroom etc.
    public Sprite WoodShedKey;
    public Sprite BathroomKey;
    public Sprite TreeHouseKey;
    public Sprite BasementKey;
    public Sprite BedroomKey;
    public Sprite Cross;

}
