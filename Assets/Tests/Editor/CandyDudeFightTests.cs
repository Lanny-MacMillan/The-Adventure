using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// The turn-based bathroom fight hides the player; both outcomes have to bring you back in one piece.
public class CandyDudeFightTests : PlayTestBase
{
    private static FighterStats Fighter(string tag)
    {
        return Object.FindObjectsByType<FighterStats>(FindObjectsInactive.Exclude).FirstOrDefault(f => f.CompareTag(tag));
    }

    [UnityTest]
    public IEnumerator LosingSendsYouBackToTheCar()
    {
        Prepare(false);
        SetFlags("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor");
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        yield return Load("TurnBasedBathroomBattle", "Bathroom");
        yield return Seconds(1f);
        Fighter("Hero").ReceiveDamage(9999f);

        yield return WaitForScene("RPGDeathtoBathroom", 10f);
        yield return WaitForScene("WorldMap", 15f);
        yield return Seconds(0.5f);
        Assert.AreEqual("WorldMap", Scene());
        PlayerHealth p = Player();
        Vector3 start = GameObject.Find("StartPoint").transform.position;
        Assert.Less(Mathf.Abs(p.transform.position.x - start.x), 0.2f, "should wake up at the car");
        Assert.IsTrue(p.gameObject.activeInHierarchy && p.GetComponent<SpriteRenderer>().enabled, "player should be back after the fight");
        Assert.IsFalse(DisableGlobals.InBattle, "HUD and player should be restored");
        Assert.AreEqual(1, PlayerPrefs.GetInt("bathroomDoor"), "keys are kept");
    }

    [UnityTest]
    public IEnumerator WinningWrecksTheBathroom()
    {
        Prepare(false);
        SetFlags("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor");
        yield return new EnterPlayMode();
        yield return StartOnTheWorldMap();
        yield return Load("TurnBasedBathroomBattle", "Bathroom");
        yield return Seconds(1f);
        Fighter("Enemy").ReceiveDamage(9999f);

        yield return WaitForScene("PostBathroom", 15f);
        yield return Seconds(1f);
        Assert.AreEqual("PostBathroom", Scene());
        Assert.AreEqual(1, PlayerPrefs.GetInt("bathroomCleared"), "the bathroom should count as cleared");
        PlayerHealth p = Player();
        Assert.IsTrue(p != null && p.gameObject.activeInHierarchy, "player should be back after the fight");
    }
}
