using UnityEngine;

// Indoors the room fills the screen like an old TV instead of the camera following you.
// Rooms wider than the screen slide sideways with you and stop at the ends; the camera never moves up or down.
public class RoomCamera : MonoBehaviour
{
    [SerializeField] private SpriteRenderer room;  // the room art; its bounds are the frame
    [SerializeField] private Vector2 areaMin;      // used instead when there's no single room sprite
    [SerializeField] private Vector2 areaMax;
    [SerializeField] private float follow = 0.12f;

    private Camera cam;
    private Transform player;
    private Bounds frame;
    private bool wide;
    private float aspect;
    private float velocity;

    private void Awake()
    {
        cam = Camera.main;
        if (cam == null)
        {
            enabled = false;
            return;
        }
        // the follow camera carried in from the World Map would keep steering this one
        var brain = cam.GetComponent<Unity.Cinemachine.CinemachineBrain>();
        if (brain != null)
        {
            brain.enabled = false;
        }
        frame = room != null ? room.bounds : new Bounds((areaMin + areaMax) / 2f, areaMax - areaMin);
        Fit();
    }

    private void Start()
    {
        foreach (PlayerHealth p in FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude))
        {
            if (p.gameObject.scene.name == "DontDestroyOnLoad")
            {
                player = p.transform;
            }
        }
        if (player == null)
        {
            var p = FindAnyObjectByType<PlayerHealth>();
            player = p != null ? p.transform : null;
        }
        if (wide && player != null)
        {
            Place(Clamp(player.position.x));
        }
    }

    private void Fit()
    {
        aspect = cam.aspect;
        float size = frame.extents.y;
        wide = frame.size.x > size * 2f * aspect;
        if (!wide)
        {
            size = Mathf.Max(size, frame.extents.x / aspect);
        }
        cam.orthographic = true;
        cam.orthographicSize = size;
        Place(wide && player != null ? Clamp(player.position.x) : frame.center.x);
    }

    private float Clamp(float x)
    {
        float half = cam.orthographicSize * aspect;
        return Mathf.Clamp(x, frame.min.x + half, frame.max.x - half);
    }

    private void Place(float x)
    {
        cam.transform.position = new Vector3(x, frame.center.y, cam.transform.position.z);
    }

    // fixed rooms are only touched when the window changes shape, so screen shakes still work
    private void LateUpdate()
    {
        if (!Mathf.Approximately(cam.aspect, aspect))
        {
            Fit();
        }
        if (wide && player != null)
        {
            float x = Mathf.SmoothDamp(cam.transform.position.x, Clamp(player.position.x), ref velocity, follow);
            Place(x);
        }
    }
}
