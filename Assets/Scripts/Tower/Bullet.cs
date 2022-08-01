using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	private float moveSpeed;
	private GameObject tower;
	private int damage;
	private GameObject targetEnemy;

	// Properties
	public int Damage { get { return damage; } }

	// Start is called before the first frame update
	void Start()
	{
		moveSpeed = 8.0f;
	}

	void FixedUpdate()
	{
		if(GameManager.instance.CurrentMenuState == MenuState.Game)
			Move();
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		// If the bullet hit its target enemy object
		if(collision.gameObject == targetEnemy)
		{
			// Deal damage to the enemy
			targetEnemy.GetComponent<Enemy>().TakeDamage(Damage);

			// Destroy this bullet
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Sets the bullet's starting and end "points"
	/// </summary>
	/// <param name="tower">The tower the bullet is being shot from</param>
	/// <param name="enemy">The bullet's target</param>
	public void SetEndpoints(GameObject tower, GameObject enemy)
	{
		if(tower.tag == "Tower")
		{
			this.tower = tower;
			TowerInfo towerInfo = TowerManager.instance.TowerInfo[tower.GetComponent<Tower>().Type];

			// Set the sprite of the bullet
			GetComponent<SpriteRenderer>().sprite = towerInfo.BulletSprite;

			// Calculate the damage of the bullet based on the tower
			damage = towerInfo.Damage;
		}

		if(enemy.tag == "Enemy")
			targetEnemy = enemy;
	}

	/// <summary>
	/// Moves the bullet towards the target enemy
	/// </summary>
	private void Move()
	{
		// Destroy the bullet if there is no target
		if(targetEnemy == null)
		{
			Destroy(gameObject);
			return;
		}

		// Move the bullet closer to the target enemy
		transform.position += EnemyManager.instance.CalculateMovement(
			transform.position,
			targetEnemy.transform.position,
			moveSpeed);
	}
}
