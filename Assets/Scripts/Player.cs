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
    [Serializable]
    public class DodgeCooldownEvent : UnityEvent<float> { }

	[SerializeField]
	private EnergyChangeEvent energyChanged = new EnergyChangeEvent();
	[SerializeField]
	private ResistanceChangeEvent resistanceChanged = new ResistanceChangeEvent();
	[SerializeField]
	private AccuracyChangeEvent accuracyChanged = new AccuracyChangeEvent();
    [SerializeField]
    private DodgeCooldownEvent dodgeCooldownChanged = new DodgeCooldownEvent();
	public GameObject smallShotPrefab;
	public GameObject longShotPrefab;
	[SerializeField]
	private GameObject teleportPrefab;
	[SerializeField]
	private GameObject shield;
	[SerializeField]
	private RoundManager roundManager;
    [SerializeField]
    private string playerName;
    public string PlayerName { get { return playerName; } }
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
	private float currentDodgeCooldown;
	
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
	public Color ShotColor { get { return shotColor; } }
	[SerializeField]
	private float shotCooldown;
	[SerializeField]
	private int normalShotPower;
	[SerializeField]
	private int normalShotEnergy;
	[SerializeField]
	private int baseDamage;
	private int damage;
	private float lastShotTime = 0;
	private float energyChargeStartTime = 0;
	private bool isCharging;
	private bool isShieldTriggered = false;
	private bool isShieldAllowed = true;
	private Vector3 initialPosition;
	private int shotsFired;
	private int shotsHit;
	private float shieldsUpTime = 0;
	public float ShieldsUpTime { get { return shieldsUpTime; } }
	private int shotsReflected = 0;
	public int ShotsReflected { get { return shotsReflected; } }

	[SerializeField, HeaderAttribute("Reflector")]
	private int initialReflectorCost;
	[SerializeField]
	private int reflectorMaintenanceCost;
	[SerializeField]
	private int minimumReflectorEnergyReq;

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
	[SerializeField]
	private AudioClip stalemateSfx;

	private ControllerInputManager cim;
	private SpriteRenderer sr;
	private float shootAngle;

	private Color originalColor;
    public Color OriginalColor { get { return originalColor;  } }

	void Awake()
	{
		shield.SetActive(false);
	}

	void Start()
	{
		energy = maximumEnergy;
		resistance = maximumResistance;
		damage = baseDamage;
		opponentPlayer = opponent.GetComponent<Player>();
		initialPosition = transform.position;

		cim = GetComponent<ControllerInputManager>();
		sr = GetComponent<SpriteRenderer>();

		originalColor = sr.color;
	}

	void Update()
	{
		if(roundManager.State == RoundManager.STATE.PLAYING)
		{
			ChargeEnergy(Time.deltaTime * passiveEnergyPerSec);
			ReduceDodgeCooldown();
			direction = cim.GetLeftDirections();

			if(isPossessingOpponent)
			{
				ToggleShields(false);
				return;
			}

			if (!cim.IsRightStickIdle())
			{
				shootAngle = cim.GetRightAngle();
			}

			if(!shield.activeInHierarchy)
			{
				// Face NORTH-EAST
				if(shootAngle > 22.5 && shootAngle < 67.5)
				{
					sr.sprite = playerDirectionSprites[1];
					sr.flipX = false;
				}
				// Face NORTH
				else if(shootAngle > 67.5 && shootAngle < 112.5)
				{
					sr.sprite = playerDirectionSprites[0];
					sr.flipX = false;
				}
				// Face NORTH-WEST
				else if(shootAngle > 112.5 && shootAngle < 157.5)
				{
					sr.sprite = playerDirectionSprites[2];
					sr.flipX = true;
				}
				// Face WEST
				else if(shootAngle > 157.5 && shootAngle < 202.5)
				{
					sr.sprite = playerDirectionSprites[2];
					sr.flipX = true;
				}
				// Face SOUTH-WEST
				else if(shootAngle > 202.5 && shootAngle < 247.5)
				{
					sr.sprite = playerDirectionSprites[3];
					sr.flipX = true;
				}
				// Face SOUTH
				else if(shootAngle > 247.5 && shootAngle < 292.5)
				{
					sr.sprite = playerDirectionSprites[4];
					sr.flipX = false;
				}
				// Face SOUTH-EAST
				else if(shootAngle > 292.5 && shootAngle < 337.5)
				{
					sr.sprite = playerDirectionSprites[3];
					sr.flipX = false;
				}
				// Face EAST
				else
				{
					sr.sprite = playerDirectionSprites[2];
					sr.flipX = false;
				}
			}

			if(isPossessed)
			{
				direction = Vector2.Scale(cim.GetLeftDirections(), new Vector2(possessedControl, possessedControl)) + 
					Vector2.Scale(opponentPlayer.Direction, new Vector2(1 - possessedControl, 1 - possessedControl));
				Move();
				ChargeResistance(Time.deltaTime * 30);
				sr.color = opponentPlayer.GetComponent<SpriteRenderer>().color;
				ToggleShields(false);
			} 
			else 
			{
				sr.color = originalColor;

				if(cim.GetRightTrigger() > 0)
				{
					Shoot();
					isCharging = false;
					ToggleShields(false);
				}
				else if(cim.GetLeftTrigger() > 0)
				{
					ActivateShield();
					isCharging = false;
				}
				else
				{
					ToggleShields(false);
					isShieldTriggered = false;
					isShieldAllowed = true;
				}
				
				if(!shield.activeInHierarchy)
				{
					if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.B)))
					{
						energyChargeStartTime = Time.time;
						isCharging = true;
					}
					else if(Input.GetKey(cim.GetButtonString(ControllerInputManager.Button.B)) && isCharging)
					{
						if(Time.time - energyChargeStartTime > timeBeforeEnergyCharge)
							ChargeEnergy(Time.deltaTime * chargeEnergyPerSec);

					}
					else if(Input.GetKeyDown(cim.GetButtonString(ControllerInputManager.Button.RB)))
					{
						Dodge();
						isCharging = false;
					}
					else
					{
						Move();
					}
				}
			}
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
		if(currentDodgeCooldown <= 0 && IsMoving())
		{
			SoundManager.instance.PlaySingleSfx(teleportSfx);
            GameObject go = Instantiate(teleportPrefab, transform.position, Quaternion.identity);
            go.GetComponent<SpriteRenderer>().color = originalColor;

            Vector2 position = transform.position;
			var dir = direction.normalized;
			position.x += dir.x * dodgeDistance;
			position.y += dir.y * dodgeDistance;
			transform.position = position;

			currentDodgeCooldown = dodgeCooldown;
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
			shot.damage = normalShotPower * baseDamage;
			shot.target = opponent;
			shot.shooter = gameObject;
			shot.transform.eulerAngles = new Vector3(0, 0, shootAngle);
			go.GetComponent<SpriteRenderer>().color = shotColor;

			ReduceEnergy(normalShotEnergy);
			lastShotTime = Time.time;
			shotsFired++;
			ModifyAccuracy();
		}
	}

	private void ActivateShield()
	{
		if(shield.activeInHierarchy && isShieldAllowed)
		{
			if(energy >= 0)
			{
				ReduceEnergy(reflectorMaintenanceCost * Time.deltaTime);
				shieldsUpTime += Time.deltaTime;
			} else {
				isShieldAllowed = false;
			}
		}
		else if(energy >= minimumReflectorEnergyReq && !isShieldTriggered)
		{
			ToggleShields(true);
			ReduceEnergy(initialReflectorCost);
			isShieldTriggered = true;
			shieldsUpTime += Time.deltaTime;
		} else {
			ToggleShields(false);
		}
	}

	private void ReduceDodgeCooldown()
	{
		if(currentDodgeCooldown > 0)
		{
			currentDodgeCooldown -= Time.deltaTime;
            dodgeCooldownChanged.Invoke(currentDodgeCooldown / dodgeCooldown);
		}
	}

	public void ReduceResistance(int damage)
	{
		resistance -= damage;

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
		SoundManager.instance.PlaySingleBacgroundSfx(stalemateSfx);

		resistance = 100;
		damage = Mathf.CeilToInt(damage * 1.5f);
		isPossessed = false;
		isPossessingOpponent = false;

		resistanceChanged.Invoke(resistance);
	}

	private void ReduceEnergy(float amount)
	{
		energy -= amount;

		energyChanged.Invoke(energy);
	}

	private void ChargeEnergy(float amount)
	{
		energy += amount;

		if(energy > maximumEnergy)
		{
			energy = maximumEnergy;
		}

		energyChanged.Invoke(energy);
	}

	private void ChargeResistance(float amount)
	{
		resistance += amount;

		if(resistance > maximumResistance)
		{
			resistance = maximumResistance;
			isPossessed = false;
			opponentPlayer.IsPossessingOpponent = false;
			damage = Mathf.CeilToInt(damage * 1.5f);
		}

		resistanceChanged.Invoke(resistance);
	}

	private void ToggleShields(bool on)
	{
		shield.SetActive(on);
		GetComponent<BoxCollider2D>().enabled = !on;
	}

	private bool IsMoving()
	{
		return !direction.x.Equals(0) || !direction.y.Equals(0);
	}

	private void ModifyAccuracy()
	{
		if(shotsFired > 0)
			accuracyChanged.Invoke(shotsHit / (float)shotsFired);
		else if(shotsHit > 0)
			accuracyChanged.Invoke(1);
		else
			accuracyChanged.Invoke(0);
	}

	public float GetAccuracy()
	{
		if(shotsFired > 0)
			return (shotsHit / (float)shotsFired);
		else if(shotsHit > 0)
			return 1;
		else
			return 0;
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
		ModifyAccuracy();
		resistanceChanged.Invoke(resistance);
		energyChanged.Invoke(energy);
		sr.color = originalColor;
		shieldsUpTime = 0;
		shotsReflected = 0;
	}

	public void OpponentShotHit()
	{
		opponentPlayer.ShotHit();
	}

	public void ShotHit()
	{
		shotsHit++;
		ModifyAccuracy();
		SoundManager.instance.PlaySingleSfx(gotHitSfx);
	}

	public void ReflectShot()
	{
		shotsReflected++;
	}
}
