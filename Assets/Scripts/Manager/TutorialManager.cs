using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TutorialDatabaseSO tutorialDatabaseSO;
    public TextMeshProUGUI tutorialText;

    public int currentId = -1;

    private TutorialSO currentTutorial => tutorialDatabaseSO.GetItemById(currentId);

    [Header("Typing Effect Settings")]
    public float typingSpeed = 0.05f;  // �� ���ڸ��� ������ �ӵ�
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool justStartedTutorial = false;

    [Header("TriggerEvent")]
    private string lastTriggerEvent = "";

    void Start()
    {
        tutorialText.text = "";
    }

    void Update()
    {
        if (currentTutorial == null)
            return;

        if (justStartedTutorial)
        {
            justStartedTutorial = false;
            return; // �� ������ �����ٰ� üũ ����
        }

        if (CheckCurrentStep())
        {
            CompleteStep();
        }
    }

    public void GoToTutorialStep(int id)
    {
        var tutorial = tutorialDatabaseSO.GetItemById(id);

        if (tutorial != null)
        {
            currentId = id;
            UpdateTutorialText();
            justStartedTutorial = true;
        }
        else
        {
            Debug.LogWarning($"Ʃ�丮�� ID�� ã�� �� ����: {id}");
        }
    }

    private void CompleteStep()
    {
        Debug.Log($"Ʃ�丮�� �Ϸ�: {currentTutorial.id} - {currentTutorial.description}");

        if (currentTutorial != null)
        {
            currentTutorial.isClear = true;
        }

        currentId = -1; 

        SuccessEffect();
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
                if (EnumTryParseKeyCode(trimmed, out KeyCode parsedKey) && Input.GetKeyDown(parsedKey))
                    return true;
            }
            return false;
        }
        else
        {
            if (EnumTryParseKeyCode(keyName, out KeyCode parsedKey))
                return Input.GetKeyDown(parsedKey);
        }
        return false;
    }

    private bool CheckTriggerEvent(string eventName)
    {
        return lastTriggerEvent == eventName;
    }

    private bool CheckBalloonState(string stateName)
    {
        // ǳ�� ���� ���� üũ
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

    private void SuccessEffect()
    {
        StopTypingEffect();
        tutorialText.text = "Ʃ�丮�� ����!"; 
        StartCoroutine(ClearTextAfterDelay(2f)); 
    }

    private IEnumerator ClearTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        tutorialText.text = "";
    }

    public void TriggerEvent(string eventName)
    {
        Debug.Log($"Ʈ���� �̺�Ʈ �߻�: {eventName}");
        lastTriggerEvent = eventName;

        if (eventName == "VanishEnd" && currentTutorial?.id == 3)
        {
            CompleteStep();
        }
    }
}
