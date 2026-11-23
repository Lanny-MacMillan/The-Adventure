using UnityEngine;

public static class CharacterSelection
{
    public const int DevGuy = 0;
    public const int Dude1 = 1;

    private const string PrefKey = "SelectedCharacter";

    public static int Selected
    {
        get { return PlayerPrefs.GetInt(PrefKey, DevGuy); }
        set
        {
            PlayerPrefs.SetInt(PrefKey, value);
            PlayerPrefs.Save();
        }
    }
}
