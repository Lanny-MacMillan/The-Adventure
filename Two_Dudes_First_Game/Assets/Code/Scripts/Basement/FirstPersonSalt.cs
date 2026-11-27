using UnityEngine;
using UnityEngine.UI;

// Your hand and salt shaker on screen: throws salt, sways while you walk, and flashes red when you get hurt.
public class FirstPersonSalt : MonoBehaviour
{
    [SerializeField] private Image hand;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite windSprite;
    [SerializeField] private Sprite throwSprite;
    [SerializeField] private Image hurtFlash;
    [SerializeField] private Sprite saltSprite;
    [SerializeField] private Material saltMaterial;
    [SerializeField] private float cooldown = 0.45f;
    [SerializeField] private float saltSpeed = 10f;

    private FirstPersonPlayer player;
    private Vector2 handHome;
    private float sway;
    private float throwTimer = 99f;
    private bool thrown;
    private int lastHealth = -1;
    private float flash;

    private void Start()
    {
        player = GetComponent<FirstPersonPlayer>();
        handHome = hand.rectTransform.anchoredPosition;
    }

    private void Update()
    {
        PlayerHealth health = player != null ? player.Health : null;
        bool canAct = health != null && !health.isDead && !PauseMenu.IsPaused && Time.timeScale > 0f;

        throwTimer += Time.deltaTime;
        if (canAct && throwTimer >= cooldown && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            throwTimer = 0f;
            thrown = false;
        }

        // wind up, let go, settle back
        if (throwTimer < 0.08f)
        {
            hand.sprite = windSprite;
        }
        else if (throwTimer < 0.3f)
        {
            hand.sprite = throwSprite;
            if (!thrown)
            {
                thrown = true;
                Throw();
            }
        }
        else
        {
            hand.sprite = idleSprite;
        }

        sway = player != null && player.Moving ? sway + Time.deltaTime * 9f : Mathf.MoveTowards(sway, 0f, Time.deltaTime * 4f);
        hand.rectTransform.anchoredPosition = handHome + new Vector2(Mathf.Sin(sway * 0.5f) * 14f, -Mathf.Abs(Mathf.Sin(sway * 0.5f)) * 10f);
        hand.enabled = health == null || !health.isDead;

        if (health != null)
        {
            if (lastHealth >= 0 && health.health < lastHealth)
            {
                flash = 1f;
            }
            lastHealth = health.health;
        }
        flash = Mathf.MoveTowards(flash, 0f, Time.deltaTime * 2.5f);
        if (hurtFlash != null)
        {
            hurtFlash.color = new Color(0.7f, 0f, 0f, flash * 0.45f);
            hurtFlash.enabled = flash > 0f;
        }
    }

    private void Throw()
    {
        bool holy = PlayerPrefs.GetInt("holyWater") == 1;
        Vector3 start = transform.position + transform.forward * 0.4f + transform.right * 0.15f - Vector3.up * 0.15f;
        GameObject go = new GameObject("Salt");
        go.transform.position = start;
        go.AddComponent<SaltShot>().Launch(transform.forward, saltSpeed, holy, saltSprite, saltMaterial);
        Sound.Play("saltThrow");
    }
}
