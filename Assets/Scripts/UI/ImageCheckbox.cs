using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCheckbox : MonoBehaviour
{
    public Button toggleButton;  // ��ư
    public Image checkboxImage;  // üũ�ڽ��� ��Ÿ���� �̹���
    public Sprite checkedSprite;  // üũ�� ���� �̹���
    public Sprite uncheckedSprite;  // üũ���� ���� ���� �̹���

    private bool isChecked = false;

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� �޼ҵ� ����
        toggleButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Ŭ�� �� �̹��� ���� (üũ�� ���·� �ٲ��ְų� �ٽ� üũ ����)
        isChecked = !isChecked;
        checkboxImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }
}
