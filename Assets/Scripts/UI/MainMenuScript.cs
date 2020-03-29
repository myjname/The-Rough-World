using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //Выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }
    
    //Смена сцены
    public void SceneLoad(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
