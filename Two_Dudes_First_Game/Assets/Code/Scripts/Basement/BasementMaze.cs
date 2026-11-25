using System.Collections.Generic;
using UnityEngine;

// Builds the first-person basement from a text map.
// # stone wall, B barred wall, D door upstairs, S start (facing up), . floor, G ghost, b bat, ^ spikes, T crusher, F green flame.
public class BasementMaze : MonoBehaviour
{
    [TextArea(12, 30)]
    [SerializeField] private string map =
        "###############\n" +
        "#..G..#...T...#\n" +
        "#.###.#.#####.#\n" +
        "#.#...#.#.b.#.#\n" +
        "#.#.###.#.#.#F#\n" +
        "#.#..b.T#.#...#\n" +
        "#.#####.#.###B#\n" +
        "#..^..#.#...#.#\n" +
        "#####.#.###.#.#\n" +
        "#.F...#.G.#.#.#\n" +
        "#.#######.#.#.#\n" +
        "#.#..b.^..#...#\n" +
        "#.#.#########.#\n" +
        "#S#..F..G..^..#\n" +
        "#D#############";
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private float wallHeight = 2f;
    [SerializeField] private Material stone;
    [SerializeField] private Material bars;
    [SerializeField] private Material door;
    [SerializeField] private Material floor;
    [SerializeField] private Material ceiling;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject bat;
    [SerializeField] private GameObject spikes;
    [SerializeField] private GameObject crusher;
    [SerializeField] private GameObject flame;

    public static BasementMaze Instance { get; private set; }
    public float CellSize => cellSize;
    public Vector3 StartPosition { get; private set; }
    public float StartYaw { get; private set; }

    private string[] rows;

    private void Awake()
    {
        Instance = this;
        rows = map.Replace("\r", "").Split('\n');
        Build();
    }

    public char Cell(int x, int z)
    {
        int r = rows.Length - 1 - z;
        if (r < 0 || r >= rows.Length || x < 0 || x >= rows[r].Length)
        {
            return '#';
        }
        return rows[r][x];
    }

    public bool Solid(int x, int z)
    {
        char c = Cell(x, z);
        return c == '#' || c == 'B' || c == 'D';
    }

    public Vector2Int ToCell(Vector3 world)
    {
        return new Vector2Int(Mathf.FloorToInt(world.x / cellSize + 0.5f), Mathf.FloorToInt(world.z / cellSize + 0.5f));
    }

    public Vector3 ToWorld(int x, int z)
    {
        return new Vector3(x * cellSize, 0f, z * cellSize);
    }

    // circle vs grid, used for walking and for salt hitting walls
    public bool Blocked(Vector3 p, float radius)
    {
        Vector2Int c = ToCell(p);
        for (int x = c.x - 1; x <= c.x + 1; x++)
        {
            for (int z = c.y - 1; z <= c.y + 1; z++)
            {
                if (!Solid(x, z))
                {
                    continue;
                }
                float h = cellSize / 2f;
                float nx = Mathf.Clamp(p.x, x * cellSize - h, x * cellSize + h);
                float nz = Mathf.Clamp(p.z, z * cellSize - h, z * cellSize + h);
                if ((p.x - nx) * (p.x - nx) + (p.z - nz) * (p.z - nz) < radius * radius)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void Build()
    {
        var meshes = new Dictionary<Material, MeshBuilder>();
        MeshBuilder For(Material m)
        {
            if (!meshes.TryGetValue(m, out MeshBuilder b))
            {
                b = new MeshBuilder();
                meshes[m] = b;
            }
            return b;
        }

        float h = cellSize / 2f;
        for (int z = 0; z < rows.Length; z++)
        {
            for (int x = 0; x < rows[rows.Length - 1 - z].Length; x++)
            {
                char c = Cell(x, z);
                if (Solid(x, z))
                {
                    continue;
                }
                Vector3 o = ToWorld(x, z);
                if (c == 'S')
                {
                    StartPosition = o;
                    StartYaw = 0f;
                }
                Spawn(c == 'G' ? ghost : c == 'b' ? bat : c == '^' ? spikes : c == 'T' ? crusher : c == 'F' ? flame : null, o);

                For(floor).Quad(o + new Vector3(-h, 0, -h), o + new Vector3(-h, 0, h), o + new Vector3(h, 0, h), o + new Vector3(h, 0, -h));
                For(ceiling).Quad(o + new Vector3(-h, wallHeight, h), o + new Vector3(-h, wallHeight, -h), o + new Vector3(h, wallHeight, -h), o + new Vector3(h, wallHeight, h));

                // a wall face on every side that touches a solid cell, facing into this cell
                Wall(For, x, z + 1, o + new Vector3(-h, 0, h), o + new Vector3(h, 0, h));
                Wall(For, x, z - 1, o + new Vector3(h, 0, -h), o + new Vector3(-h, 0, -h));
                Wall(For, x + 1, z, o + new Vector3(h, 0, h), o + new Vector3(h, 0, -h));
                Wall(For, x - 1, z, o + new Vector3(-h, 0, -h), o + new Vector3(-h, 0, h));
            }
        }

        foreach (var pair in meshes)
        {
            GameObject go = new GameObject(pair.Key != null ? pair.Key.name : "Maze");
            go.transform.SetParent(transform, false);
            go.AddComponent<MeshFilter>().sharedMesh = pair.Value.ToMesh();
            go.AddComponent<MeshRenderer>().sharedMaterial = pair.Key;
        }
    }

    private void Spawn(GameObject template, Vector3 at)
    {
        if (template == null)
        {
            return;
        }
        GameObject go = Instantiate(template, at + template.transform.localPosition, template.transform.rotation, transform);
        go.SetActive(true);
    }

    private void Wall(System.Func<Material, MeshBuilder> builder, int x, int z, Vector3 left, Vector3 right)
    {
        if (!Solid(x, z))
        {
            return;
        }
        char c = Cell(x, z);
        Material m = c == 'B' ? bars : c == 'D' ? door : stone;
        Vector3 up = Vector3.up * wallHeight;
        builder(m).Quad(left, left + up, right + up, right);
    }

    private class MeshBuilder
    {
        private readonly List<Vector3> verts = new List<Vector3>();
        private readonly List<Vector2> uvs = new List<Vector2>();
        private readonly List<int> tris = new List<int>();

        // corners: bottom-left, top-left, top-right, bottom-right as seen from the front
        public void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            int i = verts.Count;
            verts.Add(a); verts.Add(b); verts.Add(c); verts.Add(d);
            uvs.Add(new Vector2(0, 0)); uvs.Add(new Vector2(0, 1)); uvs.Add(new Vector2(1, 1)); uvs.Add(new Vector2(1, 0));
            tris.Add(i); tris.Add(i + 1); tris.Add(i + 2);
            tris.Add(i); tris.Add(i + 2); tris.Add(i + 3);
        }

        public Mesh ToMesh()
        {
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.SetVertices(verts);
            mesh.SetUVs(0, uvs);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }
    }
}
