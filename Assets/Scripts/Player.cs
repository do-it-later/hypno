﻿using System.Collections;
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

	private ControllerInputManager cim;

	void Start()
	{
		cim = GetComponent<ControllerInputManager>();
	}

	void Update()
	{
		Debug.Log(cim.GetLeftTrigger());
		direction = new Vector2 (0, 0);
		if(Input.GetKey(KeyCode.W))
		{
			direction.y += 1;
		}
		if(Input.GetKey(KeyCode.S))
		{
			direction.y -= 1;
		}
		if(Input.GetKey(KeyCode.A))
		{
			direction.x -= 1;
		}
		if(Input.GetKey(KeyCode.D))
		{
			direction.x += 1;
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

	void OnCollisionEnter2D(Collision collision)
	{
		if(collision.gameObject.tag == "Hole")
		{
			Debug.Log("Dead");
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
