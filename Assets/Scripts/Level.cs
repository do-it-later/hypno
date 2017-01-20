using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	public GameObject holePrefab;

	void Start()
	{
		CreateHole(Vector2.zero);
	}

	private void CreateHole(Vector2 position)
	{
		GameObject hole = Instantiate(holePrefab, position, Quaternion.identity);
		hole.GetComponent<Hole>().SetHoleValues(1, 0.25f, 15);
	}
}
