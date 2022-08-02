using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public GameObject currentCheckpoint;

	private string enemyName;
	private int health;
	private int damage;
	private int goldWorth;
	private float moveSpeed;

	public string Name { get { return enemyName; } }

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
	/// Sets enemy stats
	/// </summary>
	/// <param name="enemyInfo">Stats for the enemy</param>
	public void SetStats(EnemyInfo enemyInfo)
	{
		enemyName = enemyInfo.EnemyName;
		health = enemyInfo.Health;
		damage = enemyInfo.Damage;
		goldWorth = enemyInfo.GoldWorth;
		moveSpeed = enemyInfo.MoveSpeed;
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
			moveSpeed);
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
			GameManager.instance.UpdateHealth(-damage);

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
			GameManager.instance.UpdateMoney(goldWorth);

			// Delete the enenmy and update the wave
			EnemyManager.instance.CurrentWave.EnemyRemoved();
			Destroy(gameObject);
		}
	}
}
