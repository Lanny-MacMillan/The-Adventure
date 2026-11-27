#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;

// Editor-only playtest switch (Franks Revenge > Dev > Mobs Off): ghosts, bats and evil spirits don't show up. Bosses and traps stay.
public static class DevNoMobs
{
    public const string Key = "FranksRevenge.NoMobs";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Hook()
    {
        SceneManager.sceneLoaded -= OnLoaded;
        SceneManager.sceneLoaded += OnLoaded;
    }

    private static void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!UnityEditor.EditorPrefs.GetBool(Key, false))
        {
            return;
        }
        foreach (var g in Object.FindObjectsByType<EnemyGhost>(FindObjectsInactive.Exclude)) g.gameObject.SetActive(false);
        foreach (var b in Object.FindObjectsByType<AtticBat>(FindObjectsInactive.Exclude)) b.gameObject.SetActive(false);
        foreach (var s in Object.FindObjectsByType<EvilSpirit>(FindObjectsInactive.Exclude)) s.gameObject.SetActive(false);
    }
}
#endif
