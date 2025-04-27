using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip stage1bgm;
    public AudioClip stage2bgm;

    public Transform player;
    public Transform stage2Tile;

    private AudioSource audio;
    private AudioClip currentClip;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.y < stage2Tile.position.y)
        {
            if (currentClip != stage1bgm)
            {
                currentClip = stage1bgm;
                audio.clip = currentClip;
                audio.Play();
            }
        }
        
        else if (player.position.y > stage2Tile.position.y)
        {
            if (currentClip != stage2bgm)
            {
                currentClip = stage2bgm;
                audio.clip = currentClip;
                audio.Play();
            }
        }
    }

    void ChangeAudio()
    {

    }
}
