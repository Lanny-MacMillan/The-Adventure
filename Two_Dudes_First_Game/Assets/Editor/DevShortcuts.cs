using UnityEditor;
using UnityEngine;

// Editor-only test shortcuts (Franks Revenge > Dev menu). Not included in builds.
public static class DevShortcuts
{
    private static readonly string[] allProgress = { "kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "treehouseDoor", "basementDoor", "cross", "bathroomCleared", "closetDemonDone", "closetDemonTalks", "woodshedSolved", "frankDefeated", "motorGear", "motorPlug", "motorBelt", "motorGearInstalled", "motorPlugInstalled", "motorBeltInstalled", "fountainFixed", "holyWater", "frankCured", "basementCleared", "stashCured", "blessedSaltNoticed" };

    [MenuItem("Franks Revenge/Dev/Start At Bathroom Fight (keys up to bathroom)")]
    private static void UpToBathroomFight()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor");
        Done("Kitchen, Bedroom, Woodshed and Bathroom keys set. Press Play in WorldMap and head upstairs to the bathroom.");
    }

    [MenuItem("Franks Revenge/Dev/Start After Bathroom Fight (ladder + attic)")]
    private static void AfterBathroomFight()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared");
        Done("Keys up to the bathroom set and the monster counted as beaten. Press Play in WorldMap; the upstairs bathroom door goes to the wrecked bathroom with the ladder.");
    }

    [MenuItem("Franks Revenge/Dev/Start At Frank Fight (keys up to treehouse)")]
    private static void UpToFrankFight()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "closetDemonDone", "woodshedSolved", "treehouseDoor");
        Done("Keys up to the Treehouse set. Press Play in WorldMap and climb the tree to the Treehouse.");
    }

    [MenuItem("Franks Revenge/Dev/Start At Fountain (all motor pieces found)")]
    private static void AtFountain()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "closetDemonDone", "woodshedSolved", "treehouseDoor", "motorGear", "motorPlug", "motorBelt");
        Done("Treehouse key and all 3 motor pieces set. Press Play in WorldMap and use the fountain next to the house.");
    }

    [MenuItem("Franks Revenge/Dev/Start At Frank Cure Fight (holy water)")]
    private static void FrankCureFight()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "closetDemonDone", "woodshedSolved", "treehouseDoor", "motorGear", "motorPlug", "motorBelt", "fountainFixed", "holyWater");
        Done("Fountain fixed and holy water in hand. Press Play in WorldMap and climb to the Treehouse; your salt cures Frank.");
    }

    [MenuItem("Franks Revenge/Dev/Start At Basement (kill the boss)")]
    private static void AtBasement()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "closetDemonDone", "woodshedSolved", "treehouseDoor", "frankDefeated", "basementDoor");
        Done("Every key up to the Basement set, Frank beaten. Press Play in WorldMap and take the Basement door on the 1st floor.");
    }

    [MenuItem("Franks Revenge/Dev/Start At Basement (cure the boss)")]
    private static void AtBasementCure()
    {
        Reset();
        Set("kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "bathroomCleared", "closetDemonDone", "woodshedSolved", "treehouseDoor", "motorGear", "motorPlug", "motorBelt", "fountainFixed", "holyWater", "frankDefeated", "frankCured", "basementDoor");
        Done("Frank cured and holy water in hand. Press Play in WorldMap and take the Basement door; your salt cures the boss.");
    }

    // ghosts, bats and evil spirits stay away while this is ticked (editor only)
    [MenuItem("Franks Revenge/Dev/Mobs Off", false, 100)]
    private static void ToggleMobs()
    {
        bool off = !EditorPrefs.GetBool(DevNoMobs.Key, false);
        EditorPrefs.SetBool(DevNoMobs.Key, off);
        Debug.Log(off ? "Mobs off: no ghosts, bats or evil spirits next time a scene loads." : "Mobs back on.");
    }

    [MenuItem("Franks Revenge/Dev/Mobs Off", true)]
    private static bool ToggleMobsCheck()
    {
        Menu.SetChecked("Franks Revenge/Dev/Mobs Off", EditorPrefs.GetBool(DevNoMobs.Key, false));
        return true;
    }

    [MenuItem("Franks Revenge/Dev/Reset Progress")]
    private static void ResetProgress()
    {
        Reset();
        Done("All keys and progress cleared.");
    }

    private static void Reset()
    {
        foreach (string key in allProgress)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    private static void Set(params string[] keys)
    {
        foreach (string key in keys)
        {
            PlayerPrefs.SetInt(key, 1);
        }
    }

    private static void Done(string message)
    {
        PlayerPrefs.Save();
        Debug.Log("[Dev] " + message);

        // non-blocking notice (a modal dialog would freeze the editor until clicked)
        EditorWindow window = EditorWindow.focusedWindow != null ? EditorWindow.focusedWindow : SceneView.lastActiveSceneView;
        if (window != null)
        {
            window.ShowNotification(new GUIContent(message), 4);
        }
    }
}
