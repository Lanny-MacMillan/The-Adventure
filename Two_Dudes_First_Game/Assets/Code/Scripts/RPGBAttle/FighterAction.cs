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

    // Projectile
    public SaltBehaviour ProjectilePrefab;
    public Transform LaunchOffset;
    public Transform LaunchOffsetMagic;
    public Transform LaunchOffsetHeal;
    public Transform LaunchOffsetShield;

    private GameObject currentAttack;
    private readonly GameObject meleeAttack;
    private readonly GameObject magicAttack;
    private readonly GameObject healSelf;
    private readonly GameObject defend;

    private readonly FighterStats fighterStats;

    public Animator animator;
    public Animator animatorMagic;
    public Animator animatorHeal;
    public Animator animatorShield;
    public Animator animatorHero;

    private readonly AttackScript attackScript;
    private GameObject battleMenu;
    private GameObject battleMenuHeal;
    private GameObject battleMenuDefend;


    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        battleMenu = GameObject.Find("ActionMenu");
        battleMenuHeal = GameObject.Find("HealBtn");
        battleMenuDefend = GameObject.Find("DefendBtn");
    }

    public void SelectAttack(string btn)
    {
        //fighterStats.CheckMana();
        GameObject victim = hero;

        if (CompareTag("Hero"))
        {
            victim = enemy;
        }

        if (btn.CompareTo("melee") == 0) // compares to zero to get a yes or no
        {
            currentAttack = magicAttack;
            if (CompareTag("Hero"))
            {
                animator.SetBool("Salt", true); 
            }
            if (CompareTag("Enemy"))
            {
                animator.SetBool("Hook", true);
            }

            Invoke(nameof(ResetAnimationConditionals), 2); // Invoke delay so animation finishes before hit lands 
            meleePrefab.GetComponent<AttackScript>().Attack(victim); // Attack

            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
        else if (btn.CompareTo("magic") == 0)
        {
            if (CompareTag("Hero"))
            {
                animatorMagic.SetBool("Magic", true); // need conditional to render button off if no mana
            }
            if (CompareTag("Enemy"))
            {
                animatorMagic.SetBool("BossMagic", true);
            }
            Invoke(nameof(ResetAnimationConditionals), 2); // Invoke delay so animation finishes before hit lands
            magicPrefab.GetComponent<AttackScript>().Attack(victim); // Magic Attack

            this.battleMenu.SetActive(false); // Disables button action menu after click
        }
        else if (btn.CompareTo("heal") == 0)
        {
            animatorHeal.SetBool("Heal", true); // need conditional to render button off after one use

            Invoke(nameof(ResetAnimationConditionals), 2); // Invoke delay so animation finishes before heal lands
            healPrefab.GetComponent<AttackScript>().Heal(hero); // Heal

            this.battleMenu.SetActive(false); // Disables button action menu after click
            this.battleMenuHeal.SetActive(false); // Disables heal button after click

            Invoke(nameof(ResetPotion), 30);

        }
        else
        {
            Invoke(nameof(ResetAnimationConditionals), 2); // Delay so animation finishes before heal lands
            shieldPrefab.GetComponent<AttackScript>().Shield(hero); // Shield and animation for hero in FighterStats.cs

            animatorHero.SetBool("Defend", true);
            this.battleMenu.SetActive(false); // Disables button action menu after click
            this.battleMenuDefend.SetActive(false); // Disables defend button after click
        }
    }

    private void ResetPotion()
    {
        this.battleMenuHeal.SetActive(true); // Enables heal button after 3 turns
    }

    private void ResetAnimationConditionals()
    {
        if (CompareTag("Hero"))
        {
            animator.SetBool("Salt", false);
            animatorMagic.SetBool("Magic", false);
            animatorHeal.SetBool("Heal", false);
        }
        else if (CompareTag("Enemy"))
        {
            animator.SetBool("Hook", false); 
            animatorMagic.SetBool("BossMagic", false); 
        }
    }

}

