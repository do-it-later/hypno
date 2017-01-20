using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public GameObject bulletPrefab;

	[SerializeField]
	private int movementSpeed;
	private Vector2 direction;
	[SerializeField]
	private int dodgeDistance;
	[SerializeField]
	private float dodgeCooldown;
	private float lastDodgeTime;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			direction.y += 1;
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			direction.y -= 1;
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			direction.x -= 1;
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			direction.x += 1;
		}

		if(Input.GetKeyUp(KeyCode.W))
		{
			direction.y -= 1;
		}
		if(Input.GetKeyUp(KeyCode.S))
		{
			direction.y += 1;
		}
		if(Input.GetKeyUp(KeyCode.A))
		{
			direction.x += 1;
		}
		if(Input.GetKeyUp(KeyCode.D))
		{
			direction.x -= 1;
		}

		Move();

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Dodge();
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			Shoot();
		}
	}

	private void Move()
	{
		if(direction.x != 0 || direction.y != 0)
		{
			Vector2 position = this.transform.position;
			position.x += direction.x * movementSpeed * Time.deltaTime;
			position.y += direction.y * movementSpeed * Time.deltaTime;
			this.transform.position = position;
		}
	}

	private void Dodge()
	{
		if(Time.time - lastDodgeTime > dodgeCooldown && (direction.x != 0 || direction.y != 0))
		{
			Vector2 position = this.transform.position;
			position.x += direction.x * dodgeDistance;
			position.y += direction.y * dodgeDistance;
			this.transform.position = position;

			lastDodgeTime = Time.time;
		}
	}

	private void Shoot()
	{
		if(direction.x != 0 || direction.y != 0)
		{
			GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
			bullet.GetComponent<Bullet>().direction = direction.normalized;
		}
	}
}
