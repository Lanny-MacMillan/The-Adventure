using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterStats : MonoBehaviour, IComparable
{

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

    void Awake()
    {
        healthTransform = healthFill.GetComponent<RectTransform>();
        healthScale = healthFill.transform.localScale;

        magicTransform = magicFill.GetComponent<RectTransform>();
        magicScale = magicFill.transform.localScale;

        startHealth = health;
        startMagic = magic;

        GameControllerObj = GameObject.Find("GameControllerObject");
    }
    public void ReceiveHeal(float heal)
    {
        Debug.Log("Heal Received!!");
        health = health + heal;
    }

    public void ReceiveDamage(float damage)
    {
        Debug.Log("Damage Received!!");
        health = health - damage;
        //animator.Play("Damage");

        // Set damage text

        if ( health <= 0 )
        {
            dead = true;
            gameObject.tag = "Dead";
            Destroy(healthFill);
            Destroy(gameObject); // destroy player gameObject
        } else if ( damage > 0 )
        {
            xNewHealthScale = healthScale.x * (health / startHealth);
            healthFill.transform.localScale = new Vector2(xNewHealthScale, healthScale.y); 
        }
        if ( damage > 0 )
        {
            GameControllerObj.GetComponent<GameController>().battleText.gameObject.SetActive(true);
            GameControllerObj.GetComponent<GameController>().battleText.text = damage.ToString();

        }
        Invoke("ContinueGame", 4);
    }

    public void updateMagicFill(float cost)
    {
        if ( cost > 0 )
        {
            magic = magic - cost;
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
