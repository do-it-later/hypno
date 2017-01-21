using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shot : MonoBehaviour
{
	public Vector2 direction;
	public int shotPower;
	public GameObject target;

	[SerializeField]
	private int movementSpeed;

	void Start()
	{
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
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Shot")
		{
			Shot colliderShot = collider.GetComponent<Shot>();

			if(colliderShot.shotPower == this.shotPower)
			{
				Destroy(this.gameObject);
				Destroy(collider.gameObject);
			}
			else if(colliderShot.shotPower > this.shotPower)
			{
				Destroy(this.gameObject);
				colliderShot.shotPower = 3;
			}
			else if(colliderShot.shotPower < this.shotPower)
			{
				Destroy(collider.gameObject);
				this.shotPower = 3;
			}
		}

		if(collider.name == target.name)
		{
			var player = collider.GetComponent<Player>();
			player.ReduceResistance(shotPower);
			player.OpponentShotHit();
			Destroy(gameObject);
		}
	}
}
