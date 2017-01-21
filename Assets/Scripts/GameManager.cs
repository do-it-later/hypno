using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public AudioClip menuMusic;
	public AudioClip gameStartMusic;
	public AudioClip gameBuildupMusic;
	public AudioClip gameEndingLoopMusic;

	private bool playingGameLoop;

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

		playingGameLoop = false;
	}

	void Update()
	{
		Scene currentScene = SceneManager.GetActiveScene();

		if(currentScene.name == "Main Menu" && Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene("Pat's Other Scene");
		}
		if(currentScene.name == "Pat's Other Scene" && !playingGameLoop)
		{
			StartCoroutine(PlayGameMusic());
			playingGameLoop = true;
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
