using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public GameObject holePrefab;

	void Start()
	{
		CreateHole(this.transform.position);
	}

	private void CreateHole(Vector2 position)
	{
		HoleSettings settings = new HoleSettings();
		settings.delay = 12.0f;
		settings.growthSpeed = new Vector2(0.35f, 0.2f);
		settings.maxSize = new Vector2(24, 11);

		GameObject hole = Instantiate(holePrefab, position, Quaternion.identity);
		hole.GetComponent<Hole>().settings = settings;
	}
}
