using UnityEngine;

// Line of sight through the maze grid, and fake fog for sprites (sprite shaders ignore real fog).
public static class BasementSight
{
    public static bool Clear(Vector3 from, Vector3 to)
    {
        BasementMaze maze = BasementMaze.Instance;
        if (maze == null)
        {
            return true;
        }
        Vector3 d = to - from;
        d.y = 0f;
        int steps = Mathf.CeilToInt(d.magnitude / 0.25f);
        for (int i = 1; i < steps; i++)
        {
            Vector2Int c = maze.ToCell(from + d * (i / (float)steps));
            if (maze.Solid(c.x, c.y))
            {
                return false;
            }
        }
        return true;
    }

    public static void Shade(SpriteRenderer sr, Vector3 at, float minBrightness, Color tint)
    {
        Camera cam = Camera.main;
        if (sr == null || cam == null)
        {
            return;
        }
        float k = Mathf.InverseLerp(RenderSettings.fogEndDistance, RenderSettings.fogStartDistance, Vector3.Distance(cam.transform.position, at));
        float b = Mathf.Max(minBrightness, k);
        sr.color = new Color(tint.r * b, tint.g * b, tint.b * b, tint.a);
    }
}
