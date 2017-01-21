using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shot : MonoBehaviour
{
	public Vector2 direction;
	public int damage;
	public GameObject target;
	public GameObject shooter;
	private Player shooterPlayer;

	[SerializeField]
	private int movementSpeed;

	void Start()
	{
		shooterPlayer = shooter.GetComponent<Player>();
	}

	void Update()
	{
		Vector2 position = transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		transform.position = position;

		GetComponent<SpriteRenderer>().color = shooterPlayer.ShotColor;	
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Shield")
		{
			direction = new Vector2(-direction.x, -direction.y);
			GameObject newTarget = shooter;
			shooter = target;
			target = newTarget;
			shooterPlayer = shooter.GetComponent<Player>();

			damage = Mathf.RoundToInt(damage * 1.1f);
			movementSpeed = Mathf.RoundToInt(movementSpeed * 1.1f);
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
