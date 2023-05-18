using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PingPong : MonoBehaviour
{

    public List<Transform> waypoints;
    public float moveSpeed;
    public int target;


    // put your data in the Update and call on it in the fixed update
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (transform.position == waypoints[target].position)
        {
            if (target == waypoints.Count - 1)
            {
                target = 0;
            }
            else
            {
                target += 1;
            }
        }

    }

}
