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

        health = 10;
        damage = 1;
        goldWorth = 5;
        moveSpeed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
        if(CanMove())
            Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Lose Health if it has collided with a bullet
        if(collision.gameObject.tag == "Bullet")
        {

        }

        // If it has reached the base (last checkpoint)
        if(collision.gameObject == currentCheckpoint
            && currentCheckpoint.GetComponent<Checkpoint>().Next == null)
            ReachedDestination();
    }

    /// <summary>
    /// Sets enemy stats
    /// </summary>
    /// <param name="health">How much damage the enemy can take</param>
    /// <param name="damage">How many damage the base will take if the enemy reaches it</param>
    /// <param name="moveSpeed">How fast the enemy can move</param>
    public void SetStats(int health, int damage, float moveSpeed)
	{
        this.health = health;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
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
		Vector2 direction = currentCheckpoint.transform.position - transform.position;
        direction.Normalize();
        direction *= moveSpeed * Time.deltaTime;

        transform.position += new Vector3(direction.x, direction.y, 0.0f);
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
            GameManager.instance.health -= damage;

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
            GameManager.instance.money += goldWorth;

            // Delete the enenmy and update the wave
            EnemyManager.instance.CurrentWave.EnemyRemoved();
            Destroy(gameObject);
        }
	}
}
