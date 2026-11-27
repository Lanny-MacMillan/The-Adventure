using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

// The packaged game only knows the scenes in Build Settings, so every scene a door or script asks for has to be in there.
public class BuildScenesTests
{
    [Test]
    public void GameStartsOnTheSplash()
    {
        Assert.AreEqual(TestUtil.ScenesDir + "CreatedBy.unity", TestUtil.BuildScenePaths().FirstOrDefault());
    }

    [Test]
    public void EveryBuildSceneExists()
    {
        var missing = TestUtil.BuildScenePaths().Where(p => !File.Exists(p));
        Assert.IsEmpty(missing, TestUtil.Describe(missing));
    }

    [Test]
    public void NoSceneIsListedTwice()
    {
        var dupes = TestUtil.BuildScenePaths().GroupBy(p => p).Where(g => g.Count() > 1).Select(g => g.Key);
        Assert.IsEmpty(dupes, TestUtil.Describe(dupes));
    }

    [Test]
    public void ScenesNamedInCodeAreInTheBuild()
    {
        var build = TestUtil.BuildSceneNames();
        var named = TestUtil.Matches(@"(?:LoadScene|SwitchScene)\(\s*""([A-Za-z0-9_]+)""")
            .Concat(TestUtil.Matches(@"string\s+\w*[Ss]cene\w*\s*=\s*""([A-Za-z0-9_]+)"""));
        var missing = named.Where(n => !build.Contains(n.value)).Select(n => n.value + " (" + n.file + ")").Distinct();
        Assert.IsEmpty(missing, "Not in Build Settings:\n" + TestUtil.Describe(missing));
    }

    // doors, triggers, story scenes and the game manager keep their target scene in a field with "scene" in its name; rooms keep a fallback in World.previous
    [Test]
    public void ScenesNamedInScenesAreInTheBuild()
    {
        var build = TestUtil.BuildSceneNames();
        var missing = new System.Collections.Generic.List<string>();
        foreach (string path in TestUtil.BuildScenePaths())
        {
            TestUtil.WithScene(path, scene =>
            {
                foreach (var mb in TestUtil.InScene<UnityEngine.MonoBehaviour>(scene))
                {
                    if (mb == null)
                    {
                        continue;
                    }
                    foreach (var (name, value) in TestUtil.StringFields(mb))
                    {
                        bool sceneField = name.ToLower().Contains("scene") || (mb is World && name == "previous");
                        if (sceneField && !string.IsNullOrEmpty(value) && !build.Contains(value))
                        {
                            missing.Add(Path.GetFileNameWithoutExtension(path) + ": " + mb.name + "." + name + " = " + value);
                        }
                    }
                }
            });
        }
        Assert.IsEmpty(missing, TestUtil.Describe(missing));
    }
}
