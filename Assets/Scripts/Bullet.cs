using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public Vector2 direction;
	public int shotPower;

	[SerializeField]
	private int movementSpeed;
	[SerializeField]
	private int range;

	private Vector2 startPosition;

	void Start()
	{
		startPosition = this.transform.position;

		Vector2 scale = this.transform.localScale;
		scale.x *= shotPower;
		scale.y *= shotPower;
		this.transform.localScale = scale;
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

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Bullet")
		{
			Bullet colliderBullet = collider.GetComponent<Bullet>();

			if(colliderBullet.shotPower == this.shotPower)
			{
				Destroy(this.gameObject);
				Destroy(collider.gameObject);
			}
			else if(colliderBullet.shotPower > this.shotPower)
			{
				Destroy(this.gameObject);
				colliderBullet.shotPower = 3;
			}
			else if(colliderBullet.shotPower < this.shotPower)
			{
				Destroy(collider.gameObject);
				this.shotPower = 3;
			}
		}
	}
}
