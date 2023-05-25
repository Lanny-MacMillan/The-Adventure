using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    private List<FighterStats> fighterStats;

    [SerializeField]
    private GameObject battleMenu;

    public Text battleText;

    //private void Awake()
    //{
    //    battleMenu = GameObject.Find("ActionMenu");
    //}

    // Start is called before the first frame update
    void Start()
    {
        fighterStats = new List<FighterStats>(); // adding both Hero and Enemy and creating a list of stats and sorting based off speed

        GameObject hero = GameObject.FindGameObjectWithTag("Hero"); //grabs hero gameObject
        FighterStats currentFighterStats = hero.GetComponent<FighterStats>(); // sets current stats based off FighterStats on GameObject
        currentFighterStats.CalculateNextTurn(0); // calculate turn based off speed
        fighterStats.Add(currentFighterStats);

        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy"); //grabs Enemy gameObject
        FighterStats currentEnemyStats = enemy.GetComponent<FighterStats>(); // sets current stats based off FighterStats on GameObject
        currentEnemyStats.CalculateNextTurn(0); // calculate turn based off speed
        fighterStats.Add(currentEnemyStats);

        fighterStats.Sort(); // Sort fights to decide whos first
        this.battleMenu.SetActive(false); // sets battle menu to false after attack has been made

        NextTurn();

    }

    // Update is called once per frame
    public void NextTurn()
    {
        battleText.gameObject.SetActive(false);
        FighterStats currentFighterStats = fighterStats[0];
        fighterStats.Remove(currentFighterStats); // on NextTurn remove last fighter stats

        if (!currentFighterStats.GetDead())
        {
            GameObject currentUnit = currentFighterStats.gameObject; // grabs current unit gameObject
            currentFighterStats.CalculateNextTurn(currentFighterStats.nextActTurn); // calculates who's turn is next based off currentFighterStats.nextActTurn function
            fighterStats.Add(currentFighterStats); // adds new fighter stats as old tsats have been removed going into this turn
            fighterStats.Sort();

            if(currentUnit.tag == "Hero")
            {
                this.battleMenu.SetActive(true);
            }
            else
            {
                this.battleMenu.SetActive(false);
                // if randomRange is equal to 1 enemy will melee attack, 0 will magic
                string attackType = Random.Range(0, 2) == 1 ? "melee" : "magic";
                currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
            }

        }
        else
        {
            NextTurn();
        }
    }
}
