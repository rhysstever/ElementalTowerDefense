using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject currentCheckpoint;
	private EnemyType enemyType;
	private int health;	// Needs to be tracked since health per individual enemy will differ

	// Start is called before the first frame update
	void Start()
	{
		currentCheckpoint = MapManager.instance.Checkpoints[1];
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		Move();
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
		health = EnemyManager.instance.EnemyInfo[enemyType].Health;
		GetComponent<SpriteRenderer>().sprite = EnemyManager.instance.EnemyInfo[enemyType].Sprite;
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
			EnemyManager.instance.EnemyInfo[enemyType].MoveSpeed);
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
		health -= amount;

		if(health <= 0)
		{
			GameManager.instance.UpdateMoney(EnemyManager.instance.EnemyInfo[enemyType].GoldWorth);

			// Delete the enenmy and update the wave
			EnemyManager.instance.CurrentWave.EnemyRemoved();
			Destroy(gameObject);
		}
	}
}
