using System;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void ChangeSceneTo(Scenes new_scene)
    {
        string name_of_scene = new_scene.ToString();

        SceneManager.LoadScene(name_of_scene);
    }

    internal static void ResetCurrentScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
