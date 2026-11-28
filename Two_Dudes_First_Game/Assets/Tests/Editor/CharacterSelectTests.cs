using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// Picking a dude saves the choice, starts the game on the World Map, and the player wears that dude's sprites.
public class CharacterSelectTests : PlayTestBase
{
    private IEnumerator PickFromAFreshRun(int character)
    {
        yield return StartOnTheWorldMap();
        // a fresh run, like Quit to Title: nothing carried over, so the World Map makes a new player
        var endRun = typeof(GameManagerScript).GetMethod("EndRun", BindingFlags.NonPublic | BindingFlags.Instance);
        endRun.Invoke(Object.FindAnyObjectByType<GameManagerScript>(), new object[] { "CharacterSelect" });
        yield return WaitForScene("CharacterSelect", 5f);
        Assert.AreEqual("CharacterSelect", Scene());

        CharacterSelectMenu menu = Object.FindAnyObjectByType<CharacterSelectMenu>();
        CharacterOption option = Get<CharacterOption[]>(menu, "options").First(o => o.characterId == character);
        menu.Choose(option);
        yield return WaitForScene("WorldMap", 5f);
        yield return Seconds(0.5f);
    }

    private static bool WearsDude1(PlayerHealth p)
    {
        PlayerSkin skin = p.GetComponent<PlayerSkin>();
        var dude1 = Get<RuntimeAnimatorController>(skin, "dude1Animations");
        return p.GetComponent<Animator>().runtimeAnimatorController == dude1;
    }

    [UnityTest]
    public IEnumerator PickingDude1PlaysAsDude1()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        yield return PickFromAFreshRun(CharacterSelection.Dude1);
        Assert.AreEqual("WorldMap", Scene());
        Assert.AreEqual(CharacterSelection.Dude1, CharacterSelection.Selected, "choice should be saved");
        Assert.IsTrue(WearsDude1(Player()), "player should wear Dude1's sprites");
    }

    [UnityTest]
    public IEnumerator PickingTheDevGuyPlaysAsTheDevGuy()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        yield return PickFromAFreshRun(CharacterSelection.DevGuy);
        Assert.AreEqual("WorldMap", Scene());
        Assert.AreEqual(CharacterSelection.DevGuy, CharacterSelection.Selected, "choice should be saved");
        Assert.IsFalse(WearsDude1(Player()), "player should wear the Dev Guy's sprites");
    }
}
