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

    [SerializeField]
    private GameObject shieldPrefab;

    //[SerializeField]
    //private GameObject defendPrefab;

    //[SerializeField]
    //private Sprite faceIcon;

    // Projectile
    public SaltBehaviour ProjectilePrefab;
    public Transform LaunchOffset;
    public Transform LaunchOffsetMagic;
    public Transform LaunchOffsetHeal;
    public Transform LaunchOffsetShield;

    private GameObject currentAttack;
    private GameObject meleeAttack;
    private GameObject magicAttack;
    private GameObject healSelf;
    private GameObject defend;

    private FighterStats fighterStats;

    public Animator animator;
    public Animator animatorMagic;
    public Animator animatorHeal;
    public Animator animatorShield;

    private AttackScript attackScript;
    private GameController gameController;
    private GameObject battleMenu;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        battleMenu = GameObject.Find("ActionMenu");
    }

    public void SelectAttack(string btn)
    { 
        GameObject victim = hero;

        if ( tag == "Hero" )
        {
            victim = enemy;
        }

        if (btn.CompareTo("melee") == 0) // compares to zero to get a yes or no
        {
            currentAttack = magicAttack;
            if (tag == "Hero")
            {
                animator.SetBool("Salt", true); // player animation
            }
            if (tag == "Enemy")
            {
                animator.SetBool("Hook", true); // boss animation
            }

            Invoke("ResetAnimationConditionals", 2); // Delay so animation finishes before hit lands or damage shows
            meleePrefab.GetComponent<AttackScript>().Attack(victim); // Attack

            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
        else if (btn.CompareTo("magic") == 0)
        {
            if (tag == "Hero")
            {
                animatorMagic.SetBool("Magic", true); // need conditional to render button off if no mana
            }
            if (tag == "Enemy")
            {
                animatorMagic.SetBool("BossMagic", true); // boss animation
            }

            Invoke("ResetAnimationConditionals", 2); // Delay so animation finishes before hit lands or damage shows
            magicPrefab.GetComponent<AttackScript>().Attack(victim); // Magic Attack

            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
        else if (btn.CompareTo("heal") == 0)
        {
            animatorHeal.SetBool("Heal", true); // need conditional to render button off after one use

            Invoke("ResetAnimationConditionals", 2); // Delay so animation finishes before heal lands
            healPrefab.GetComponent<AttackScript>().Heal(hero); // Heal
            
            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
        else
        {
            //animatorShield.SetBool("PlayerShield", true); // need conditional to render button off after one use

            Invoke("ResetAnimationConditionals", 2); // Delay so animation finishes before heal lands
            shieldPrefab.GetComponent<AttackScript>().Shield(hero); // Shield and animation for hero in the FighterStats

            //animatorShield.SetBool("Defend", true);
            // add 100 defense and return to base def after boss turn
            Debug.Log("Defend!");

            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
    }

    private void ResetAnimationConditionals()
    {
        if (tag == "Hero")
        {
            animator.SetBool("Salt", false);
            animatorMagic.SetBool("Magic", false);
            animatorHeal.SetBool("Heal", false);
            //animatorShield.SetBool("Shield", false);
            Debug.Log("RESET PLAYER ANIMATIONS!");

        }
        else if (tag == "Enemy")
        {
            animator.SetBool("Hook", false); 
            animatorMagic.SetBool("BossMagic", false); 
            Debug.Log("RESET BOSS ANIMATIONS!");

        }
    }

}

