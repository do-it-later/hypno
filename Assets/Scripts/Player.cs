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
	[Serializable]
	public class AccuracyChangeEvent : UnityEvent<float> { }

	[SerializeField]
	private EnergyChangeEvent energyChanged = new EnergyChangeEvent();
	[SerializeField]
	private ResistanceChangeEvent resistanceChanged = new ResistanceChangeEvent();
	[SerializeField]
	private AccuracyChangeEvent accuracyChanged = new AccuracyChangeEvent();
	public GameObject smallShotPrefab;
	public GameObject longShotPrefab;

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
	private Color shotColor;
	[SerializeField]
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
	private bool isCharging;
	private Vector3 initialPosition;
	private int shotsFired;
	private int shotsHit;

	[SerializeField, HeaderAttribute("Images")]
	private List<Sprite> playerDirectionSprites;

	[SerializeField, HeaderAttribute("SFX")]
	private AudioClip shotSmallSfx;
	[SerializeField]
	private AudioClip shotLargeSfx;
	[SerializeField]
	private AudioClip gotHitSfx;
	[SerializeField]
	private AudioClip teleportSfx;
	[SerializeField]
	private AudioClip hoverSfx;

	private ControllerInputManager cim;
	private float shootAngle;

	private Color originalColor;

	void Start()
	{
		energy = maximumEnergy;
		resistance = maximumResistance;
		damage = baseDamage;
		opponentPlayer = opponent.GetComponent<Player>();
		initialPosition = transform.position;

		cim = GetComponent<ControllerInputManager>();

		originalColor = GetComponent<SpriteRenderer>().color;
	}

	void Update()
	{
		ChargeEnergy(passiveEnergyPerSec);
		direction = cim.GetLeftDirections();

		if(isPossessingOpponent)
			return;

		if (!cim.IsRightStickIdle())
		{
			shootAngle = cim.GetRightAngle();

			float targetAngle = cim.GetRightAngle();
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

			// Face EAST
			if(targetAngle > 337.5 && targetAngle < 22.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[2];
				spriteRenderer.flipX = false;
			}
			// Face NORTH-EAST
			if(targetAngle > 22.5 && targetAngle < 67.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[1];
				spriteRenderer.flipX = false;
			}
			// Face NORTH
			if(targetAngle > 67.5 && targetAngle < 112.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[0];
				spriteRenderer.flipX = false;
			}
			// Face NORTH-WEST
			if(targetAngle > 112.5 && targetAngle < 157.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[1];
				spriteRenderer.flipX = true;
			}
			// Face WEST
			if(targetAngle > 157.5 && targetAngle < 202.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[2];
				spriteRenderer.flipX = true;
			}
			// Face SOUTH-WEST
			if(targetAngle > 202.5 && targetAngle < 247.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[3];
				spriteRenderer.flipX = true;
			}
			// Face SOUTH
			if(targetAngle > 247.5 && targetAngle < 292.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[4];
				spriteRenderer.flipX = false;
			}
			// Face SOUTH-EAST
			if(targetAngle > 292.5 && targetAngle < 337.5)
			{
				spriteRenderer.sprite = playerDirectionSprites[3];
				spriteRenderer.flipX = false;
			}
		}

		if(isPossessed)
		{
			direction = Vector2.Scale(cim.GetLeftDirections(), new Vector2(possessedControl, possessedControl)) + 
				Vector2.Scale(opponentPlayer.Direction, new Vector2(1 - possessedControl, 1 - possessedControl));
			Move();
			ChargeResistance(30);
			GetComponent<SpriteRenderer>().color = opponentPlayer.GetComponent<SpriteRenderer>().color;
		} 
		else 
		{
			GetComponent<SpriteRenderer>().color = originalColor;

			if(cim.GetRightTrigger() > 0)
			{
				Shoot();
				isCharging = false;
			}
			else if(cim.GetLeftTrigger() > 0)
			{
				ShootCharged();
				isCharging = false;
			}
			
			if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.B)))
			{
				energyChargeStartTime = Time.time;
				isCharging = true;
			}
			else if(Input.GetKey(cim.GetButtonString(ControllerInputManager.Button.B)) && isCharging)
			{
				ChargeEnergy(chargeEnergyPerSec);
			}
			else if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.RB)))
			{
				Dodge();
				isCharging = false;
			}
			else
			{
				Move();
				isCharging = false;
			}
		}
	}

	private void Move()
	{
//		SoundManager.instance.PlayLoopedSfx(hoverSfx);

		Vector2 position = transform.position;
		position.x += direction.x * movementSpeed * Time.deltaTime;
		position.y += direction.y * movementSpeed * Time.deltaTime;
		transform.position = position;
	}

	private void Dodge()
	{
		if(Time.time - lastDodgeTime > dodgeCooldown && IsMoving())
		{
			SoundManager.instance.PlaySingleSfx(teleportSfx);

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
			SoundManager.instance.PlaySingleSfx(shotSmallSfx);

			GameObject go = Instantiate(smallShotPrefab, transform.position, Quaternion.identity);
			var x = Mathf.Cos (shootAngle * Mathf.Deg2Rad);
			var y = Mathf.Sin (shootAngle * Mathf.Deg2Rad);
			var shot = go.GetComponent<Shot>();
			shot.direction = new Vector2(x, y).normalized;
			shot.shotPower = normalShotPower;
			shot.target = opponent;
			shot.transform.eulerAngles = new Vector3(0, 0, shootAngle);
			go.GetComponent<SpriteRenderer>().color = shotColor;

			ReduceEnergy(normalShotEnergy);
			lastShotTime = Time.time;
			shotsFired++;
			accuracyChanged.Invoke(shotsHit / (float)shotsFired);
		}
	}

	private void ShootCharged()
	{
		if(Time.time - lastChargeShotTime < chargeShotCooldown)
			return;

		if(energy >= chargeShotEnergy)
		{
			SoundManager.instance.PlaySingleSfx(shotLargeSfx);

			GameObject go = Instantiate(longShotPrefab, transform.position, Quaternion.identity);
			var x = Mathf.Cos (shootAngle * Mathf.Deg2Rad);
			var y = Mathf.Sin (shootAngle * Mathf.Deg2Rad);
			var shot = go.GetComponent<Shot>();
			shot.direction = new Vector2(x, y).normalized;
			shot.shotPower = chargeShotPower;
			shot.target = opponent;
			shot.transform.eulerAngles = new Vector3(0, 0, shootAngle);
			go.GetComponent<SpriteRenderer>().color = shotColor;

			ReduceEnergy(chargeShotEnergy);
			lastChargeShotTime = Time.time;
			shotsFired++;
			accuracyChanged.Invoke(shotsHit / (float)shotsFired);
		}
	}

	public void ReduceResistance(int power)
	{
		SoundManager.instance.PlaySingleSfx(gotHitSfx);

		resistance -= power * damage;

		if(resistance <= 0)
		{
			resistance = 0;
			// Edge case where you're already possessing then get possessed 
			if(isPossessingOpponent)
			{
				ResetWhenDoublePossessed();
				opponentPlayer.ResetWhenDoublePossessed();
			}
			isPossessed = true;
			opponentPlayer.IsPossessingOpponent = true;
		}

		resistanceChanged.Invoke(resistance);
	}

	public void ResetWhenDoublePossessed()
	{
		resistance = 100;
		damage = Mathf.CeilToInt(damage * 1.5f);
		isPossessed = false;
		isPossessingOpponent = false;
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

	public void RestartCharacter()
	{
		gameObject.SetActive(true);
		transform.position = initialPosition;
		resistance = maximumResistance;
		energy = maximumEnergy;
		damage = baseDamage;
		shotsHit = 0;
		shotsFired = 0;
		accuracyChanged.Invoke(0);
		resistanceChanged.Invoke(resistance);
		energyChanged.Invoke(energy);
	}

	public void OpponentShotHit()
	{
		opponentPlayer.ShotHit();
	}

	public void ShotHit()
	{
		shotsHit++;
		Debug.Log(shotsHit);
		accuracyChanged.Invoke(shotsHit / (float)shotsFired);
	}
}
