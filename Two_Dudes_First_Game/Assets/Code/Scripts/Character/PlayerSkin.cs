using UnityEngine;

// Swaps the player's look to the character picked on the select screen.
// Only the sprites and animations change - every other script on the player stays the same.
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
