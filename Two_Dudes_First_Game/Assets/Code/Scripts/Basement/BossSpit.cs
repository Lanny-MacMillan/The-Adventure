using UnityEngine;

// A glob of green goo the boss spits down the room. Strafe out of the way.
public class BossSpit : MonoBehaviour
{
    private Vector3 velocity;
    private float life = 4f;
    private SpriteRenderer sr;

    public void Launch(Vector3 dir, float speed, Sprite sprite, Material mat)
    {
        velocity = dir.normalized * speed;
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sharedMaterial = mat;
        transform.localScale = Vector3.one * 1.6f;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        life -= Time.deltaTime;
        Vector3 next = transform.position + velocity * Time.deltaTime;
        BasementMaze maze = BasementMaze.Instance;
        if (life <= 0f || (maze != null && maze.Blocked(next, 0.1f)))
        {
            Destroy(gameObject);
            return;
        }
        transform.position = next;

        FirstPersonPlayer p = FirstPersonPlayer.Instance;
        if (p != null)
        {
            Vector3 d = p.transform.position - next;
            if (d.x * d.x + d.z * d.z < 0.45f * 0.45f)
            {
                p.Hurt(next);
                Destroy(gameObject);
                return;
            }
        }
        Camera cam = Camera.main;
        if (cam != null)
        {
            transform.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);
        }
    }
}
