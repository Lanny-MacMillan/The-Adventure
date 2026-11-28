using UnityEngine;

// Only shown when you're playing a certain dude (the other dude's head in the toilet, etc).
public class ShowForCharacter : MonoBehaviour
{
    [SerializeField] private int character = CharacterSelection.Dude1;

    private void Awake()
    {
        gameObject.SetActive(CharacterSelection.Selected == character);
    }
}
