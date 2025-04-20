using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("LoadingScene"); 
    }

    public void OpenSettings()
    {
        Debug.Log("����â ����"); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
