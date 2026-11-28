using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// Restart wipes progress from one list in GameManagerScript. Any flag the game saves that isn't on it would leak into the next run.
public class ProgressKeysTests
{
    // saved on purpose across runs
    private static readonly string[] Settings = { "musicVolume", "sfxVolume", "SelectedCharacter" };

    private static HashSet<string> ResetList()
    {
        return new HashSet<string>(TestUtil.Field<string[]>(typeof(GameManagerScript), "doorKeys"));
    }

    private static IEnumerable<(string key, string from)> SavedKeys()
    {
        foreach (var m in TestUtil.Matches(@"PlayerPrefs\.Set(?:Int|Float|String)\(\s*""([A-Za-z0-9_]+)"""))
        {
            yield return (m.value, m.file);
        }
        foreach (Item.ItemType t in (Item.ItemType[])Enum.GetValues(typeof(Item.ItemType)))
        {
            string key = new Item { itemType = t }.PrefKey();
            yield return (key, "item " + t);
            if (key.StartsWith("motor"))
            {
                yield return (key + "Installed", "item " + t);
            }
        }
        foreach (string path in TestUtil.BuildScenePaths())
        {
            var found = new List<(string, string)>();
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
                        bool flagField = name.EndsWith("Flag") || name == "flag" || name == "visitsKey";
                        if (flagField && !string.IsNullOrEmpty(value))
                        {
                            found.Add((value, System.IO.Path.GetFileNameWithoutExtension(path) + ": " + mb.name + "." + name));
                        }
                    }
                }
            });
            foreach (var f in found)
            {
                yield return f;
            }
        }
    }

    [Test]
    public void RestartWipesEveryProgressFlag()
    {
        var reset = ResetList();
        var leaks = SavedKeys().Where(k => !Settings.Contains(k.key) && !reset.Contains(k.key))
            .Select(k => k.key + " (" + k.from + ")").Distinct();
        Assert.IsEmpty(leaks, "Not reset by Restart:\n" + TestUtil.Describe(leaks));
    }

    [Test]
    public void DevResetClearsTheSameFlagsAsRestart()
    {
        var dev = new HashSet<string>(TestUtil.Field<string[]>(typeof(DevShortcuts), "allProgress"));
        var reset = ResetList();
        var onlyRestart = reset.Except(dev).Select(k => "missing from Dev > Reset Progress: " + k);
        var onlyDev = dev.Except(reset).Select(k => "missing from Restart: " + k);
        var diff = onlyRestart.Concat(onlyDev);
        Assert.IsEmpty(diff, TestUtil.Describe(diff));
    }

    [Test]
    public void SettingsSurviveARestart()
    {
        var reset = ResetList();
        var wiped = Settings.Where(reset.Contains);
        Assert.IsEmpty(wiped, "Settings should not be wiped:\n" + TestUtil.Describe(wiped));
    }
}
