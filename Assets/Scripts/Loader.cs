using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    static float spawnRate = 0.5f;
    static int level = 1;

    public static float getSpawnRate(){
        return spawnRate;
    }

    public enum Scene{
        Main,
        Loading,
        StartMenu

    }
    private static Action onLoaderCallback;

    public static void Load(Scene scene){
        // Set the loader callback action to load the target scene
        onLoaderCallback = () =>{
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene.ToString());
        };
        // Load loading scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(Scene.Loading.ToString());
    }

    // Execute loader callback action which will loade the target scene
    public static void LoaderCallback() {
        if (onLoaderCallback != null){
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }

    public static void Reload(Scene scene){
        // Reset difficulty
        Load(scene);
        spawnRate = 2.0f;
        
    }

    public static int GetLevel(){ return level; }

    public static void NextLevel(Scene scene){
        // Increase difficulty
        level++;
        Load(scene);
        spawnRate -= 0.1f;
    }
}
