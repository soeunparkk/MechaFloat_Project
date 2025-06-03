using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float downAngle = 90f;
    public float upAngle = 0f;
    public float downSpeed = 300f;
    public float upSpeed = 100f;
    public float waitTime = 1f;

    private bool isFalling = true;
    private float waitTimer = 0f;
    private float effectTimer = 0f;

    [Header("해머 잔상 이펙트 세팅")]
    [SerializeField] private Transform effectPos; // 먼지 효과 위치(빈 오브젝트 해머 자식 등)
    public string effectName = "HammerTrail";      // EffectManager 등록한 이펙트 이름
    public float effectInterval = 0.08f;           // 이펙트 나오는 시간 간격(초)

    private float lastAngle;

    void Start()
    {
        lastAngle = transform.localEulerAngles.z;
    }

    void Update()
    {
        float targetAngle = isFalling ? downAngle : upAngle;
        float speed = isFalling ? downSpeed : upSpeed;

        float currentZ = transform.localEulerAngles.z;
        float newZ = Mathf.MoveTowardsAngle(currentZ, targetAngle, speed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, newZ);

        // 1. 해머가 움직이고 있는지 체크(각도 변화량)
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(currentZ, newZ));
        bool isMoving = angleDelta > 0.1f; // 0.1 이상 변하면 움직임 간주

        if (isMoving)
        {
            effectTimer += Time.deltaTime;
            if (effectTimer >= effectInterval)
            {
                // 2. 이펙트 생성(주기마다)
                Vector3 spawnPos = (effectPos != null) ? effectPos.position : transform.position;
                EffectManager.instance.PlayEffect(effectName, spawnPos, Quaternion.identity);

                effectTimer = 0f;
            }
        }
        else
        {
            effectTimer = effectInterval; // 멈추면 바로 쿨타임 초기화
        }

        // 원래 해머 움직임 로직
        if (Mathf.Abs(Mathf.DeltaAngle(newZ, targetAngle)) < 0.1f)
        {
            if (isFalling)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    isFalling = false;
                    waitTimer = 0f;
                }
            }
            else
            {
                isFalling = true;
            }
        }

        lastAngle = newZ;
    }
}
