using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

// The intro and outro: the outro shows the right drive-off for your dude and Frank's fate, and Esc skips both.
public class StorySceneTests : PlayTestBase
{
    private const string Outro = TestUtil.ScenesDir + "Outro.unity";

    // builds the outro's panel list as it would play, without playing it, plus the drive-off pictures it can pick from
    private static List<StoryScene.Panel> OutroPanels(int character, bool frankCured, out Dictionary<string, Sprite> drives)
    {
        int keepCharacter = CharacterSelection.Selected;
        bool keepCured = StoryScene.FrankCured;
        List<StoryScene.Panel> panels = null;
        var found = new Dictionary<string, Sprite>();
        try
        {
            CharacterSelection.Selected = character;
            StoryScene.FrankCured = frankCured;
            TestUtil.WithScene(Outro, scene =>
            {
                StoryScene story = TestUtil.InScene<StoryScene>(scene).First();
                var build = typeof(StoryScene).GetMethod("BuildPanels", BindingFlags.NonPublic | BindingFlags.Instance);
                panels = (List<StoryScene.Panel>)build.Invoke(story, null);
                var so = new SerializedObject(story);
                foreach (string f in new[] { "driveDevAlone", "driveDevFrank", "driveDude1Alone", "driveDude1Frank" })
                {
                    found[f] = (Sprite)so.FindProperty(f).objectReferenceValue;
                }
            });
        }
        finally
        {
            CharacterSelection.Selected = keepCharacter;
            StoryScene.FrankCured = keepCured;
        }
        drives = found;
        return panels;
    }

    [TestCase(CharacterSelection.DevGuy, true, "driveDevFrank")]
    [TestCase(CharacterSelection.DevGuy, false, "driveDevAlone")]
    [TestCase(CharacterSelection.Dude1, true, "driveDude1Frank")]
    [TestCase(CharacterSelection.Dude1, false, "driveDude1Alone")]
    public void OutroDrivesOffWithTheRightPanel(int character, bool frankCured, string expectedPanel)
    {
        var panels = OutroPanels(character, frankCured, out var drives);
        Assert.AreEqual("THE END", panels.Last().lines[0], "the outro should end on THE END");
        StoryScene.Panel drive = panels[panels.Count - 2];
        Assert.IsNotNull(drives[expectedPanel], expectedPanel + " has no picture");
        Assert.AreEqual(drives[expectedPanel], drive.picture, "wrong drive-off picture");
        bool frankInTheBack = drive.lines.Any(l => l.Contains("Frank sat in the back"));
        Assert.AreEqual(frankCured, frankInTheBack, "Frank's line doesn't match how his fight ended");
    }

    [UnityTest]
    public IEnumerator EscSkipsTheIntro()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        AudioListener.volume = 0f;
        yield return Load("Intro", "TitleScreen");
        yield return Seconds(0.5f);
        Set(Object.FindAnyObjectByType<StoryScene>(), "skipAll", true);
        yield return WaitForScene("CharacterSelect", 5f);
        Assert.AreEqual("CharacterSelect", Scene());
    }

    [UnityTest]
    public IEnumerator EscSkipsTheOutro()
    {
        Prepare(false);
        yield return new EnterPlayMode();
        AudioListener.volume = 0f;
        yield return Load("Outro", "Basement");
        yield return Seconds(0.5f);
        Set(Object.FindAnyObjectByType<StoryScene>(), "skipAll", true);
        yield return WaitForScene("TitleScreen", 5f);
        Assert.AreEqual("TitleScreen", Scene());
    }
}
