using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public Sound[] sounds;

	public static AudioManager instance;

    void Awake()
    {
    	
    	
    	if (instance == null)
    	{
    		instance = this;
    	}
    	else
    	{
    		Destroy(gameObject);
    		return;
    	}

		// keep AudioManager persistent between scenes
    	DontDestroyOnLoad(gameObject);

    	// create an audio source for each sound/music
        foreach (Sound s in sounds)
        {
        	s.source = gameObject.AddComponent<AudioSource>();
        	s.source.clip = s.clip;
        	s.source.volume = s.volume;
        	s.source.pitch = s.pitch;
        	s.source.loop = s.loop;
        	s.source.outputAudioMixerGroup = s.group;
        }
    }

    public void Play(string name)
    {
    	// look for audio source by that name
    	Sound s = Array.Find(sounds, sound => sound.name == name);

    	// if not found throw warning error
    	if (s == null)
    	{
    		Debug.LogWarning("Sound " + name + " not found");
    		return;
    	}

    	// otherwise play found sound
    	s.source.Play();
    }

    void Start()
    {
    	// play background music at start of game
    	Play("BGM");
    }
}
