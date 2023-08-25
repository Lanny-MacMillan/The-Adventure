using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cissy : MonoBehaviour
{
    public Animator animator;
    public bool RightSide = false;
    public bool LeftSide = true;
    public PlayerMovement playerMovement;

    void Update()
    {
        // Cissys Eyes based on player location
        if (playerMovement.lastX >= 52.55)
        {
            Debug.Log("CISSYS_EYE_LOOK_RIGHT");

            RightSide = true;
            LeftSide = false;
            animator.SetBool("RightSide", true);
            animator.SetBool("LeftSide", false);
        }
        if (playerMovement.lastX < 52.55)
        {
            Debug.Log("CISSYS_EYE_LOOK_LEFT");

            LeftSide = true;
            RightSide = false;
            animator.SetBool("LeftSide", true);
            animator.SetBool("RightSide", false);
        }
    }
}
