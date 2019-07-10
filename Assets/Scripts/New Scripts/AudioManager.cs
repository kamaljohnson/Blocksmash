using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        Play("theam");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
            Debug.LogWarning("sound " + name + " not found !!");
    }
    public void Mute()
    {
        if (GameManager.mute == false)
        {
            foreach (Sound s in sounds)
            {
                s.volume = 0;
                s.source.volume = 0;
            }
            GameManager.mute = true;
        }
        else
        {
            foreach (Sound s in sounds)
            {
                s.volume = 1;
                s.source.volume = 1;
            }
            GameManager.mute = false;
            Play("theam");
        }
    }
}
