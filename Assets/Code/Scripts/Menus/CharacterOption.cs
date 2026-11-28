using UnityEngine;
using UnityEngine.Rendering.Universal;

// needs a Collider2D for mouse hover/click
public class CharacterOption : MonoBehaviour
{
    public int characterId;

    [SerializeField] private CharacterSelectMenu menu;
    [SerializeField] private Light2D spotLight;
    [SerializeField] private SpriteRenderer characterSprite;

    [Header("Stage light")]
    [SerializeField] private float litIntensity = 2.2f;
    [SerializeField] private float idleIntensity = 0.35f;
    [SerializeField] private float darkIntensity = 0f;
    [SerializeField] private float fadeSpeed = 8f;

    [Header("Character tint")]
    [SerializeField] private Color litColor = Color.white;
    [SerializeField] private Color idleColor = new Color(0.55f, 0.55f, 0.55f, 1f);
    [SerializeField] private Color darkColor = new Color(0.12f, 0.12f, 0.15f, 1f);

    public enum State { Idle, Lit, Dark }
    private State state = State.Idle;

    public void SetState(State newState)
    {
        state = newState;
    }

    private void Update()
    {
        float targetIntensity = idleIntensity;
        Color targetColor = idleColor;

        if (state == State.Lit)
        {
            targetIntensity = litIntensity;
            targetColor = litColor;
        }
        else if (state == State.Dark)
        {
            targetIntensity = darkIntensity;
            targetColor = darkColor;
        }

        float t = 1f - Mathf.Exp(-fadeSpeed * Time.deltaTime);

        if (spotLight != null)
        {
            spotLight.intensity = Mathf.Lerp(spotLight.intensity, targetIntensity, t);
        }

        if (characterSprite != null)
        {
            characterSprite.color = Color.Lerp(characterSprite.color, targetColor, t);
        }
    }

    private void OnMouseEnter()
    {
        menu.Hover(this);
    }

    private void OnMouseExit()
    {
        menu.Unhover(this);
    }

    private void OnMouseDown()
    {
        menu.Choose(this);
    }
}
