using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

// Every room you can walk into: you land on the floor at each door, and the fixed room camera can see you there.
public class RoomSetupTests
{
    private static readonly string[] Rooms =
    {
        "HouseInterior1stFloor", "HouseInterior2ndFloor", "Kitchen", "Bedroom", "WoodShed",
        "Bathroom", "PostBathroom", "Attic", "Treehouse",
    };

    private static string RoomPath(string name)
    {
        return TestUtil.ScenesDir + name + ".unity";
    }

    [Test]
    public void EveryRoomHasTheRoomCamera([ValueSource(nameof(Rooms))] string room)
    {
        TestUtil.WithScene(RoomPath(room), scene =>
        {
            Assert.AreEqual(1, TestUtil.InScene<RoomCamera>(scene).Count(), room + " needs exactly one RoomCamera");
        });
    }

    // the floor right under each door's spawn point, so arriving never drops you
    [Test]
    public void EveryDoorSpawnIsOnTheFloor([ValueSource(nameof(DoorScenes))] string scenePath)
    {
        var bad = new List<string>();
        TestUtil.WithScene(scenePath, scene =>
        {
            Physics2D.SyncTransforms();
            foreach (OpenDoor door in TestUtil.InScene<OpenDoor>(scene))
            {
                Vector2 spawn = door.SpawnPoint;
                float best = float.MaxValue;
                foreach (RaycastHit2D hit in Physics2D.RaycastAll(spawn + Vector2.up * 0.5f, Vector2.down, 4f))
                {
                    if (hit.collider.gameObject.scene != scene || hit.collider.isTrigger || hit.collider.GetComponentInParent<PlayerHealth>() != null)
                    {
                        continue;
                    }
                    best = Mathf.Min(best, spawn.y - hit.point.y);
                }
                if (best > 1.5f)
                {
                    bad.Add(door.name + " -> " + door.SceneName + (best == float.MaxValue ? ": no floor below" : ": floor " + best.ToString("F2") + " below the spawn"));
                }
            }
        });
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    [Test]
    public void EveryDoorSpawnIsOnCamera([ValueSource(nameof(Rooms))] string room)
    {
        var bad = new List<string>();
        TestUtil.WithScene(RoomPath(room), scene =>
        {
            RoomCamera cam = TestUtil.InScene<RoomCamera>(scene).FirstOrDefault();
            Assert.IsNotNull(cam, room + " has no RoomCamera");
            Bounds frame = Frame(cam);
            foreach (OpenDoor door in TestUtil.InScene<OpenDoor>(scene))
            {
                Vector2 p = door.SpawnPoint;
                if (p.x < frame.min.x || p.x > frame.max.x || p.y < frame.min.y - 1f || p.y > frame.max.y)
                {
                    bad.Add(door.name + " spawn " + p + " outside " + frame.min + ".." + frame.max);
                }
            }
        });
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    private static Bounds Frame(RoomCamera cam)
    {
        var so = new UnityEditor.SerializedObject(cam);
        var room = so.FindProperty("room").objectReferenceValue as SpriteRenderer;
        if (room != null)
        {
            return room.bounds;
        }
        Vector2 min = so.FindProperty("areaMin").vector2Value;
        Vector2 max = so.FindProperty("areaMax").vector2Value;
        return new Bounds((min + max) / 2f, max - min);
    }

    private static IEnumerable<string> DoorScenes()
    {
        return Rooms.Select(RoomPath).Append(TestUtil.ScenesDir + "WorldMap.unity");
    }
}
