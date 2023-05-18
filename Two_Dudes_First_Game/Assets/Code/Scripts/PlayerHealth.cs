using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;

    public SpriteRenderer playerChar;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            playerChar.enabled = false;
            playerMovement.enabled = false;
            //Destroy(gameObject);
        }
    }
}
