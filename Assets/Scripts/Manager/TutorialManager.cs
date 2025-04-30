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
    public float typingSpeed = 0.05f;  // 한 글자마다 나오는 속도
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
            return; // 한 프레임 쉼
        }

        // 타이핑이 아직 끝나지 않았다면 조건 체크하지 않음
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
            Debug.LogWarning($"튜토리얼 ID를 찾을 수 없음: {id}");
        }
    }

    private void CompleteStep()
    {
        Debug.Log($"튜토리얼 완료: {currentTutorial.id} - {currentTutorial.description}");

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
                // 현재 줄 중간 타이핑
                displayed += currentLine[j];
                tutorialText.text = displayed;

                float timer = 0f;
                while (timer < typingSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        // 현재 줄을 즉시 완성
                        tutorialText.text = currentLine;
                        yield return null;
                        j = currentLine.Length; // 탈출 조건
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            // 다음 줄로 넘어가기 전 대기 or Enter 대기
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
        tutorialText.text = "튜토리얼 성공!"; 
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
        Debug.Log($"트리거 이벤트 발생: {eventName}");
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
