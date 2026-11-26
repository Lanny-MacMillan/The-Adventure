using UnityEngine;

// A handful of salt flying down the corridor. Stops at walls and anything it hits.
public class SaltShot : MonoBehaviour
{
    private Vector3 velocity;
    private bool holy;
    private float life = 1.6f;
    private SpriteRenderer sr;

    public void Launch(Vector3 dir, float speed, bool holyWater, Sprite sprite, Material mat)
    {
        velocity = dir * speed;
        holy = holyWater;
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sharedMaterial = mat;
        sr.color = holy ? new Color(0.55f, 0.85f, 1f, 1f) : Color.white;
        transform.localScale = Vector3.one * 2.2f;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }
        Vector3 next = transform.position + velocity * Time.deltaTime;
        life -= Time.deltaTime;

        BasementMaze maze = BasementMaze.Instance;
        if (life <= 0f || (maze != null && maze.Blocked(next, 0.08f)))
        {
            Destroy(gameObject);
            return;
        }
        transform.position = next;

        for (int i = BasementEnemy.All.Count - 1; i >= 0; i--)
        {
            BasementEnemy e = BasementEnemy.All[i];
            if (e != null && e.Contains(next) && e.TakeSalt(holy))
            {
                Sound.Play("saltHit");
                Destroy(gameObject);
                return;
            }
        }

        Camera cam = Camera.main;
        if (cam != null)
        {
            transform.rotation = cam.transform.rotation;
        }
        transform.Rotate(0f, 0f, 400f * Time.deltaTime, Space.Self);
    }
}
