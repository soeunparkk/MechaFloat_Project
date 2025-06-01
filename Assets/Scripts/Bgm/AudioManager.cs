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

 
    
    void ChangeAudio()
    {

    }
}
