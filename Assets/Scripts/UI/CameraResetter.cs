using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResetter : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialFieldOfView; // Perspective ī�޶��� ���
    private float initialOrthographicSize; // Orthographic ī�޶��� ���

    public Camera mainCamera; // Inspector���� ���� ī�޶� �Ҵ� �Ǵ� �ڵ����� ã��

    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ���� "MainCamera" �±װ� ���� ī�޶� ã��
            if (mainCamera == null)
            {
                mainCamera = GetComponent<Camera>(); // �� ��ũ��Ʈ�� ī�޶� ���� �پ��ִٸ�
            }
        }

        if (mainCamera != null)
        {
            // ���� ���� �� (�Ǵ� �� ��ũ��Ʈ�� Ȱ��ȭ�� ��) ���� ī�޶��� Transform ���� ����
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
            Debug.Log("ī�޶� �ʱ� ��ġ �����: " + initialPosition);
        }
        else
        {
            Debug.LogError("���� ī�޶� ã�ų� �Ҵ��� �� �����ϴ�!");
        }
    }

    // �� �Լ��� ȣ���Ͽ� ī�޶� ����� �ʱⰪ���� �ǵ���
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
            Debug.Log("ī�޶� ��ġ�� �ʱⰪ���� ���µ�.");
        }
    }

    // ����: Ư�� Ű�� ������ ī�޶� ���� (�׽�Ʈ��)
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ResetCameraToInitialValues();
    //     }
    // }
}