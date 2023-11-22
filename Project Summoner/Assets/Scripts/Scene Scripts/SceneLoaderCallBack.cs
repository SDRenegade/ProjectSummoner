using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A monobehaviour script that is used in the loading scene to transition to the next scene.
public class SceneLoaderCallBack : MonoBehaviour
{
    private bool isFirstUpdtate = true;

    void Update()
    {
        if(isFirstUpdtate) {
            SceneLoader.LoaderCallBack();
            isFirstUpdtate = false;
        }
    }
}
