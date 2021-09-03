using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

	#region Singleton

	public static AudioManager Instance { get; private set; } = null;

	private void Awake()
	{
		if(Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	#endregion

	[SerializeField] private Sound[] sounds;

	[SerializeField] private AudioMixer mainMixer;

	// Start is called before the first frame update
	void Start()
    {
		foreach(Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.loop = s.loop;
			s.source.pitch = s.pitch;
			s.source.outputAudioMixerGroup = s.channel;
		}
    }

	public void PlayAudio(string name, bool loop = false, float rampTime = 0)
	{
		// TODO: Add transition time so that audio clips can ramp down.
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if(s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.loop = loop;

		if(rampTime > 0)
		{
			PlaySoundRampClip(s, rampTime);
		}
		else
		{
			s.source.Play();
		}
	}

	public AudioSource GetAudioSource(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return null;
		}

		return s.source;
	}

	public float GetMixerVolume(string mixer)
	{
		float value;
		bool result = mainMixer.GetFloat(mixer, out value);

		if (result)
		{
			return  Mathf.Pow(10, value / 30);
		}
		else
		{
			Debug.LogWarning("Mixer parameter: " + mixer + "not found!");
			return 0.5f;
		}
	}

	public void SetMixerVolume(string mixer, float volume)
	{
		mainMixer.SetFloat(mixer, Mathf.Log10(volume) * 30f);
	}


	// -------------------------------------------------
	// Fun Audio modifications below.
	// -------------------------------------------------

	public void PlaySoundRampClip(Sound s, float rampTime)
	{
		s.source.volume = 0;
		s.source.Play();
		StartCoroutine(PlaySoundRampClipIEnumerator(s, rampTime));
	}

	IEnumerator PlaySoundRampClipIEnumerator(Sound s, float rampTime)
	{
		float steps = 0.02f;

		while(s.source.volume < 1f)
		{
			s.source.volume += steps / rampTime;
			yield return new WaitForSecondsRealtime(steps);
		}

		s.source.volume = 1f;
	}

	public void ChangeClipSpeed(string name, float targetPitch, float time = 0.5f, AnimationCurve curve = null)
	{
		StopCoroutine("ChangeClipSpeedIEnumerator");
		StartCoroutine(ChangeClipSpeedIEnumerator(name, targetPitch, time, curve));
	}

	IEnumerator ChangeClipSpeedIEnumerator(string name, float targetPitch, float time, AnimationCurve curve)
	{
		// TODO: Implement custom curve foor speed ramp.

		AudioSource source = GetAudioSource(name);
		if(source == null) { StopCoroutine("ChangeClipSpeedIEnumerator"); }

		float step = 0.02f;

		float pitchDelta = targetPitch - source.pitch;

		if (source.pitch > targetPitch)
		{
			while (source.pitch > targetPitch)
			{
				source.pitch += (pitchDelta * step) / time;
				yield return new WaitForSecondsRealtime(step);
			}
		}
		else
		{
			while (source.pitch < targetPitch)
			{
				source.pitch += (pitchDelta * step) / time;
				yield return new WaitForSecondsRealtime(step);
			}
		}
		source.pitch = targetPitch;

	}


	public void SetSubMixerVolume(string mixer, float volume = 1f, float time = 1f)
	{
		StopCoroutine("SubMixerVolumeIEnumerator");
		StartCoroutine(SubMixerVolumeIEnumerator(mixer, time, volume));
	}

	IEnumerator SubMixerVolumeIEnumerator(string mixer, float time, float targetVolume)
	{

		// Steps decides the time increment between each volume change.
		float steps = 0.02f;

		float currentVolume = GetMixerVolume(mixer);

		float volumeDelta = targetVolume - currentVolume;

		if (targetVolume < currentVolume)
		{
			while (targetVolume < currentVolume)
			{
				SetMixerVolume(mixer, currentVolume += (volumeDelta * steps / time));
				yield return new WaitForSecondsRealtime(steps);
			}
		}
		else
		{
			while (targetVolume > currentVolume)
			{
				SetMixerVolume(mixer, currentVolume += (volumeDelta * steps / time));
				yield return new WaitForSecondsRealtime(steps);
			}
		}
		SetMixerVolume(mixer, targetVolume);
	}

}
