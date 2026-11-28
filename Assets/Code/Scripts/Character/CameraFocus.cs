using UnityEngine;
using UnityEngine.SceneManagement;

// The follow camera watches this point instead of the player: it sits at your feet height whenever you're standing,
// holds still through normal jumps, and drops with you right away when you fall, so you always land framed like the start.
public class CameraFocus : MonoBehaviour
{
    [SerializeField] private Unity.Cinemachine.CinemachineVirtualCameraBase followCam;
    [SerializeField] private float riseAllowance = 2.8f; // a full jump is ~2.5

    private PlayerMovement player;
    private Transform focus;
    private float y;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        if (followCam == null)
        {
            followCam = GetComponentInChildren<Unity.Cinemachine.CinemachineVirtualCameraBase>(true);
        }
        focus = new GameObject("CameraFocus").transform;
        DontDestroyOnLoad(focus.gameObject);
        Snap();
        if (followCam != null)
        {
            followCam.Follow = focus;
        }
        SceneManager.sceneLoaded += OnLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        if (focus != null)
        {
            Destroy(focus.gameObject);
        }
    }

    private void OnLoaded(Scene s, LoadSceneMode m)
    {
        Snap();
        if (followCam != null)
        {
            followCam.PreviousStateIsValid = false;
        }
    }

    private void Snap()
    {
        y = transform.position.y;
        focus.position = transform.position;
    }

    private void LateUpdate()
    {
        float py = transform.position.y;
        if (player != null && player.Grounded)
        {
            y = py;
        }
        else if (py < y)
        {
            y = py;
        }
        else if (py > y + riseAllowance)
        {
            y = py - riseAllowance;
        }
        focus.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
