using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

// Plays the real game for a few seconds at a time: doors, salt vs ghosts, hearts, the splash.
public class PlayFlowTests : PlayTestBase
{
    private static readonly string[][] Doors =
    {
        new[] { "Treehouse", "WorldMap" }, new[] { "WorldMap", "Treehouse" },
        new[] { "HouseInterior1stFloor", "WorldMap" }, new[] { "Kitchen", "HouseInterior1stFloor" },
        new[] { "HouseInterior1stFloor", "Kitchen" }, new[] { "HouseInterior2ndFloor", "HouseInterior1stFloor" },
        new[] { "Bedroom", "HouseInterior2ndFloor" }, new[] { "HouseInterior2ndFloor", "Bedroom" },
        new[] { "PostBathroom", "HouseInterior2ndFloor" }, new[] { "WoodShed", "WorldMap" },
        new[] { "WorldMap", "WoodShed" }, new[] { "HouseInterior1stFloor", "Basement" },
        new[] { "WorldMap", "HouseInterior1stFloor" },
    };

    [UnityTest]
    public IEnumerator EveryDoorArrivalLandsOnePlayerAtTheDoor()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        var bad = new System.Collections.Generic.List<string>();
        foreach (string[] pair in Doors)
        {
            PlayerHealth p = Player();
            p.health = p.maxHealth;
            yield return Load(pair[0], pair[1]);
            int players = Object.FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude).Length;
            Vector2 start = p.transform.position;
            OpenDoor door = Object.FindObjectsByType<OpenDoor>(FindObjectsInactive.Exclude).FirstOrDefault(d => d.SceneName == pair[1]);
            yield return Seconds(0.8f);
            float drop = start.y - p.transform.position.y;
            string tag = pair[1] + " -> " + pair[0] + ": ";
            if (players != 1) bad.Add(tag + players + " players");
            if (door == null) bad.Add(tag + "no door back");
            else if (Mathf.Abs(start.x - door.SpawnPoint.x) > 0.1f) bad.Add(tag + "arrived at x " + start.x.ToString("F2") + ", door at " + door.SpawnPoint.x.ToString("F2"));
            if (drop > 0.1f) bad.Add(tag + "dropped " + drop.ToString("F2"));
        }
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    [UnityTest]
    public IEnumerator TwoSaltTossesKillAGhostThatCantHurtYouWhileDying()
    {
        Prepare(true);
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        PlayerHealth p = Player();
        PlayerMovement pm = p.GetComponent<PlayerMovement>();
        foreach (var s in Object.FindObjectsByType<EvilSpirit>(FindObjectsInactive.Exclude)) s.gameObject.SetActive(false);
        foreach (var b in Object.FindObjectsByType<AtticBat>(FindObjectsInactive.Exclude)) b.gameObject.SetActive(false);
        EnemyGhost ghost = GameObject.Find("GhostMob_Lawn").transform.Find("Ghost").GetComponent<EnemyGhost>();
        for (int i = 0; i < 2; i++)
        {
            Vector3 g = ghost.transform.position;
            SaltBehaviour salt = Object.Instantiate(pm.ProjectilePrefab, new Vector3(g.x - 1f, g.y, 0f), Quaternion.identity);
            salt.facingRight = true;
            yield return Seconds(0.6f);
        }
        bool dead = ghost.enemyDead;
        bool harmless = ghost.GetComponents<Collider2D>().All(c => !c.enabled) && !ghost.GetComponent<EnemyDamage>().enabled;
        p.health = p.maxHealth;
        p.GetComponent<Rigidbody2D>().position = ghost.transform.position;
        yield return Seconds(0.4f);
        int health = p.health;
        int max = p.maxHealth;
        Assert.IsTrue(dead, "ghost still alive after two tosses");
        Assert.IsTrue(harmless, "dying ghost still has colliders or damage on");
        Assert.AreEqual(max, health, "touching a dying ghost hurt");
    }

    [UnityTest]
    public IEnumerator AHeartGivesOneBack()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        PlayerHealth p = Player();
        GameObject heart = GameObject.Find("Woods").transform.Find("Mobs/Heart_Canopy").gameObject;
        heart.SetActive(true);
        p.health = 1;
        p.GetComponent<Rigidbody2D>().position = (Vector2)heart.transform.position + Vector2.up * 0.2f;
        p.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        yield return Seconds(0.8f);
        int health = p.health;
        Assert.AreEqual(2, health);
    }

    [UnityTest]
    public IEnumerator SplashGoesToTheTitle()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        yield return Load("CreatedBy", "");
        float giveUp = Time.time + 8f;
        while (SceneManager.GetActiveScene().name != "TitleScreen" && Time.time < giveUp)
        {
            yield return null;
        }
        string scene = SceneManager.GetActiveScene().name;
        Assert.AreEqual("TitleScreen", scene);
    }
}
