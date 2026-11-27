using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Shared bits for the tests: the build scene list, the game's source files, and opening scenes without touching the one you have open.
public static class TestUtil
{
    public const string ScenesDir = "Assets/Level/Scenes/";

    public static string[] BuildScenePaths()
    {
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }

    public static HashSet<string> BuildSceneNames()
    {
        return new HashSet<string>(BuildScenePaths().Select(Path.GetFileNameWithoutExtension));
    }

    // every .cs file of the game itself (not plugins, not tests)
    public static IEnumerable<(string path, string text)> GameSource()
    {
        foreach (string f in Directory.GetFiles("Assets/Code", "*.cs", SearchOption.AllDirectories))
        {
            yield return (f.Replace('\\', '/'), File.ReadAllText(f));
        }
        foreach (string f in Directory.GetFiles("Assets/Editor", "*.cs", SearchOption.AllDirectories))
        {
            yield return (f.Replace('\\', '/'), File.ReadAllText(f));
        }
    }

    // first capture group of every match across the game's source
    public static IEnumerable<(string value, string file)> Matches(string pattern)
    {
        var re = new Regex(pattern);
        foreach (var (path, text) in GameSource())
        {
            foreach (Match m in re.Matches(text))
            {
                yield return (m.Groups[1].Value, path);
            }
        }
    }

    // opens a scene next to whatever you have open, runs the check, then closes it again
    public static void WithScene(string path, Action<Scene> check)
    {
        // already open in the editor: check that copy and leave it alone
        Scene open = SceneManager.GetSceneByPath(path);
        if (open.IsValid() && open.isLoaded)
        {
            check(open);
            return;
        }
        Scene scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        try
        {
            check(scene);
        }
        finally
        {
            EditorSceneManager.CloseScene(scene, true);
        }
    }

    public static IEnumerable<T> InScene<T>(Scene scene) where T : Component
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            foreach (T c in root.GetComponentsInChildren<T>(true))
            {
                yield return c;
            }
        }
    }

    // string fields serialized on a component, as (field name, value)
    public static IEnumerable<(string name, string value)> StringFields(UnityEngine.Object o)
    {
        var so = new SerializedObject(o);
        SerializedProperty it = so.GetIterator();
        while (it.Next(true))
        {
            if (it.propertyType == SerializedPropertyType.String)
            {
                yield return (it.name, it.stringValue);
            }
        }
    }

    public static T Field<T>(Type type, string name, object target = null)
    {
        FieldInfo f = type.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        return (T)f.GetValue(target);
    }

    public static string Describe(IEnumerable<string> items)
    {
        return string.Join("\n", items);
    }
}

// Tests share the editor's save, so anything that plays the game snapshots it first and puts it back after.
public static class SaveGuard
{
    private const string Slot = "FranksRevenge.TestSaveBackup";

    public static IEnumerable<string> Keys()
    {
        var keys = new HashSet<string>(TestUtil.Field<string[]>(typeof(GameManagerScript), "doorKeys"));
        foreach (string k in new[] { "musicVolume", "sfxVolume", "SelectedCharacter" })
        {
            keys.Add(k);
        }
        return keys;
    }

    // survives the domain reload when play mode starts
    public static void Backup()
    {
        if (SessionState.GetString(Slot, "") != "")
        {
            return;
        }
        var lines = new List<string>();
        foreach (string k in Keys())
        {
            if (PlayerPrefs.HasKey(k))
            {
                lines.Add(k + "=" + PlayerPrefs.GetInt(k) + "=" + PlayerPrefs.GetFloat(k).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                lines.Add(k + "=-");
            }
        }
        SessionState.SetString(Slot, string.Join("\n", lines));
    }

    public static void Restore()
    {
        string saved = SessionState.GetString(Slot, "");
        if (saved == "")
        {
            return;
        }
        foreach (string line in saved.Split('\n'))
        {
            string[] p = line.Split('=');
            if (p[1] == "-")
            {
                PlayerPrefs.DeleteKey(p[0]);
            }
            else if (p[0] == "musicVolume" || p[0] == "sfxVolume")
            {
                PlayerPrefs.SetFloat(p[0], float.Parse(p[2], System.Globalization.CultureInfo.InvariantCulture));
            }
            else
            {
                PlayerPrefs.SetInt(p[0], int.Parse(p[1]));
            }
        }
        PlayerPrefs.Save();
        SessionState.EraseString(Slot);
    }
}
