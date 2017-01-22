using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct HoleSettings
{
	public float delay;
	public Vector2 growthSpeed;
	public Vector2 maxSize;
}

public class Hole : MonoBehaviour
{
	public HoleSettings settings;

	private float creationTime;

	[SerializeField]
	private RoundManager roundManager;

	void Start()
	{
		creationTime = Time.time;
	}

	void Update()
	{
		if(roundManager.State == RoundManager.STATE.PLAYING)
		{
			Vector2 scale = this.transform.localScale;
			float currentTime = Time.time;

			if(currentTime - creationTime > settings.delay && scale.x < settings.maxSize.x)
			{
				scale.x += settings.growthSpeed.x * Time.deltaTime;
			}
			if(currentTime - creationTime > settings.delay && scale.y < settings.maxSize.y)
			{
				scale.y += settings.growthSpeed.y * Time.deltaTime;
			}

			this.transform.localScale = scale;
		}
	}

	public void RestartHole()
	{
		creationTime = Time.time;
		transform.localScale = new Vector3(0, 0, 1);
	}
}
