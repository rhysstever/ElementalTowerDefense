using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject currentCheckpoint;
	private EnemyType enemyType;

	// Need to be tracked since per enemy values will differ
	private int currentHealth; 
	private float currentMoveSpeed;
	private List<Affliction> afflictions;

	public float DistToCP { get { return Vector2.Distance(transform.position, currentCheckpoint.transform.position); } }
	public EnemyType Type { get { return enemyType; } }
	public int Health { get { return currentHealth; } }
	public float MoveSpeed { get { return currentMoveSpeed; } }

	// Start is called before the first frame update
	void Start()
	{
		currentCheckpoint = MapManager.instance.Checkpoints[1];
		afflictions = new List<Affliction>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		Process();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// If it has reached the base (last checkpoint)
		if(collision.gameObject == currentCheckpoint
			&& currentCheckpoint.GetComponent<Checkpoint>().Next == null)
			ReachedDestination();
	}

	/// <summary>
	/// Set the type of enemy the object is
	/// </summary>
	/// <param name="enemyInfo">The type of enemy</param>
	public void SetType(EnemyType type)
	{
		enemyType = type;
		currentHealth = EnemyManager.instance.EnemyInfo[enemyType].Health;
		GetComponent<SpriteRenderer>().sprite = EnemyManager.instance.EnemyInfo[enemyType].Sprite;
	}

	private void Process()
	{
		currentMoveSpeed = EnemyManager.instance.EnemyInfo[enemyType].MoveSpeed;

		ProcessAfflictions();

		Move();
	}

	/// <summary>
	/// Determines if the enemy can move based on current conditions
	/// </summary>
	/// <returns>True/False whether the enemy can move</returns>
	private bool CanMove()
	{
		// The enemy can only move in the game menu state
		if(GameManager.instance.CurrentMenuState != MenuState.Game)
			return false;

		return true;
	}

	/// <summary>
	/// Moves the enemy in the direction of its target checkpoint
	/// </summary>
	private void Move()
	{
		if(!CanMove())
			return;

		// Move the enemy closer to its current checkpoint
		transform.position += EnemyManager.instance.CalculateMovement(
			transform.position,
			currentCheckpoint.transform.position,
			currentMoveSpeed);
	}

	/// <summary>
	/// Handles the enemy reaching its target checkpoint
	/// </summary>
	public void ReachedDestination()
	{
		// Update the current CP to the next one
		if(currentCheckpoint.GetComponent<Checkpoint>().Next != null)
			currentCheckpoint = currentCheckpoint.GetComponent<Checkpoint>().Next;
		// If the enemy reaches the last checkpoint
		else
		{
			// Damage the base
			GameManager.instance.UpdateHealth(-EnemyManager.instance.EnemyInfo[enemyType].Damage);

			// Delete the enenmy and update the wave
			EnemyManager.instance.CurrentWave.EnemyRemoved();
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Deals damage to the enemy
	/// </summary>
	/// <param name="amount">The amount of damage being dealt to the enemy</param>
	public void TakeDamage(int amount)
	{
		currentHealth -= amount;

		if(BuildManager.instance.CurrentSelection == gameObject)
			UIManager.instance.UpdateSelectedObjectUI(gameObject);

		if(currentHealth <= 0)
		{
			// If the player is currently selecting the enemy, deselect it
			if(BuildManager.instance.CurrentSelection == gameObject)
				BuildManager.instance.Select(null);

			// Give the player the bounty for killing the enemy
			GameManager.instance.UpdateMoney(EnemyManager.instance.EnemyInfo[enemyType].GoldWorth);

			// Delete the enenmy and update the wave
			EnemyManager.instance.CurrentWave.EnemyRemoved();
			Destroy(gameObject);
		}
	}

	private void ProcessAfflictions()
	{
		for(int i = 0; i < afflictions.Count; i++)
		{
			if(afflictions[i].Duration > 0.0f)
			{
				switch(afflictions[i].Type)
				{
					case AfflictionType.DamageOverTime:
						break;
					case AfflictionType.Slow:
						Slow slowAffliction = (Slow)afflictions[i];
						currentMoveSpeed *= slowAffliction.SlowAmount;
						break;
				}
			}
			else
			{
				afflictions.RemoveAt(i);
				i--;
			}
		}
	}
}
