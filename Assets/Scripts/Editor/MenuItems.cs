using UnityEngine;
using UnityEditor;

public class MenuItems
{
    [MenuItem("Tools/Clear PlayerPrefs %#c")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
    }
}