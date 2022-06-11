using UnityEngine.Audio;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
        }

        DontDestroyOnLoad(gameObject);
    }

    
   

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopPlay(string name)
    {
        
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void pauseMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
    
    public void unpauseMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
    }

}
