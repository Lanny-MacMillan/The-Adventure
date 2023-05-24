using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cissy : MonoBehaviour
{
    public Animator animator;
    public bool RightSide = false;
    public bool LeftSide = true;
    public PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cissys Eyes based n player location
        if (playerMovement.lastX >= 1.80)
        {
            RightSide = true;
            LeftSide = false;
            animator.SetBool("RightSide", true);
            animator.SetBool("LeftSide", false);
        }
        if (playerMovement.lastX < 1.80)
        {
            LeftSide = true;
            RightSide = false;
            animator.SetBool("LeftSide", true);
            animator.SetBool("RightSide", false);
        }
    }
}
