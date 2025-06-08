using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResetter : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialFieldOfView; // Perspective 카메라의 경우
    private float initialOrthographicSize; // Orthographic 카메라의 경우

    public Camera mainCamera; // Inspector에서 메인 카메라 할당 또는 자동으로 찾기

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 씬에 "MainCamera" 태그가 붙은 카메라를 찾음
            if (mainCamera == null)
            {
                mainCamera = GetComponent<Camera>(); // 이 스크립트가 카메라에 직접 붙어있다면
            }
        }

        if (mainCamera != null)
        {
            // 게임 시작 시 (또는 이 스크립트가 활성화될 때) 현재 카메라의 Transform 값을 저장
            initialPosition = mainCamera.transform.position;
            initialRotation = mainCamera.transform.rotation;
            if (mainCamera.orthographic)
            {
                initialOrthographicSize = mainCamera.orthographicSize;
            }
            else
            {
                initialFieldOfView = mainCamera.fieldOfView;
            }
            Debug.Log("카메라 초기 위치 저장됨: " + initialPosition);
        }
        else
        {
            Debug.LogError("메인 카메라를 찾거나 할당할 수 없습니다!");
        }
    }

    // 이 함수를 호출하여 카메라를 저장된 초기값으로 되돌림
    public void ResetCameraToInitialValues()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = initialPosition;
            mainCamera.transform.rotation = initialRotation;
            if (mainCamera.orthographic)
            {
                mainCamera.orthographicSize = initialOrthographicSize;
            }
            else
            {
                mainCamera.fieldOfView = initialFieldOfView;
            }
            Debug.Log("카메라 위치가 초기값으로 리셋됨.");
        }
    }

    // 예시: 특정 키를 누르면 카메라 리셋 (테스트용)
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ResetCameraToInitialValues();
    //     }
    // }
}