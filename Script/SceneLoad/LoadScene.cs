using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScene
{
    public enum SceneName
    {
        GameMenuScene,
        Loading,
        GameScene
    }

    private static SceneName targetSceneName;

    public static void Load(SceneName sceneName)
    {
        Time.timeScale = 1;
        targetSceneName = sceneName;
    }

    public static void LoadLoadingScene()
    {
        SceneManager.LoadScene((int)LoadScene.SceneName.Loading);//游戏加载场景
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene((int)targetSceneName);
    }

    public static int GetLoadingSceneNumber()
    {
        return (int)SceneName.Loading;
    }
    public static int GetTargetSceneNumber()
    {
        return (int)targetSceneName;
    }
}
