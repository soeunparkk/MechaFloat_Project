using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingBarController : MonoBehaviour
{


    public static string Test_Scene1;      
    public Slider loadingBar;
   // public Image fillImage;

    // 씬 전환을 시작하는 정적 함수 (외부에서 호출)
    public static void LoadScene(string sceneName)
    {
        Test_Scene1 = sceneName;
        SceneManager.LoadScene("Test_Scene1"); // 로딩 씬으로 전환
    }

    // 로딩씬에서 호출될 코루틴
    void Start()
    {
        // 로딩씬에서만 실행되도록 체크
        if (SceneManager.GetActiveScene().name == "Test_Scene1")
        {
            StartCoroutine(LoadAsyncScene());
        }
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(Test_Scene1);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            if (loadingBar != null)
                loadingBar.value = progress;

            if (op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
