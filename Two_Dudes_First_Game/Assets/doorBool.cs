using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBool : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("YOUVE HT THE BLUE KEY");

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("YOUVE HT THE BLUE KEY");
        }
    }
}
