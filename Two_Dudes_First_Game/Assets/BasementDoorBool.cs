using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDoorBool : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("YOUVE HT THE BLUE KEY");

        //if (collision.gameObject.tag == "Player")
        //{
        //    Debug.Log("YOUVE HT THE BLUE KEY");
        //}
    }
}
