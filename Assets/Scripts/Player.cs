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
	[SerializeField]
	private int normalShotEnergy;
	[SerializeField]
	private int chargeShotEnergy;
	[SerializeField]
	private int chargeShotPower;
	[SerializeField]
	private float energyPerSec;
	private bool chargingEnergy;
	private float energy;

	[SerializeField]
	private float shotCooldown;
	private float lastShotTime = 0;

	private ControllerInputManager cim;

	void Start()
	{
		energy = 100;
		chargingEnergy = false;

		cim = GetComponent<ControllerInputManager>();
	}

	void Update()
	{
		if (!cim.IsRightStickIdle ())
		{
			transform.eulerAngles = new Vector3(0, 0, cim.GetRightAngle());
		}

		direction = new Vector2 (0, 0);
		direction = cim.GetLeftDirections();
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

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Dodge();
		}

		if(Input.GetKeyDown(KeyCode.E) || cim.GetRightTrigger() > 0)
		{
			Shoot();
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			chargingEnergy = true;
		}
		else if(Input.GetKeyUp(KeyCode.Q))
		{
			chargingEnergy = false;
		}

		if(chargingEnergy)
		{
			ChargeEnergy();

		}
		else
		{
			Move();
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.tag == "Hole")
		{
			Destroy(gameObject);
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
		if(Time.time - lastShotTime < shotCooldown)
			return;

		if(energy > normalShotEnergy)
		{
			GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
			var x = Mathf.Cos (transform.eulerAngles.z * Mathf.Deg2Rad);
			var y = Mathf.Sin (transform.eulerAngles.z * Mathf.Deg2Rad);
			bullet.GetComponent<Bullet>().direction = new Vector2(x, y).normalized;

			energy -= normalShotEnergy;
		}
	}

	private void ShootCharged()
	{
		if(energy > chargeShotEnergy)
		{
			GameObject bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
			var x = Mathf.Cos (transform.eulerAngles.z * Mathf.Deg2Rad);
			var y = Mathf.Sin (transform.eulerAngles.z * Mathf.Deg2Rad);
			bullet.GetComponent<Bullet>().direction = new Vector2(x, y).normalized;
			bullet.GetComponent<Bullet>().shotPower = chargeShotPower;

			energy -= chargeShotEnergy;
		}
	}

	private void ChargeEnergy()
	{
		energy += 1/Time.deltaTime * energyPerSec;

		if(energy > 100)
		{
			energy = 100;
		}
	}
}
