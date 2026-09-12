using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingleTon<AudioManager>
{
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    public string sound;
    AudioSource Audio;

    // Start is called before the first frame update
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
    }

    public void PlayAudio(string action)
    {
        switch (action)
        {
            case "JUMP":
                Audio.clip = audioJump;
                break;
            case "ATTACK":
                Audio.clip = audioAttack;
                break;
            case "DIE":
                Audio.clip = audioDie;
                break;
            case "DAMAGED":
                Audio.clip = audioDamaged;
                break;
            case "ITEM":
                Audio.clip = audioItem;
                break;
            case "FINISH":
                Audio.clip = audioFinish;
                break;

        }
        Audio.PlayOneShot(Audio.clip);
       
    }
}
