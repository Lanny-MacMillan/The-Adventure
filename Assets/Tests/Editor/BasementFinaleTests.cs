using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// The last fight: both ways of beating the Stash have to end the game and reach the outro with the right Frank ending.
public class BasementFinaleTests : PlayTestBase
{
    private bool stashCured;
    private bool outroFrankCured;

    [UnityTest]
    public IEnumerator CuringTheStashReachesTheOutroWithFrankCured()
    {
        Prepare(false);
        SetFlags("basementDoor", "frankDefeated", "frankCured", "holyWater");
        yield return new EnterPlayMode();
        yield return BeatTheStash(true);
        Assert.AreEqual("Outro", Scene());
        Assert.IsTrue(stashCured, "holy water should cure the Stash");
        Assert.IsTrue(outroFrankCured, "Frank was cured, so he should ride along in the outro");
    }

    [UnityTest]
    public IEnumerator KillingTheStashReachesTheOutroWithoutFrank()
    {
        Prepare(false);
        SetFlags("basementDoor", "frankDefeated");
        yield return new EnterPlayMode();
        yield return BeatTheStash(false);
        Assert.AreEqual("Outro", Scene());
        Assert.IsFalse(stashCured, "plain salt should kill the Stash");
        Assert.IsFalse(outroFrankCured, "Frank was killed, so the back seat stays empty");
    }

    private IEnumerator BeatTheStash(bool holy)
    {
        yield return StartOnTheWorldMap();
        yield return Load("Basement", "HouseInterior1stFloor");
        yield return Seconds(1f);
        BasementBoss boss = Object.FindAnyObjectByType<BasementBoss>();
        Assert.IsNotNull(boss, "no boss in the Basement");
        // skip walking the maze, and make the ending's lines quick
        Set(boss, "awake", true);
        Set(Get<BasementEnding>(boss, "ending"), "lineTime", 0.05f);

        float giveUp = Time.time + 30f;
        while (!BasementEnding.Active && Time.time < giveUp)
        {
            FirstPersonPlayer.Instance.Health.health = FirstPersonPlayer.Instance.Health.maxHealth;
            boss.TakeSalt(holy);
            yield return Seconds(0.05f);
        }
        Assert.IsTrue(BasementEnding.Active, "the ending never started");
        stashCured = PlayerPrefs.GetInt("stashCured") == 1;
        yield return WaitForScene("Outro", 20f);
        outroFrankCured = StoryScene.FrankCured;
    }
}
