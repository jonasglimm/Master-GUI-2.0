using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour //attach this script to each button onClick()
{
    public void LoadScene(int sceneNumber) //sceneNumber set in File -> Build Settings
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
