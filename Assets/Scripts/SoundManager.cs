using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
	#region Fields / Properties
	public AudioSource sfxSource;
	public AudioSource musicSource;
	public static SoundManager instance = null;
	#endregion

	#region MonoBehavior
	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}
	#endregion

	#region Public
	public void PlaySingleSfx(AudioClip clip)
	{
		sfxSource.clip = clip;
		sfxSource.loop = false;
		sfxSource.Play();
	}

	public void PlayLoopedSfx(AudioClip clip)
	{
		sfxSource.clip = clip;
		sfxSource.loop = true;
		sfxSource.Play();
	}

	public void StopSfx()
	{
		sfxSource.Stop();
	}

	public void MuteSfx(bool mute)
	{
		sfxSource.mute = mute;
	}

	public bool IsSfxMute()
	{
		return sfxSource.mute;
	}

	public void PlaySingleMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.loop = false;
		musicSource.Play();
	}

	public void PlayLoopedMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void StopMusic()
	{
		musicSource.Stop();
	}

	public void MuteMusic(bool mute)
	{
		musicSource.mute = mute;
	}

	public bool IsMusicMute()
	{
		return musicSource.mute;
	}
	#endregion
}
