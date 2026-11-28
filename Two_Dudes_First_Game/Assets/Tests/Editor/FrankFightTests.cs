using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// Both ways of beating Frank have to save the right flags, unlock the exit and hand over the Basement key.
public class FrankFightTests : PlayTestBase
{
    private FrankBoss frank;

    private IEnumerator FightFrank()
    {
        yield return StartOnTheWorldMap();
        yield return Load("Treehouse", "WorldMap");
        frank = Object.FindAnyObjectByType<FrankBoss>();
        Assert.IsNotNull(frank, "Frank isn't in the Treehouse");

        // he wakes up first; you can't hurt him until he turns
        float giveUp = Time.time + 15f;
        while (!Get<bool>(frank, "fighting") && Time.time < giveUp)
        {
            Player().health = Player().maxHealth;
            yield return null;
        }
        Assert.IsTrue(Get<bool>(frank, "fighting"), "the fight never started");

        giveUp = Time.time + 40f;
        while (!Get<bool>(frank, "dead") && Time.time < giveUp)
        {
            Player().health = Player().maxHealth;
            frank.TakeSalt(1);
            yield return Seconds(0.05f);
        }
        Assert.IsTrue(Get<bool>(frank, "dead"), "Frank never went down");

        // the cure ends in a chat; the key shows up after it
        giveUp = Time.time + 20f;
        while (BasementKey() == null && Time.time < giveUp)
        {
            Player().health = Player().maxHealth;
            DialogueManager dm = DialogueManager.GetInstance();
            if (dm != null && dm.dialogueIsPlaying)
            {
                yield return FinishDialogue();
            }
            yield return null;
        }
    }

    // the reward is a key spawner that drops the key and removes itself
    private static ItemWorld BasementKey()
    {
        foreach (ItemWorld w in Object.FindObjectsByType<ItemWorld>(FindObjectsInactive.Exclude))
        {
            if (w.GetItem() != null && w.GetItem().itemType == Item.ItemType.BasementKey)
            {
                return w;
            }
        }
        return null;
    }

    [UnityTest]
    public IEnumerator KillingFrankUnlocksTheWayOut()
    {
        Prepare(false);
        SetFlags("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "treehouseDoor");
        yield return new EnterPlayMode();
        yield return FightFrank();
        Assert.AreEqual(1, PlayerPrefs.GetInt("frankDefeated"), "frankDefeated");
        Assert.AreEqual(0, PlayerPrefs.GetInt("frankCured"), "frankCured");
        Assert.IsNotNull(BasementKey(), "the Basement key should appear");
        Assert.IsTrue(Get<Behaviour>(frank, "exitDoor").enabled, "the exit should unlock");
    }

    [UnityTest]
    public IEnumerator CuringFrankUnlocksTheWayOut()
    {
        Prepare(false);
        SetFlags("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "treehouseDoor",
            "motorGear", "motorPlug", "motorBelt", "motorGearInstalled", "motorPlugInstalled", "motorBeltInstalled", "fountainFixed", "holyWater");
        yield return new EnterPlayMode();
        yield return FightFrank();
        Assert.AreEqual(1, PlayerPrefs.GetInt("frankDefeated"), "frankDefeated");
        Assert.AreEqual(1, PlayerPrefs.GetInt("frankCured"), "frankCured");
        Assert.IsNotNull(BasementKey(), "Frank should hand over the Basement key");
        Assert.IsTrue(Get<Behaviour>(frank, "exitDoor").enabled, "the exit should unlock");
    }
}
