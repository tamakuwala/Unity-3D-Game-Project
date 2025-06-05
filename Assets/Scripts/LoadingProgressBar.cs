using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingProgressBar : MonoBehaviour
{
    
    private Scene loadingScene;
    
    private void Awake(){
        //scrollBar = transform.GetComponent<ScrollBar>();
        this.GetComponentInChildren<Scrollbar>().size = 0;
    }

    private void Update(){
        float currentSize = this.GetComponentInChildren<Scrollbar>().size;

        Debug.Log(currentSize);
        /*if (Loader.GetLoadingProgress() < 0.9f && currentSize < 0.9f) {
            this.GetComponentInChildren<Scrollbar>().size = currentSize + Random.Range(0.001f, 0.005f);
        } */
        
    }


}
