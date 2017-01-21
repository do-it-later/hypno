using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shot : MonoBehaviour
{
	public Vector2 direction;
	public int damage;
	public GameObject target;

	[SerializeField]
	private int movementSpeed;

	void Update()
	{
		Vector2 position = transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		transform.position = position;
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Shield")
		{
			target.GetComponent<Player>().AbsorbEnergy();
			Destroy(gameObject);
		}
		else if(collider.name == target.name)
		{
			var player = collider.GetComponent<Player>();
			player.ReduceResistance(damage);
			player.OpponentShotHit();
			Destroy(gameObject);
		}
	}
}
