using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRunDust : MonoBehaviour
{
    public ParticleSystem runDustEffect;

    void Update()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        if (isRunning)
        {
            if (!runDustEffect.isPlaying)
                runDustEffect.Play();
        }
        else
        {
            if (runDustEffect.isPlaying)
                runDustEffect.Stop();
        }
    }
}