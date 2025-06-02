using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRunDust : MonoBehaviour
{
    [SerializeField] private GameObject dustPos;

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        PlayerJump playerJump = GetComponent<PlayerJump>();

        if (isRunning && playerJump.isGrounded())
        {
            EffectManager.instance.PlayEffect("RunDust", dustPos.transform.position, Quaternion.identity);
        }
    }
}