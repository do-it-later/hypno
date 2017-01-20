using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Vector2 direction;

	[SerializeField]
	private int movementSpeed;
	[SerializeField]
	private int range;
	private Vector2 startPosition;

	void Start()
	{
		startPosition = this.transform.position;
	}

	void Update()
	{
		Vector2 position = this.transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		this.transform.position = position;

		if(Vector2.Distance(this.transform.position, startPosition) > range)
		{
			Destroy(gameObject);
		}
	}
}
