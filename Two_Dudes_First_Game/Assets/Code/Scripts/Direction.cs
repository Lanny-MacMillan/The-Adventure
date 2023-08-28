using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Direction : MonoBehaviour
{
    public Transform leftWayPoint; //
    public Transform rightWayPoint; //
    //Transform leftWayPoint, rightWayPoint;
    Vector3 localScale;
    bool movingRight = false;
    Rigidbody2D rb;
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        //leftWayPoint = GameObject.Find("LeftWayPoint").GetComponent<Transform>();
        //rightWayPoint = GameObject.Find("RightWayPoint").GetComponent<Transform>();
    }

    void Update()
    {

        if (transform.position.x > rightWayPoint.position.x)
            movingRight = false;

        if (transform.position.x < leftWayPoint.position.x)
            movingRight = true;

        if (movingRight == true)
            moveRight();

        if (movingRight == false)
            moveLeft();

    }
    void moveRight()
    {
        movingRight = true;
        localScale.x = -1;
        transform.localScale = localScale;
    }
    void moveLeft()
    {
        movingRight = false;
        localScale.x = 1;
        transform.localScale = localScale;
    }

}
