using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonToggle : MonoBehaviour
{
    public Button toggleButton;  // 버튼 (토글 역할)
    public Image checkboxImage;  // 체크박스를 나타내는 이미지
    public Sprite checkedSprite;  // 체크된 상태 이미지
    public Sprite uncheckedSprite;  // 체크되지 않은 상태 이미지
   

    private bool isChecked = false;

    void Start()
    {
        // 버튼 클릭 시 토글 상태 변경
        toggleButton.onClick.AddListener(OnButtonClick);
        UpdateImage();
    }

    void OnButtonClick()
    {
        // 버튼 클릭 시 체크박스 상태 변경 (체크된 상태로 바꾸거나 다시 체크 해제)
        isChecked = !isChecked;
        UpdateImage();
    }

    void UpdateImage()
    {
        // 이미지 변경
        checkboxImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }
}
