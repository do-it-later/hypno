using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
	private int delay;
	private float growthSpeed;
	private int maxSize;

	private float creationTime;


	void Start()
	{
		creationTime = Time.time;
	}

	void Update()
	{
		Vector2 scale = this.transform.localScale;

		if(Time.time - creationTime > delay && scale.x < maxSize && scale.y < maxSize)
		{
			scale.x += growthSpeed * Time.deltaTime;
			scale.y += growthSpeed * Time.deltaTime;
			this.transform.localScale = scale;
		}
	}

	public void SetHoleValues(int delay, float growthSpeed, int maxSize)
	{
		this.delay = delay;
		this.growthSpeed = growthSpeed;
		this.maxSize = maxSize;
	}
}
