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

    // keys 1-4 press the matching button (only while it's showing)
    private static readonly string[] buttonNames = { "AttackBtn", "MagicBtn", "HealBtn", "DefendBtn" };
    private readonly Button[] buttons = new Button[4];

    private void Awake()
    {
        battleMenu = GameObject.Find("ActionMenu");
        for (int i = 0; i < buttonNames.Length; i++)
        {
            GameObject b = GameObject.Find(buttonNames[i]);
            if (b != null)
            {
                buttons[i] = b.GetComponent<Button>();
            }
        }
    }


    void Start()
    {
        Invoke(nameof(DelayedStartForAnimation), 4); // This delays the initial battle functions so the intro animation for the boss can play

    }

    void Update()
    {
        if (battleMenu == null || !battleMenu.activeInHierarchy)
        {
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                && buttons[i] != null && buttons[i].gameObject.activeInHierarchy && buttons[i].interactable)
            {
                buttons[i].onClick.Invoke();
                return;
            }
        }
    }

    void DelayedStartForAnimation()
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

        NextTurn();
    }

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

            if(currentUnit.CompareTag("Hero"))
            {
                currentUnit.GetComponent<FighterAction>().HeroTurnStarted();
                this.battleMenu.SetActive(true);
            }
            else
            {
                this.battleMenu.SetActive(false);
                // if randomRange is equal to 1 enemy will melee attack, 0 will magic
                string attackType = Random.Range(0, 2) == 1 ? "melee" : "magic";
                if (attackType == "magic" && !currentUnit.GetComponent<FighterAction>().CanCastMagic())
                {
                    attackType = "melee"; // out of mana
                }
                currentUnit.GetComponent<FighterAction>().SelectAttack(attackType);
            }
        }
        else
        {
            NextTurn();
        }
    }

    public void DisableButtons ()
    {
        this.battleMenu.SetActive(false);
    }
}
