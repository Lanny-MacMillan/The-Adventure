using UnityEngine;
using UnityEngine.UI;

// Candy Dude fight: shows whichever dude you picked instead of always the Dev Guy.
public class BattleSkin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Image portrait;

    [Header("Dude1")]
    [SerializeField] private RuntimeAnimatorController dude1Animations;
    [SerializeField] private Sprite dude1Idle;
    [SerializeField] private Sprite dude1Portrait;

    private void Awake()
    {
        if (CharacterSelection.Selected != CharacterSelection.Dude1)
        {
            return;
        }
        if (animator != null && dude1Animations != null)
        {
            animator.runtimeAnimatorController = dude1Animations;
        }
        if (body != null && dude1Idle != null)
        {
            body.sprite = dude1Idle;
        }
        if (portrait != null && dude1Portrait != null)
        {
            portrait.sprite = dude1Portrait;
        }
    }
}
