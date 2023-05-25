using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private string animationName;

    [SerializeField]
    private bool magicAttack; // bool to trigger magic attack and mp consumption

    [SerializeField]
    private float magicCost; // mp cost

    [SerializeField]
    private float minAttackMultiplier; // minimum on random scale for attack

    [SerializeField]
    private float maxAttackMultiplier; // maximum on random scale for attack

    [SerializeField]
    private float minDefenseMultiplier; // minimum on random scale for defense

    [SerializeField]
    private float maxDefenseMultiplier; // maximum on random scale for defense

    [SerializeField]
    private float healAmount; // maximum on random scale for defense

    private FighterStats attackerStats;
    private FighterStats targetStats;
    private float damage = 0.0f;
    //private float xMagicNewScale;
    //private Vector2 magicScale;

    //private void Start()
    //{
    //    magicScale = GameObject.Find("HeroMagicFill").GetComponent<RectTransform>().localScale;
    //}

    public void Attack(GameObject victim)
    {
        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();

        if (attackerStats.magic >= magicCost)
        {
            float multiplier = Random.Range(minAttackMultiplier, maxAttackMultiplier);

            damage = multiplier * attackerStats.melee;
            if (magicAttack)
            {
                damage = multiplier * attackerStats.magicRange;
            }

            float defenseMultiplier = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);
            damage = Mathf.Max(0, damage - (defenseMultiplier * targetStats.defense));
            owner.GetComponent<Animator>().Play(animationName);
            targetStats.ReceiveDamage(Mathf.CeilToInt(damage));
            attackerStats.updateMagicFill(magicCost);
        }
        else
        {
            Invoke("SkipTurnContinueGame", 2);
        }
    }

    void SkipTurnContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}