using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
	[Serializable]
	public class EnergyChangeEvent : UnityEvent<float> { }
	[Serializable]
	public class ResistanceChangeEvent : UnityEvent<float> { }

	[SerializeField]
	private EnergyChangeEvent energyChanged = new EnergyChangeEvent();
	[SerializeField]
	private ResistanceChangeEvent resistanceChanged = new ResistanceChangeEvent();
	public GameObject shotPrefab;

	[SerializeField]
	private GameObject opponent;
	private Player opponentPlayer;
	[SerializeField, HeaderAttribute("Possession")]
	private float possessedControl;
	private bool isPossessed;
	private bool isPossessingOpponent;
	public bool IsPossessingOpponent { set { isPossessingOpponent = value; } }
	[SerializeField]
	private int movementSpeed;
	private Vector2 direction;
	public Vector2 Direction { get { return direction; } }
	[SerializeField, HeaderAttribute("Dodge")]
	private int dodgeDistance;
	[SerializeField]
	private float dodgeCooldown;
	private float lastDodgeTime;
	
	[SerializeField, HeaderAttribute("Energy")]
	private float maximumEnergy = 100;
	private float energy;
	[SerializeField]
	private float passiveEnergyPerSec;
	[SerializeField]
	private float chargeEnergyPerSec;
	[SerializeField]
	private float timeBeforeEnergyCharge;

	[SerializeField, HeaderAttribute("Resistance")]
	private float maximumResistance = 100;
	private float resistance;

	[SerializeField, HeaderAttribute("Shot")]
	private float shotCooldown;
	[SerializeField]
	private float chargeShotCooldown;
	[SerializeField]
	private int normalShotPower;
	[SerializeField]
	private int normalShotEnergy;
	[SerializeField]
	private int chargeShotPower = 3;
	[SerializeField]
	private int chargeShotEnergy;
	[SerializeField]
	private int baseDamage;
	private int damage;
	private float lastShotTime = 0;
	private float lastChargeShotTime = 0;
	private float energyChargeStartTime = 0;

	private ControllerInputManager cim;
	private float shootAngle;

	void Start()
	{
		energy = maximumEnergy;
		resistance = maximumResistance;
		damage = baseDamage;
		opponentPlayer = opponent.GetComponent<Player>();

		cim = GetComponent<ControllerInputManager>();
	}

	void Update()
	{
		ChargeEnergy(passiveEnergyPerSec);
		direction = cim.GetLeftDirections();

		if(isPossessingOpponent)
			return;

		if (!cim.IsRightStickIdle ())
		{
			shootAngle = cim.GetRightAngle();
		}

		if(isPossessed)
		{
			direction = Vector2.Scale(cim.GetLeftDirections(), new Vector2(possessedControl, possessedControl)) + 
				Vector2.Scale(opponentPlayer.Direction, new Vector2(1 - possessedControl, 1 - possessedControl));
			Move();
			ChargeResistance(30);
		} 
		else 
		{
			if(cim.GetRightTrigger() > 0)
			{
				Shoot();
			}
			else if(cim.GetLeftTrigger() > 0)
			{
				ShootCharged();
			}

			if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.B)))
				energyChargeStartTime = Time.time;

			if(Input.GetKey(cim.GetButtonString(ControllerInputManager.Button.B)))
			{
				ChargeEnergy(chargeEnergyPerSec);
			}
			else if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.RB)))
			{
				Dodge();
			}
			else
			{
				Move();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Hole")
		{
			Destroy(gameObject);
			Debug.Log("Dead");
		}
	}

	private void Move()
	{
		Vector2 position = transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		transform.position = position;
	}

	private void Dodge()
	{
		if(Time.time - lastDodgeTime > dodgeCooldown && IsMoving())
		{
			Vector2 position = transform.position;
			var dir = direction.normalized;
			position.x += dir.x * dodgeDistance;
			position.y += dir.y * dodgeDistance;
			transform.position = position;

			lastDodgeTime = Time.time;
		}
	}

	private void Shoot()
	{
		if(Time.time - lastShotTime < shotCooldown)
			return;

		if(energy >= normalShotEnergy)
		{
			GameObject go = Instantiate(shotPrefab, transform.position, Quaternion.identity);
			var x = Mathf.Cos (shootAngle * Mathf.Deg2Rad);
			var y = Mathf.Sin (shootAngle * Mathf.Deg2Rad);
			var shot = go.GetComponent<Shot>();
			shot.direction = new Vector2(x, y).normalized;
			shot.shotPower = normalShotPower;
			shot.target = opponent;

			ReduceEnergy(normalShotEnergy);
			lastShotTime = Time.time;
		}
	}

	private void ShootCharged()
	{
		if(Time.time - lastChargeShotTime < chargeShotCooldown)
			return;

		if(energy >= chargeShotEnergy)
		{
			GameObject go = Instantiate(shotPrefab, transform.position, Quaternion.identity);
			var x = Mathf.Cos (shootAngle * Mathf.Deg2Rad);
			var y = Mathf.Sin (shootAngle * Mathf.Deg2Rad);
			var shot = go.GetComponent<Shot>();
			shot.direction = new Vector2(x, y).normalized;
			shot.shotPower = chargeShotPower;
			shot.target = opponent;

			ReduceEnergy(chargeShotEnergy);
			lastChargeShotTime = Time.time;
		}
	}

	public void ReduceResistance(int power)
	{
		resistance -= power * damage;

		if(resistance <= 0)
		{
			resistance = 0;
			isPossessed = true;
			opponentPlayer.IsPossessingOpponent = true;
		}

		resistanceChanged.Invoke(resistance);
	}

	private void ReduceEnergy(float amount)
	{
		energy -= amount;

		energyChanged.Invoke(energy);
	}

	private void ChargeEnergy(float amount)
	{
		if(Time.time - energyChargeStartTime < timeBeforeEnergyCharge)
			return;

		energy += Time.deltaTime * amount;

		if(energy > maximumEnergy)
		{
			energy = maximumEnergy;
		}

		energyChanged.Invoke(energy);
	}

	private void ChargeResistance(float amount)
	{
		resistance += Time.deltaTime * amount;

		if(resistance > maximumResistance)
		{
			resistance = maximumResistance;
			isPossessed = false;
			opponentPlayer.IsPossessingOpponent = false;
			damage = Mathf.CeilToInt(damage * 1.5f);
		}

		resistanceChanged.Invoke(resistance);
	}

	private bool IsMoving()
	{
		return !direction.x.Equals(0) || !direction.y.Equals(0);
	}
}
