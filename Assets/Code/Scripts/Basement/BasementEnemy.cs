using System.Collections.Generic;
using UnityEngine;

// Anything down in the basement that salt can hit. Its sprites always turn to face the camera, like Doom.
public abstract class BasementEnemy : MonoBehaviour
{
    public static readonly List<BasementEnemy> All = new List<BasementEnemy>();

    [SerializeField] protected float hitRadius = 0.45f;
    [SerializeField] protected float hitHeight = 1.8f;
    [SerializeField] protected Transform billboard;

    protected virtual void OnEnable()
    {
        All.Add(this);
    }

    protected virtual void OnDisable()
    {
        All.Remove(this);
    }

    public bool Contains(Vector3 p)
    {
        Vector3 d = p - transform.position;
        return d.x * d.x + d.z * d.z < hitRadius * hitRadius && d.y > -0.2f && d.y < hitHeight;
    }

    // true if the salt was used up
    public abstract bool TakeSalt(bool holy);

    protected virtual void LateUpdate()
    {
        Camera cam = Camera.main;
        if (billboard != null && cam != null)
        {
            billboard.rotation = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f);
        }
    }

    protected static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }
}
