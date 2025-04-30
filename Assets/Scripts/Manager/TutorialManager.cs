using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TutorialDatabaseSO tutorialDatabaseSO;
    
    public int currentId = -1;

    private TutorialSO currentTutorial => tutorialDatabaseSO.GetItemById(currentId);

    [Header("Typing Effect Settings")]
    public float typingSpeed = 0.05f;  // �� ���ڸ��� ������ �ӵ�
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool justStartedTutorial = false;

    [Header("TriggerEvent")]
    private string lastTriggerEvent = "";

    [Header("UI")]
    [SerializeField] private GameObject TutorialPopupUI;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private GameObject spaceIcon;
    [SerializeField] private GameObject eIcon;

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
            return; // �� ������ ��
        }

        // Ÿ������ ���� ������ �ʾҴٸ� ���� üũ���� ����
        if (isTyping)
            return;

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
            TutorialPopupUI.SetActive(true);
            UpdateTutorialText();          
            UpdateTutorialIcons();         
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
            UpdateTutorialIcons();
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
            case CheckConditionType.AnyKey:
                return Input.anyKeyDown;
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

    private bool EnumTryParseKeyCode(string keyName, out KeyCode keyCode)
    {
        return System.Enum.TryParse(keyName, true, out keyCode);
    }

    private IEnumerator TypeText(string text)
    {
        tutorialText.text = "";
        isTyping = true;

        string[] lines = text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string currentLine = lines[i];
            string displayed = "";

            for (int j = 0; j < currentLine.Length; j++)
            {
                // ���� �� �߰� Ÿ����
                displayed += currentLine[j];
                tutorialText.text = displayed;

                float timer = 0f;
                while (timer < typingSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        // ���� ���� ��� �ϼ�
                        tutorialText.text = currentLine;
                        yield return null;
                        j = currentLine.Length; // Ż�� ����
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // ���� �ٷ� �Ѿ�� �� ��� or Enter ���
            float delay = 0.7f;
            float waitTimer = 0f;
            while (waitTimer < delay)
            {
                if (Input.GetKeyDown(KeyCode.Return)) break;
                waitTimer += Time.deltaTime;
                yield return null;
            }
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
        TutorialPopupUI.SetActive(false);
    }

    public void TriggerEvent(string eventName)
    {
        Debug.Log($"Ʈ���� �̺�Ʈ �߻�: {eventName}");
        lastTriggerEvent = eventName;

        if (eventName == "WindZoneArea" && currentTutorial?.id == 4)
        {
            CompleteStep();
        }

        if (eventName == "MovingEnd" && currentTutorial?.id == 5)
        {
            CompleteStep();
        }

        if (eventName == "HammerEnd" && currentTutorial?.id == 6)
        {
            CompleteStep();
        }

        if (eventName == "VanishEnd" && currentTutorial?.id == 8)
        {
            CompleteStep();
        }
    }

    private void UpdateTutorialIcons()
    {
        if (currentTutorial == null) return;

        spaceIcon.SetActive(false);
        eIcon.SetActive(false);

        switch (currentTutorial.parameter)
        {
            case "Space":
                spaceIcon.SetActive(true);
                break;

            case "E":
                eIcon.SetActive(true);
                break;
        }
    }
}
