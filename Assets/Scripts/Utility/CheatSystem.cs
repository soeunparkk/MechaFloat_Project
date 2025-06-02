using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatSystem : MonoBehaviour
{
    #region 레퍼런스
    public static CheatSystem instance { get; private set; }

    [Header("UI 레퍼런스")]
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TextMeshProUGUI outputText;

    [Header("세팅")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F12;

    private bool isGodMode = false;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController playerController;

    private Dictionary<string, System.Action<string[]>> commands;
    private List<string> outputLines = new List<string>();
    private bool isActive = false;

    #endregion

    #region 초기화
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeCommands();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (cheatPanel != null)
        {
            cheatPanel.SetActive(false);
        }
        Log("치트 시스템 준비 완료. F12 키로 열기");
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
        if (isActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ExecuteCommand();
        }
        // 
        if (isGodMode && Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            GodModToMousePosition();
        }
    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, System.Action<string[]>>
        {
            { "god", ToggleGodMod },
            { "fly", ToggleFlyMod },
            { "tel", Teleportation },
            { "clear", ClearConsole },
            { "help", ShowHelp }
        };
    }

    #endregion

    #region 치트 모드

    private void ToggleGodMod(string[] args)
    {
        isGodMode = !isGodMode;

        if (playerController != null)
            playerController.SetInvincibility(isGodMode);

        Log($"무적 모드 {(isGodMode ? "활성화됨" : "비활성화됨")}");
        ShowToast($"무적 모드 {(isGodMode ? "ON" : "OFF")}", ToastMessage.MessageType.Success);
    }

    private void GodModToMousePosition()
    {
        if (playerTransform == null)
        {
            Log("플레이어 참조가 설정되지 않았습니다.", true);
            return;
        }

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            playerTransform.position = hitInfo.point;
            Log("플레이어가 마우스 위치로 이동했습니다.");
            ShowToast("플레이어 위치 이동 완료", ToastMessage.MessageType.Success);
        }
        else
        {
            Log("마우스 아래에 충돌 가능한 지형이 없습니다.", true);
        }
    }

    private void ToggleFlyMod(string[] args)
    {
        Log("자유 모드 토글됨");
        ShowToast("자유 모드 ON/OFF", ToastMessage.MessageType.Success);

        // TODO : 자유 모드 구현
    }

    private void Teleportation(string[] args)
    {
        Log("위치 이동");
        ShowToast("위치 이동", ToastMessage.MessageType.Success);

        // TODO: 순간이동 구현 (미리 지정된 여러 위치에 이동 (예시 : F1 → 구간1, F2 → 구간2, F3 → 구간3) || F키 한 번 누를 때마다 다음 구간으로 넘어가는 식)
    }

    private void ClearConsole(string[] args)
    {
        outputLines.Clear();
        if (outputText != null)
            outputText.text = "";
        Log("콘솔 정리됨");
    }

    private void ShowHelp(string[] args)
    {
        Log("=== 치트 명령어 ===");
        Log("god - 무적모드");
        Log("Tel - 위치 이동");
        Log("clear - 콘솔 정리");
    }

    #endregion

    #region 유틸
    private void Log(string message, bool isError = false)
    {
        string coloredMessage = isError ? $"<color=red>{message}</color>" : message;
        outputLines.Add(coloredMessage);

        if (outputLines.Count > 10)
            outputLines.RemoveAt(0);

        if (outputText != null)
        {
            outputText.text = string.Join("\n", outputLines);
        }
    }

    private void ShowToast(string message, ToastMessage.MessageType type = ToastMessage.MessageType.info)
    {
        if (ToastMessage.instance != null)
        {
            ToastMessage.instance.ShowMessage($"[치트] {message}", type);
        }
    }

    private void ExecuteCommand()
    {
        if (commandInput == null || string.IsNullOrEmpty(commandInput.text))
            return;

        string command = commandInput.text.Trim().ToLower();
        string[] parts = command.Split(' ');

        Log($"> {command}");

        if (commands.ContainsKey(parts[0]))
        {
            commands[parts[0]](parts);
        }
        else
        {
            Log($"알수 없는 명령어 : {parts[0]}", true);
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void TogglePanel()
    {
        isActive = !isActive;

        if (cheatPanel != null)
        {
            cheatPanel.SetActive(isActive);

            if (isActive && commandInput != null)
            {
                commandInput.Select();
                commandInput.ActivateInputField();
            }
        }
    }

    #endregion
}
