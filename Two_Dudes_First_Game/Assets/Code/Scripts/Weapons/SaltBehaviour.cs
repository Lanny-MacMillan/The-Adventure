using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltBehaviour : MonoBehaviour
{
    private float speed = 6f;
    public PlayerMovement playerMovement; // access script with player direction


    // Update is called once per frame
    private void Update()
    {
        // probably not best practice, seems resource heavy searching for the PlayerChar every frame
        if (GameObject.Find("PlayerChar").GetComponent<PlayerMovement>().isFacingRight)
        {
            // facing right, tranform salt to right side of player
            transform.position += speed * Time.deltaTime * transform.right;
        } else {
            // facing left, tranform salt to left side of player
            transform.position += speed * Time.deltaTime * -transform.right;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
            Debug.Log("Ground Hit!");
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Debug.Log("Enemy Hit!");
        }

    }

}
