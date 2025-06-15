using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("�����")]
    private bool isGodMode = false;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController playerController;

    [Header("�������")]
    private bool isFlyMode = false;
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] private float flyBoostMultiplier = 2.5f;

    [Header("�����̵� ���")]
    private bool isTeleportModeActive = false;
    private int currentPointIndex = 0;
    [SerializeField] private List<Transform> teleportPoints;

    [Header("��Ų ���")]
    [SerializeField] private List<SkinSO> allSkins;
    [SerializeField] private PlayerSkinApplier skinApplier;
    [SerializeField] private GameObject skinsPanel;
    [SerializeField] private Transform skinButtonParent;
    [SerializeField] private GameObject skinButtonPrefab;
    
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
        // ���� ���
        if (isFlyMode)
        {
            HandleFlyMovement();
        }
        // ���� �̵� ���
        if (isTeleportModeActive)
        {
            if (Input.GetKeyDown(KeyCode.F1)) TeleportToIndex(0);
            if (Input.GetKeyDown(KeyCode.F2)) TeleportToIndex(1);
            if (Input.GetKeyDown(KeyCode.F3)) TeleportToIndex(2);
            if (Input.GetKeyDown(KeyCode.F4)) TeleportToIndex(3);
            if (Input.GetKeyDown(KeyCode.F)) TeleportToNextPoint();
        }
    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, System.Action<string[]>>
        {
            { "god", ToggleGodMod },
            { "fly", ToggleFlyMod },
            { "tel", Teleportation },
            { "skin", ShowSkins },
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

        float h = Input.GetAxis("Horizontal"); // A/D �Ǵ� ��/��
        float v = Input.GetAxis("Vertical");   // W/S �Ǵ� ��/��

        Vector3 move = new Vector3(h, 0, v);

        if (Input.GetKey(KeyCode.Space))                // ���
            move.y += 1;
        if (Input.GetKey(KeyCode.LeftControl))          // �ϰ�
            move.y -= 1;

        // Shift ������ �ӵ� ����
        float speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? flyBoostMultiplier : 1f;
        float currentSpeed = flySpeed * speedMultiplier;

        // ī�޶� ���� �������� �����̱�
        Transform cam = Camera.main.transform;
        Vector3 direction = cam.TransformDirection(move);
        direction.y = move.y; // ���� �̵��� ���� ��������

        playerTransform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void Teleportation(string[] args)
    {
        if (args.Length == 1)
        {
            isTeleportModeActive = !isTeleportModeActive;
            Log($"�����̵� ��� {(isTeleportModeActive ? "Ȱ��ȭ" : "��Ȱ��ȭ")}");
            ShowToast($"�����̵� ��� {(isTeleportModeActive ? "ON" : "OFF")}", ToastMessage.MessageType.info);
        }
    }

    private void TeleportToIndex(int index)
    {
        if (teleportPoints == null || index >= teleportPoints.Count) return;

        playerTransform.position = teleportPoints[index].position;
        Log($"���� {index + 1}�� �����̵�");
        ShowToast($"���� {index + 1} �̵�", ToastMessage.MessageType.Success);
    }

    private void TeleportToNextPoint()
    {
        if (teleportPoints == null || teleportPoints.Count == 0) return;

        currentPointIndex = (currentPointIndex + 1) % teleportPoints.Count;
        playerTransform.position = teleportPoints[currentPointIndex].position;
        Log($"���� ����({currentPointIndex + 1})�� �����̵�");
        ShowToast($"���� ����({currentPointIndex + 1}) �̵�", ToastMessage.MessageType.Success);
    }

    private void ShowSkins(string[] args)
    {
        if (allSkins == null || allSkins.Count == 0)
        {
            Log("��� ������ ��Ų�� �����ϴ�.");
            return;
        }

        foreach (Transform child in skinButtonParent)
            Destroy(child.gameObject);

        skinsPanel.SetActive(true);
        Log("��Ų ���� UI ǥ�õ�.");

        for (int i = 0; i < allSkins.Count; i++)
        {
            int index = i;
            SkinSO skin = allSkins[i];

            GameObject btnObj = Instantiate(skinButtonPrefab, skinButtonParent);
            TMP_Text btnText = btnObj.GetComponentInChildren<TMP_Text>();
            btnText.text = skin.skinName;

            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                skinApplier.ApplySkin(skin);
                ShowToast($"��Ų '{skin.skinName}' ����", ToastMessage.MessageType.Success);
                Log($"��Ų '{skin.skinName}' ���� �Ϸ�");
            });
        }
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
        Log("tel - ��ġ �̵�");
        Log("fly - ���� �̵�");
        Log("skin - ��Ų Ȯ�� ");
        Log("clear - �ܼ� ����");
        Log("====================");
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

                skinsPanel.SetActive(false);
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
