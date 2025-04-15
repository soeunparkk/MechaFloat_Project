using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("TestScene"); 
    }

    public void OpenSettings()
    {
        Debug.Log("설정창 열기"); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
