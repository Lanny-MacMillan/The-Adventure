using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class FighterStats : MonoBehaviour, IComparable
{
    private const string RPGDeathtoBathroom = "RPGDeathtoBathroom";
    private const string postBathroomTransition = "Transition_PostBathroom";

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject healthFill;

    [SerializeField]
    private GameObject magicFill;

    [Header("Player Stats")]
    public float health;
    public float magic;
    public float melee;
    public float magicRange;
    public float defense;
    public float speed;
    public float experience;

    private float startHealth;
    private float startMagic;

    [HideInInspector]
    public int nextActTurn;

    private bool dead = false;

    // resize health and mana bars
    private Transform healthTransform;
    private Transform magicTransform;

    private Vector2 healthScale; // keeping track of scale so we can apply health to it
    private Vector2 magicScale; // keeping track of scale so we can apply mana to it

    private float xNewHealthScale;
    private float xNewMagicScale;

    private GameObject GameControllerObj;
    private GameObject battleMenuMagic;

    void Awake()
    {
        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        GameControllerObj = GameObject.Find("GameControllerObject");
        battleMenuMagic = GameObject.Find("MagicBtn");


    }
    void Update()
    {
        if (CompareTag("Hero") && magic < 20)
        {
            this.battleMenuMagic.SetActive(false); // Disables heal button if Hero mana beolow casting cost
        }

    }

    public void ReceiveHeal(float heal)
    {
        Debug.Log("Heal Received!!");
        health += heal;
    }

    public void ReceiveShield(float defUp)
    {
        Debug.Log("Shield Received!!");
        defense += defUp;
        animator.Play("Defend");
    }

    void BathroomScene()
    {
        //Fade in if possible so it seems like we were knocked out
        // exit transition to bathroom - follow up on WaitandChangeScript
        SceneManager.LoadScene(RPGDeathtoBathroom);
        dead = true;
        gameObject.tag = "Dead";
        Destroy(healthFill);
        Destroy(gameObject); // destroy hero gameObject

    }
    
    void PostBathroomScene()
    {
        // exit transition to bathroom - follow up on Transition_Bathroom
        SceneManager.LoadScene(postBathroomTransition);
        dead = true;
        gameObject.tag = "Dead";
        Destroy(healthFill);
        Destroy(gameObject); // destroy hero gameObject
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        animator.Play("Damage");

        if ( health <= 0 )
        {
            if (CompareTag("Hero"))
            {
                animator.Play("Death");
                Invoke(nameof(BathroomScene), 2);
            }
            if (CompareTag("Enemy"))
            {
                Invoke(nameof(PostBathroomScene), 2);

            }

        } else if ( damage > 0 )
        {
            xNewHealthScale = healthScale.x * (health / startHealth);
            healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y);

            if (CompareTag("Hero"))
            {
                // sets the hero damage in the UI to be displayed by the Battle message GameObject
                GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleText.text = ("Hero takes " + damage.ToString() + " damage");
            }
            if (CompareTag("Enemy"))
            {
                // sets the boss damage in the UI to be displayed by the Battle message GameObject
                GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleText.text = ("Boss takes " + damage.ToString() + " damage");
            }

        } else if ( damage == 0  )
        {
                // sets the boss damage in the UI to be displayed by the Battle message GameObject
                GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
                GameControllerObj.GetComponent<GameController>().battleText.text = ("Missed");
        }
        Invoke(nameof(ContinueGame), 4);
    }


    public void UpdateMagicFill(float cost)
    {
        if ( cost > 0 )
        {
            magic -= cost;
            xNewMagicScale = magicScale.x * (magic / startMagic);
            magicFill.transform.localScale = new Vector2(xNewMagicScale, magicScale.y);
        }
    }

    public bool GetDead()
    {
        return dead;
    }

    void ContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }

    public void CalculateNextTurn(int currentTurn)
    {
        nextActTurn = currentTurn + Mathf.CeilToInt(100f / speed);
    }

    public int CompareTo(object otherStats)
    {
        int nex = nextActTurn.CompareTo(((FighterStats)otherStats).nextActTurn);
        return nex;
    }
}
