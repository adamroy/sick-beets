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

    [MenuItem("Tools/Clear PlayerPrefs %#c")]
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private static string mainScene = "app";
    private static string[] tertiaryScenes = new string[] { "game", "ui" };

    private static void UnloadTertiaryScenes()
    {
        if(!EditorApplication.isPlaying && EditorSceneManager.GetActiveScene().name == mainScene)
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
}