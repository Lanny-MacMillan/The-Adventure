using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

// Tuning and setup that's easy to knock out of place in the Inspector.
public class GameplayDataTests
{
    private const string PlayerPrefab = "Assets/Sprites/PlayChar/PreFab/PlayerChar(Master).prefab";
    private const string SaltPrefab = "Assets/Sprites/PlayChar/PreFab/ProjectileSaltMaster.prefab";
    private const string GhostPrefab = "Assets/Sprites/Mobs/GhostMob.prefab";

    private static float GhostHealth(EnemyGhost g)
    {
        return new SerializedObject(g).FindProperty("maxHealth").floatValue;
    }

    [Test]
    public void GhostsTakeTwoSaltHits()
    {
        var bad = new List<string>();
        foreach (var g in AssetDatabase.LoadAssetAtPath<GameObject>(GhostPrefab).GetComponentsInChildren<EnemyGhost>(true))
        {
            if (GhostHealth(g) != 2f) bad.Add("GhostMob prefab: " + GhostHealth(g));
        }
        foreach (string path in TestUtil.BuildScenePaths())
        {
            TestUtil.WithScene(path, scene =>
            {
                foreach (var g in TestUtil.InScene<EnemyGhost>(scene))
                {
                    if (GhostHealth(g) != 2f) bad.Add(scene.name + ": " + g.transform.parent?.name + " = " + GhostHealth(g));
                }
            });
        }
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    // a short toss that arcs down and lands, not a bullet
    [Test]
    public void SaltIsAnArcingToss()
    {
        var salt = AssetDatabase.LoadAssetAtPath<GameObject>(SaltPrefab);
        Assert.IsTrue(salt.GetComponent<BoxCollider2D>().isTrigger, "salt hits through its trigger");
        Assert.Greater(salt.GetComponent<Rigidbody2D>().gravityScale, 0f, "salt should fall in an arc");
        Assert.IsNotNull(salt.GetComponent<SaltBehaviour>());
    }

    [Test]
    public void PlayerPrefabHasItsHelpers()
    {
        var player = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefab);
        Assert.IsNotNull(player.GetComponent<CameraFocus>(), "CameraFocus");
        Assert.IsNotNull(player.GetComponent<PlayerBark>(), "PlayerBark");
        Assert.IsNotNull(player.GetComponent<DontDestroy>(), "DontDestroy");
        Assert.IsNotNull(player.GetComponent<PlayerHealth>(), "PlayerHealth");
        var bark = new SerializedObject(player.GetComponent<PlayerBark>());
        Assert.IsNotNull(bark.FindProperty("font").objectReferenceValue, "PlayerBark font");
        Assert.IsNotNull(bark.FindProperty("box").objectReferenceValue, "PlayerBark box");
    }

    // a room's spare player has to bow out before anything else picks a player (the missing-player bug)
    [Test]
    public void SparePlayersRemoveThemselvesFirst()
    {
        var order = (DefaultExecutionOrder)System.Attribute.GetCustomAttribute(typeof(DontDestroy), typeof(DefaultExecutionOrder));
        Assert.IsNotNull(order, "DontDestroy needs [DefaultExecutionOrder]");
        Assert.Less(order.order, 0);
        Assert.IsNotNull(typeof(DontDestroy).GetMethod("Awake", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic), "the check runs in Awake");
    }

    [Test]
    public void ClimbableBranchesAreOneWayGround()
    {
        var bad = new List<string>();
        int ground = LayerMask.NameToLayer("Ground");
        TestUtil.WithScene(TestUtil.ScenesDir + "WorldMap.unity", scene =>
        {
            var climb = scene.GetRootGameObjects().FirstOrDefault(r => r.name == "Woods")?.transform.Find("ClimbTrees");
            Assert.IsNotNull(climb, "Woods/ClimbTrees is missing");
            int count = 0;
            foreach (var col in climb.GetComponentsInChildren<BoxCollider2D>(true))
            {
                count++;
                var eff = col.GetComponent<PlatformEffector2D>();
                if (col.gameObject.layer != ground) bad.Add(col.transform.parent.name + "/" + col.name + " not on Ground");
                if (eff == null || !eff.useOneWay || !col.usedByEffector) bad.Add(col.transform.parent.name + "/" + col.name + " not one-way");
            }
            Assert.Greater(count, 30, "expected the woods' branches");
        });
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    [Test]
    public void TreehouseDoorSitsOnItsTree()
    {
        TestUtil.WithScene(TestUtil.ScenesDir + "WorldMap.unity", scene =>
        {
            var door = TestUtil.InScene<OpenDoor>(scene).FirstOrDefault(d => d.SceneName == "Treehouse");
            Assert.IsNotNull(door, "no door to the Treehouse");
            Assert.Greater(door.SpawnPoint.y, 0f, "the treehouse should be up in the tree");
        });
    }
}
