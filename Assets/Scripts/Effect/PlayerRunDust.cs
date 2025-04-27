using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRunDust : MonoBehaviour
{
    public ParticleSystem runDustEffect;
    private PlayerState playerState;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
    }

    void Update()
    {
        if (playerState.isRunning)
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