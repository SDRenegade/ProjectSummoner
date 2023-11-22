using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//An enum that holds an element for every scene (Update whenever a new scene is added to the game)
public enum SceneEnum
{
    GameScene,
    LoadingScene,
    BattleScene
}

//A static class used for loading between scenes.
public static class SceneLoader
{
    private static Action onLoaderCallBack;

    //Loads the LoadingScene scene and sets the onLoaderCallBack action to load the desired scene.
    public static void Load(SceneEnum scene)
    {
        SceneManager.LoadScene(SceneEnum.LoadingScene.ToString());

        onLoaderCallBack = () => {
            SceneManager.LoadScene(scene.ToString());
        };
    }

    //If the callback function is not null, executes the callback function loading the desired scene
    public static void LoaderCallBack()
    {
        if(onLoaderCallBack != null) {
            onLoaderCallBack();
            onLoaderCallBack = null;
        }
    }
}
