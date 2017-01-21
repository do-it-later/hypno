using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public AudioClip menuMusic;
	public AudioClip gameStartMusic;
	public AudioClip gameBuildupMusic;
	public AudioClip gameEndingLoopMusic;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else if(instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	void Start()
	{
		SoundManager.instance.PlayLoopedMusic(menuMusic);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine(PlayGameMusic());
		}
	}

	IEnumerator PlayGameMusic()
	{
		SoundManager.instance.PlaySingleMusic(gameStartMusic);
		yield return new WaitForSeconds(gameStartMusic.length);
		SoundManager.instance.PlaySingleMusic(gameBuildupMusic);
		yield return new WaitForSeconds(gameBuildupMusic.length);
		SoundManager.instance.PlayLoopedMusic(gameEndingLoopMusic);
	}
}
