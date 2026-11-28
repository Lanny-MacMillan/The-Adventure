using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cissy : MonoBehaviour
{
    public Animator animator;
    public bool RightSide = false;
    public bool LeftSide = true;
    public PlayerMovement playerMovement;

    [SerializeField] private float lookThresholdX = 52.55f;

    void Update()
    {
        // the scene's own player copy deletes itself, so follow the player carried in from earlier scenes
        if (playerMovement == null || playerMovement.gameObject.scene.name != "DontDestroyOnLoad")
        {
            playerMovement = FindCarriedPlayer();
            if (playerMovement == null)
            {
                return;
            }
        }

        // Cissys Eyes based on player location
        bool right = playerMovement.transform.position.x >= lookThresholdX;
        if (right != RightSide)
        {
            RightSide = right;
            LeftSide = !right;
            animator.SetBool("RightSide", right);
            animator.SetBool("LeftSide", !right);
        }
    }

    private static PlayerMovement FindCarriedPlayer()
    {
        foreach (PlayerMovement p in FindObjectsByType<PlayerMovement>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                return p;
            }
        }
        return null;
    }
}
