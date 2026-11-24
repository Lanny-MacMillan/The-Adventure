using UnityEditor;
using UnityEngine;

// Editor-only test shortcuts (Franks Revenge > Dev menu). Not included in builds.
public static class DevShortcuts
{
    private static readonly string[] allProgress = { "kitchenDoor", "bedroomDoor", "woodshedDoor", "bathroomDoor", "treehouseDoor", "basementDoor", "cross", "bathroomCleared", "closetDemonDone", "closetDemonTalks", "woodshedSolved", "frankDefeated", "motorGear", "motorPlug", "motorBelt", "fountainFixed", "holyWater", "frankCured" };

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
