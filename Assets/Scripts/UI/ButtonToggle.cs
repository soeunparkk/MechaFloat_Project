using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonToggle : MonoBehaviour
{
    public Button toggleButton;  // ��ư (��� ����)
    public Image checkboxImage;  // üũ�ڽ��� ��Ÿ���� �̹���
    public Sprite checkedSprite;  // üũ�� ���� �̹���
    public Sprite uncheckedSprite;  // üũ���� ���� ���� �̹���
   

    private bool isChecked = false;

    void Start()
    {
        // ��ư Ŭ�� �� ��� ���� ����
        toggleButton.onClick.AddListener(OnButtonClick);
        UpdateImage();
    }

    void OnButtonClick()
    {
        // ��ư Ŭ�� �� üũ�ڽ� ���� ���� (üũ�� ���·� �ٲٰų� �ٽ� üũ ����)
        isChecked = !isChecked;
        UpdateImage();
    }

    void UpdateImage()
    {
        // �̹��� ����
        checkboxImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }
}
