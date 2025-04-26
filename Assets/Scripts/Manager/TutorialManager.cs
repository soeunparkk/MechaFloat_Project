using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TutorialDatabaseSO tutorialDatabaseSO;
    public TextMeshProUGUI tutorialText;

    private int currentIndex = 0;
    private TutorialSO currentTutorial =>
        (currentIndex < tutorialDatabaseSO.tutorials.Count) ? tutorialDatabaseSO.tutorials[currentIndex] : null;

    [Header("Typing Effect Settings")]
    public float typingSpeed = 0.05f;  // 한 글자마다 나오는 속도
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Start()
    {
        UpdateTutorialText();
    }

    private void Update()
    {
        if (currentTutorial == null)
            return;

        if (CheckCurrentStep())
        {
            CompleteStep();
        }
    }

    private void CompleteStep()
    {
        Debug.Log($"튜토리얼 완료: {currentTutorial.id} - {currentTutorial.description}");

        if (currentTutorial != null)
        {
            currentTutorial.isClear = true;
        }

        currentIndex++;

        if (currentTutorial != null)
        {
            UpdateTutorialText();
        }
        else
        {
            Debug.Log("튜토리얼 전부 완료!");
            StartTypingEffect("튜토리얼 완료!");
        }
    }


    private void UpdateTutorialText()
    {
        if (currentTutorial != null)
        {
            StartTypingEffect(currentTutorial.description);
        }
    }

    private bool CheckCurrentStep()
    {
        switch (currentTutorial.checkConditionType)
        {
            case CheckConditionType.InputKey:
                return CheckInputKey(currentTutorial.parameter);

            case CheckConditionType.TriggerEvent:
                return CheckTriggerEvent(currentTutorial.parameter);

            case CheckConditionType.BalloonState:
                return CheckBalloonState(currentTutorial.parameter);

            default:
                return false;
        }
    }

    private bool CheckInputKey(string keyName)
    {
        if (keyName.Contains(","))
        {
            var keys = keyName.Split(',');

            foreach (var key in keys)
            {
                var trimmed = key.Trim();
                if (trimmed == "Horizontal" && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
                    return true;
                if (trimmed == "Vertical" && Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
                    return true;
                if (EnumTryParseKeyCode(trimmed, out KeyCode parsedKey) && Input.GetKeyDown(parsedKey))
                    return true;
            }
            return false;
        }
        else
        {
            if (keyName == "Horizontal")
                return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
            if (keyName == "Vertical")
                return Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

            if (EnumTryParseKeyCode(keyName, out KeyCode parsedKey))
                return Input.GetKeyDown(parsedKey);
        }
        return false;
    }

    private bool CheckTriggerEvent(string eventName)
    {
        // 트리거 이벤트 발동 체크
        return false;
    }

    private bool CheckBalloonState(string stateName)
    {
        // 풍선 장착 상태 체크
        return false;
    }

    private bool EnumTryParseKeyCode(string keyName, out KeyCode keyCode)
    {
        return System.Enum.TryParse(keyName, true, out keyCode);
    }

    private IEnumerator TypeText(string text)
    {
        tutorialText.text = "";
        foreach (char c in text)
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void StopTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void StartTypingEffect(string text)
    {
        isTyping = true;
        StopTypingEffect();
        typingCoroutine = StartCoroutine(TypeText(text));
    }
}
