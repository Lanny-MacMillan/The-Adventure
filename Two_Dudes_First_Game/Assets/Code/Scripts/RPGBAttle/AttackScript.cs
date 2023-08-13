using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;

    [SerializeField]
    private string animationName; // passes in animationName from the SerializedField, allows for more dynamic calling of boss/hero animations

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

            owner.GetComponent<Animator>().Play(animationName); // plays player or boss animations

            targetStats.ReceiveDamage(Mathf.CeilToInt(damage)); // damage animation itself is delayed, 1s
            attackerStats.UpdateMagicFill(magicCost);
        }
        else
        {
            Invoke(nameof(SkipTurnContinueGame), 4);
        }
    }

    public void Heal(GameObject owner)
    {
        targetStats = owner.GetComponent<FighterStats>();
        targetStats.ReceiveHeal(30);

        Invoke(nameof(SkipTurnContinueGame), 4);
    }

    public void Shield(GameObject owner)
    {
        targetStats = owner.GetComponent<FighterStats>();
        targetStats.ReceiveShield(100);

        Invoke(nameof(SkipTurnContinueGame), 4);
    }

    void SkipTurnContinueGame()
    {
        GameObject.Find("GameControllerObject").GetComponent<GameController>().NextTurn();
    }
}