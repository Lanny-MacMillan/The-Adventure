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
    private GameObject rangePrefab;

    [SerializeField]
    private Sprite faceIcon;

    private GameObject currentAttack;

    void Awake()
    {
        hero = GameObject.FindGameObjectWithTag("Hero");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
    public void SelectAttack(string btn)
    {
        GameObject victim = hero;
        if (tag == "Hero")
        {
            victim = enemy;
        }
        if (btn.CompareTo("melee") == 0) // compars to zero to get a yes or no
        {
            //meleePrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Melee Attack!");
        }
        else if (btn.CompareTo("magic") == 0)
        {
            //rangePrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Magic Attack!");
        }
        else if (btn.CompareTo("heal") == 0)
        {
            //rangePrefab.GetComponent<AttackScript>().Attack(victim);
            Debug.Log("Heal!");
        }
        else
        {
            Debug.Log("Run!");
        }
    }
}

