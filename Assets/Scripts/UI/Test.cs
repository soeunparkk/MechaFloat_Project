using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button testBackInGameButton;

    private void Start()
    {
        testBackInGameButton.onClick.AddListener(OnClickBackInGame);
    }

    private void OnClickBackInGame()
    {
        SceneManager.LoadScene("SkinSelectionScene");
    }
}
