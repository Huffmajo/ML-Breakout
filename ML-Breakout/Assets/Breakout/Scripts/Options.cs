using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
	public AudioMixer audioMixer;

	// set exposed musicVolume mixer level
	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
	}

	// set exposed soundVolume mixer level
	public void SetSoundVolume(float volume)
	{
		audioMixer.SetFloat("soundVolume", Mathf.Log10(volume) * 20);
	}
}
