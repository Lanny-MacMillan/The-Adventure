using UnityEngine;

// Covers the up arrow painted on the bathroom art until the rope ladder has finished unrolling. No ladder = always covered.
public class HideUntilLadder : MonoBehaviour
{
    [SerializeField] private Animator ladder;

    private void Update()
    {
        if (ladder != null && ladder.isActiveAndEnabled && ladder.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
