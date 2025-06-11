using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private GameObject clearUI;                            // Ŭ���� ���� UI ������Ʈ
    [SerializeField] private float delayBeforeLoad = 2f;
    [SerializeField] private string mainSceneName = "StartScene_1";         // ���� �� �̸�

    private bool _isGoalReached = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isGoalReached) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        _isGoalReached = true;
        ShowClearUI();
        StartCoroutine(LoadMainSceneAfterDelay());
    }

    private void ShowClearUI()
    {
        if (clearUI != null)
            clearUI.SetActive(true);
    }

    private System.Collections.IEnumerator LoadMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(mainSceneName);
    }
}
