using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

// Shared bits for tests that play the real game. Your save and the Mobs Off setting are put back after every test, pass or fail.
// Each test does: Prepare(...); yield return new EnterPlayMode(); ...  (play mode has to be entered from the test itself)
public abstract class PlayTestBase
{
    private const string NoMobs = "FranksRevenge.NoMobs";
    private const string NoMobsBackup = "FranksRevenge.Test.NoMobsWas";

    protected static PlayerHealth Player()
    {
        return Object.FindObjectsByType<PlayerHealth>(FindObjectsInactive.Exclude).FirstOrDefault(p => p.gameObject.scene.name == "DontDestroyOnLoad");
    }

    protected static IEnumerator Load(string scene, string from)
    {
        // keep playing when the editor isn't the front window, or game time stops and the test waits forever
        Application.runInBackground = true;
        SceneSwitch.prevScene = from;
        SceneManager.LoadScene(scene);
        yield return null;
        yield return null;
    }

    // game time, not test-runner ticks (the runner can tick several times per game frame)
    protected static IEnumerator Seconds(float s)
    {
        float end = Time.time + s;
        while (Time.time < end)
        {
            yield return null;
        }
    }

    protected static IEnumerator WaitForScene(string scene, float giveUp)
    {
        float end = Time.time + giveUp;
        while (SceneManager.GetActiveScene().name != scene && Time.time < end)
        {
            yield return null;
        }
    }

    protected static string Scene()
    {
        return SceneManager.GetActiveScene().name;
    }

    // before play mode: snapshot the save and pick whether mobs show up
    protected static void Prepare(bool mobs)
    {
        SaveGuard.Backup();
        if (!SessionState.GetBool(NoMobsBackup + ".set", false))
        {
            SessionState.SetBool(NoMobsBackup, EditorPrefs.GetBool(NoMobs, false));
            SessionState.SetBool(NoMobsBackup + ".set", true);
        }
        EditorPrefs.SetBool(NoMobs, !mobs);
    }

    protected static void SetFlags(params string[] keys)
    {
        foreach (string k in keys)
        {
            PlayerPrefs.SetInt(k, 1);
        }
        PlayerPrefs.Save();
    }

    protected static IEnumerator StartOnTheWorldMap()
    {
        AudioListener.volume = 0f;
        yield return Load("WorldMap", "WorldMap");
        yield return Seconds(0.5f);
    }

    // clicks through whatever dialogue is up, taking the first choice when one comes up
    protected static IEnumerator FinishDialogue(float giveUp = 20f)
    {
        DialogueManager dm = DialogueManager.GetInstance();
        if (dm == null)
        {
            yield break;
        }
        MethodInfo cont = typeof(DialogueManager).GetMethod("ContinueStory", BindingFlags.NonPublic | BindingFlags.Instance);
        FieldInfo story = typeof(DialogueManager).GetField("currentStory", BindingFlags.NonPublic | BindingFlags.Instance);
        float end = Time.time + giveUp;
        while (dm.dialogueIsPlaying && Time.time < end)
        {
            yield return Seconds(0.15f);
            if (!dm.dialogueIsPlaying)
            {
                break;
            }
            var s = (Ink.Runtime.Story)story.GetValue(dm);
            if (s.currentChoices.Count > 0)
            {
                dm.MakeChoice(0);
            }
            else
            {
                cont.Invoke(dm, null);
            }
        }
    }

    protected static T Get<T>(object target, string field)
    {
        return (T)target.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(target);
    }

    protected static void Set(object target, string field, object value)
    {
        target.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).SetValue(target, value);
    }

    // runs after every test, passed or failed
    [UnityTearDown]
    public IEnumerator PutEverythingBack()
    {
        if (Application.isPlaying)
        {
            AudioListener.volume = 1f;
            Time.timeScale = 1f;
            yield return new ExitPlayMode();
        }
        if (SessionState.GetBool(NoMobsBackup + ".set", false))
        {
            EditorPrefs.SetBool(NoMobs, SessionState.GetBool(NoMobsBackup, false));
            SessionState.EraseBool(NoMobsBackup);
            SessionState.EraseBool(NoMobsBackup + ".set");
        }
        SaveGuard.Restore();
    }
}
