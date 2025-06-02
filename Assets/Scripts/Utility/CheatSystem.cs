using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheatSystem : MonoBehaviour
{
    #region ���۷���
    public static CheatSystem instance { get; private set; }

    [Header("UI ���۷���")]
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TextMeshProUGUI outputText;

    [Header("����")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F12;

    private bool isGodMode = false;
    private bool isFlyMode = false;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private float flySpeed = 10f;

    private Dictionary<string, System.Action<string[]>> commands;
    private List<string> outputLines = new List<string>();
    private bool isActive = false;

    #endregion

    #region �ʱ�ȭ
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
        Log("ġƮ �ý��� �غ� �Ϸ�. F12 Ű�� ����");
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
        // ���� && ��ġ �̵�
        if (isGodMode && Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
        {
            GodModToMousePosition();
        }
        if (isFlyMode)
        {
            HandleFlyMovement();
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

    #region ġƮ ���
    private void ToggleGodMod(string[] args)
    {
        isGodMode = !isGodMode;

        if (playerController != null)
            playerController.SetInvincibility(isGodMode);

        Log($"���� ��� {(isGodMode ? "Ȱ��ȭ��" : "��Ȱ��ȭ��")}");
        ShowToast($"���� ��� {(isGodMode ? "ON" : "OFF")}", ToastMessage.MessageType.Success);
    }

    private void GodModToMousePosition()
    {
        if (playerTransform == null)
        {
            Log("�÷��̾� ������ �������� �ʾҽ��ϴ�.", true);
            return;
        }

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            playerTransform.position = hitInfo.point;
            Log("�÷��̾ ���콺 ��ġ�� �̵��߽��ϴ�.");
            ShowToast("�÷��̾� ��ġ �̵� �Ϸ�", ToastMessage.MessageType.Success);
        }
        else
        {
            Log("���콺 �Ʒ��� �浹 ������ ������ �����ϴ�.", true);
        }
    }

    private void ToggleFlyMod(string[] args)
    {
        isFlyMode = !isFlyMode;

        if (playerController != null)
        {
            playerController.rb.useGravity = false;
            playerController.rb.isKinematic = true;
        }

        Log($"���� ���� ��� {(isFlyMode ? "Ȱ��ȭ��" : "��Ȱ��ȭ��")}");
        ShowToast($"���� ���� ��� {(isFlyMode ? "ON" : "OFF")}", ToastMessage.MessageType.Success);
    }

    private void HandleFlyMovement()
    {
        if (playerTransform == null) return;

        float h = Input.GetAxis("Horizontal");      // A/D �Ǵ� ��/��
        float v = Input.GetAxis("Vertical");        // W/S �Ǵ� ��/��

        Vector3 move = new Vector3(h, 0, v);

        if (Input.GetKey(KeyCode.Space))    // ���
            move.y += 1;
        if (Input.GetKey(KeyCode.Q))        // �ϰ�
            move.y -= 1;

        // ī�޶� ���� �������� �����̱� (�÷��̾� ���� �ƴ�)
        Transform cam = Camera.main.transform;
        Vector3 direction = cam.TransformDirection(move);
        direction.y = move.y; // ���� �̵��� ���� ��������

        playerTransform.position += direction * flySpeed * Time.deltaTime;
    }

    private void Teleportation(string[] args)
    {
        Log("��ġ �̵�");
        ShowToast("��ġ �̵�", ToastMessage.MessageType.Success);

        // TODO: �����̵� ���� (�̸� ������ ���� ��ġ�� �̵� (���� : F1 �� ����1, F2 �� ����2, F3 �� ����3) || FŰ �� �� ���� ������ ���� �������� �Ѿ�� ��)
    }

    private void ClearConsole(string[] args)
    {
        outputLines.Clear();
        if (outputText != null)
            outputText.text = "";
        Log("�ܼ� ������");
    }

    private void ShowHelp(string[] args)
    {
        Log("=== ġƮ ��ɾ� ===");
        Log("god - �������");
        Log("Tel - ��ġ �̵�");
        Log("clear - �ܼ� ����");
    }

    #endregion

    #region ��ƿ
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
            ToastMessage.instance.ShowMessage($"[ġƮ] {message}", type);
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
            Log($"�˼� ���� ��ɾ� : {parts[0]}", true);
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
            else
            {
                // �г� ��Ȱ��ȭ�� �� ��� ġƮ ��� ����
                DisableAllCheats();
            }
        }
    }

    private void DisableAllCheats()
    {
        // ���� ��� ��Ȱ��ȭ
        if (isGodMode)
        {
            isGodMode = false;
            if (playerController != null)
                playerController.SetInvincibility(false);
            Log("���� ��� OFF");
            ShowToast("���� ��� OFF", ToastMessage.MessageType.Warning);
        }

        // ���� ��� ��Ȱ��ȭ
        if (isFlyMode)
        {
            isFlyMode = false;
            if (playerController != null)
            {
                playerController.rb.useGravity = true;
                playerController.rb.isKinematic = false;
            }
            Log("���� ��� OFF");
            ShowToast("���� ��� OFF", ToastMessage.MessageType.Warning);
        }
    }

    #endregion
}
