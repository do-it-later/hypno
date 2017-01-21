using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shot : MonoBehaviour
{
	public Vector2 direction;
	public int shotPower;
	public GameObject target;
	public GameObject shooter;
	public Player shooterPlayer;
	public Color shotColor;

	[SerializeField]
	private int movementSpeed;

	void Start()
	{
		shooterPlayer = shooter.GetComponent<Player>();
	}

	void Update()
	{
		Vector2 position = this.transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		this.transform.position = position;

		GetComponent<SpriteRenderer>().color = shooterPlayer.ShotColor;
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Reflector")
		{
			direction = new Vector2(-direction.x, -direction.y);
			GameObject newShooter = target;
			target = shooter;
			shooter = newShooter;
			shooterPlayer = shooter.GetComponent<Player>();
			shotPower = Mathf.RoundToInt(1.1f * shotPower);
			movementSpeed = Mathf.RoundToInt(1.1f * movementSpeed);

			Debug.Log("hit");
		}
		else if(collider.tag == "Shot")
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

		else if(collider.name == target.name)
		{
			Debug.Log("shot");
			var player = collider.GetComponent<Player>();
			player.ReduceResistance(shotPower);
			player.OpponentShotHit();
			Destroy(gameObject);
		}
	}
}
