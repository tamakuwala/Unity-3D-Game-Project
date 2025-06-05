using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
   public void PlayGame(){
       Loader.Load(Loader.Scene.Main);
   }

   public void QuitGame(){
    Application.Quit();
   }

   public void Restart(){
       UnityEngine.SceneManagement.SceneManager.LoadScene(0);
   }
}
