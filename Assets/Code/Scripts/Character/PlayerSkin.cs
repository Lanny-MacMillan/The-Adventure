using UnityEngine;

// swaps sprites/animations to the selected character, other scripts untouched
public class PlayerSkin : MonoBehaviour
{
    [Header("Dude1 (alt character)")]
    [SerializeField] private RuntimeAnimatorController dude1Animations;
    [SerializeField] private Sprite dude1IdleSprite;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

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

        if (spriteRenderer != null && dude1IdleSprite != null)
        {
            spriteRenderer.sprite = dude1IdleSprite;
        }
    }
}
