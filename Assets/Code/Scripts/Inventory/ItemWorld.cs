using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    // spawns preFab items in the world
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        // Instantiate looks for ItemAssets > Instance > pfItemWorld. Takes that prefab and intatiates it in a new position
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private Item item;
    private SpriteRenderer spriteRenderer;

    // Sets item
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Gets item and sets sprite
    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
