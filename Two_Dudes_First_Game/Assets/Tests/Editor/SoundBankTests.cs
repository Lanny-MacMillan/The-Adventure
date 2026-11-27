using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

// Sound.Play / Sound.Music look sounds up by name, so a typo just means silence. These catch that.
public class SoundBankTests
{
    private SoundBank bank;

    [OneTimeSetUp]
    public void Load()
    {
        bank = Resources.Load<SoundBank>("SoundBank");
        Assert.IsNotNull(bank, "Resources/SoundBank is missing");
    }

    [Test]
    public void EverySoundEffectPlayedInCodeExists()
    {
        var keys = new HashSet<string>(bank.effects.Select(e => e.key));
        var missing = TestUtil.Matches(@"Sound\.Play\(\s*""([A-Za-z0-9_]+)""")
            .Where(m => !keys.Contains(m.value)).Select(m => m.value + " (" + m.file + ")").Distinct();
        Assert.IsEmpty(missing, TestUtil.Describe(missing));
    }

    [Test]
    public void EveryMusicTrackPlayedInCodeExists()
    {
        var keys = new HashSet<string>(bank.music.Select(t => t.key));
        var missing = TestUtil.Matches(@"Sound\.Music\(\s*""([A-Za-z0-9_]+)""")
            .Where(m => !keys.Contains(m.value)).Select(m => m.value + " (" + m.file + ")").Distinct();
        Assert.IsEmpty(missing, TestUtil.Describe(missing));
    }

    [Test]
    public void EveryEffectHasASound()
    {
        var empty = bank.effects.Where(e => e.clips == null || e.clips.Length == 0 || e.clips.Any(c => c == null)).Select(e => e.key);
        Assert.IsEmpty(empty, TestUtil.Describe(empty));
    }

    [Test]
    public void EveryTrackHasMusic()
    {
        var empty = bank.music.Where(t => t.clip == null).Select(t => t.key);
        Assert.IsEmpty(empty, TestUtil.Describe(empty));
    }

    [Test]
    public void NoNameIsUsedTwice()
    {
        var dupes = bank.effects.GroupBy(e => e.key).Where(g => g.Count() > 1).Select(g => "effect " + g.Key)
            .Concat(bank.music.GroupBy(t => t.key).Where(g => g.Count() > 1).Select(g => "music " + g.Key));
        Assert.IsEmpty(dupes, TestUtil.Describe(dupes));
    }

    // a blank track means silence on purpose
    [Test]
    public void SceneMusicPointsAtRealScenesAndTracks()
    {
        var tracks = new HashSet<string>(bank.music.Select(t => t.key));
        var build = TestUtil.BuildSceneNames();
        var bad = bank.scenes.Where(s => !build.Contains(s.scene)).Select(s => "scene not in build: " + s.scene)
            .Concat(bank.scenes.Where(s => !string.IsNullOrEmpty(s.track) && !tracks.Contains(s.track)).Select(s => s.scene + " -> missing track " + s.track));
        Assert.IsEmpty(bad, TestUtil.Describe(bad));
    }

    // door open sounds and the like are picked in the Inspector
    [Test]
    public void SoundsPickedInScenesExist()
    {
        var keys = new HashSet<string>(bank.effects.Select(e => e.key));
        var missing = new List<string>();
        foreach (string path in TestUtil.BuildScenePaths())
        {
            TestUtil.WithScene(path, scene =>
            {
                foreach (var mb in TestUtil.InScene<MonoBehaviour>(scene))
                {
                    if (mb == null)
                    {
                        continue;
                    }
                    foreach (var (name, value) in TestUtil.StringFields(mb))
                    {
                        if (name.ToLower().Contains("sound") && !string.IsNullOrEmpty(value) && !keys.Contains(value))
                        {
                            missing.Add(System.IO.Path.GetFileNameWithoutExtension(path) + ": " + mb.name + "." + name + " = " + value);
                        }
                    }
                }
            });
        }
        Assert.IsEmpty(missing, TestUtil.Describe(missing));
    }
}
