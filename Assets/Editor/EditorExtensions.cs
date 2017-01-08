using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class EditorExtensions
{
    static EditorExtensions()
    {
        EditorApplication.playmodeStateChanged += UnloadTertiaryScenes;
    }

    private static string mainScene = "app";
    private static string[] tertiaryScenes = new string[] { "game", "ui" };

    // Load and unload tertiary scenes on play so they aren't duplicated when loaded in game code
    private static void UnloadTertiaryScenes()
    {
        if (!EditorApplication.isPlaying && EditorSceneManager.GetActiveScene().name == mainScene)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                foreach (var sceneName in tertiaryScenes)
                {
                    var scene = EditorSceneManager.GetSceneByName(sceneName);
                    if (scene.isLoaded)
                    {
                        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByName(sceneName), false);
                    }
                }
            }
            else
            {
                foreach (var sceneName in tertiaryScenes)
                {
                    var scene = EditorSceneManager.GetSceneByName(sceneName);
                    if (scene.path != null && !scene.isLoaded)
                    {
                        var myScenePath = scene.path;
                        EditorSceneManager.OpenScene(Application.dataPath + "/" + myScenePath.Substring(myScenePath.IndexOf("Scenes")), OpenSceneMode.Additive);
                    }
                }
            }
        }
    }

    [MenuItem("Tools/Clear PlayerPrefs %#c")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Clear Console %#x")]
    private static void ClearConsole()
    {
        // This simply does "LogEntries.Clear()" the long way:
        var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        clearMethod.Invoke(null, null);
    }
}