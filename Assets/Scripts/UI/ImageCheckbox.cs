using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCheckbox : MonoBehaviour
{
    public Button toggleButton;  // 버튼
    public Image checkboxImage;  // 체크박스를 나타내는 이미지
    public Sprite checkedSprite;  // 체크된 상태 이미지
    public Sprite uncheckedSprite;  // 체크되지 않은 상태 이미지

    private bool isChecked = false;

    void Start()
    {
        // 버튼 클릭 이벤트에 메소드 연결
        toggleButton.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // 클릭 시 이미지 변경 (체크된 상태로 바꿔주거나 다시 체크 해제)
        isChecked = !isChecked;
        checkboxImage.sprite = isChecked ? checkedSprite : uncheckedSprite;
    }
}
