using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterAction : MonoBehaviour
{
    private GameObject hero;
    private GameObject enemy;

    [SerializeField]
    private GameObject meleePrefab;

    [SerializeField]
    private GameObject magicPrefab;

    [SerializeField]
    private GameObject healPrefab;

    //[SerializeField]
    //private GameObject defendPrefab;

    //[SerializeField]
    //private Sprite faceIcon;

    // Projectile
    public SaltBehaviour ProjectilePrefab;
    public Transform LaunchOffset;

    private GameObject currentAttack;
    private GameObject meleeAttack;
    private GameObject magicAttack;
    private GameObject healSelf;
    private GameObject defend;

    public Animator animator;
    public Animator animatorMagic;
    //private bool Melee = false;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void SelectAttack(string btn)
    { 
        Debug.Log("tag: " + tag);
        Debug.Log("tag: " + this.tag);
        GameObject victim = hero;
        Debug.Log("melee tag: " + victim.tag);

        if ( tag == "Hero" )
        {
            victim = enemy;
        }

        if (btn.CompareTo("melee") == 0) // compares to zero to get a yes or no
        {
            currentAttack = magicAttack;
            meleePrefab.GetComponent<AttackScript>().Attack(victim);
            //Instantiate(ProjectilePrefab, LaunchOffset.position, transform.rotation);
            animator.SetBool("Salt", true);
            Invoke("ResetConditionals", 2);
            Debug.Log("Melee Attack!");
        }
        else if (btn.CompareTo("magic") == 0)
        {
            // magic animation here
            magicPrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Magic Attack!");
        }
        else if (btn.CompareTo("heal") == 0)
        {
            // heal animation here
            healPrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Heal!");
        }
        else
        {
            // defend animation here
            // add 20 defense and return to base def after boss turn
            Debug.Log("Defend!");
        }
    }
    private void ResetConditionals()
    {
        animator.SetBool("Salt", false);

    }
}

