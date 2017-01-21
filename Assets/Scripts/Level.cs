using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public GameObject holePrefab;

	void Start()
	{
		Vector2 holePosition = transform.position;
		holePosition.y += 0.5f;

		CreateHole(holePosition);
	}

	private void CreateHole(Vector2 position)
	{
		HoleSettings settings = new HoleSettings();
		settings.delay = 12.0f;
		settings.growthSpeed = new Vector2(0.2f, 0.1f);
		settings.maxSize = new Vector2(17.5f, 8.0f);

		GameObject hole = Instantiate(holePrefab, position, Quaternion.identity);
		hole.GetComponent<Hole>().settings = settings;
	}
}
