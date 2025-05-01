using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBarController : MonoBehaviour
{
    public Image loadingFillImage;
    private float currentFill = 0f; // 실제 보이는 fillAmount

    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        string nextSceneName = "GameScene_3";
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float targetProgress = Mathf.Clamp01(op.progress / 0.9f);

            // 부드럽게 이동 (Lerp)
            currentFill = Mathf.Lerp(currentFill, targetProgress, Time.deltaTime * 5f);
            loadingFillImage.fillAmount = currentFill;

            
            if (op.progress >= 0.9f && currentFill >= 0.995f)
            {
                yield return new WaitForSeconds(0.5f); // 살짝 멈춤
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
