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
	/// Set the bullet's originating tower
	/// </summary>
	/// <param name="tower">The tower game object the bullet was fired from</param>
	public void SetTower(GameObject tower)
	{
		if(tower.tag != "Tower")
			return;

		this.tower = tower;
		// Calculate the damage of the bullet based on the tower
		damage = TowerManager.instance.TowerInfo[tower.GetComponent<Tower>().Type].Damage;
	}

	/// <summary>
	/// Set the bullet's target
	/// </summary>
	/// <param name="enemy">An enemy game object</param>
	public void SetTargetEnemy(GameObject enemy)
	{
		if(tower.tag != "Enemy")
			return;

		targetEnemy = enemy;
	}

	/// <summary>
	/// Moves the bullet towards the target enemy
	/// </summary>
	private void Move()
	{
		// Destroy the bullet if there is no target
		if(targetEnemy == null)
			Destroy(gameObject);

		// Move the bullet closer to the target enemy
		transform.position += EnemyManager.instance.CalculateMovement(
			transform.position,
			targetEnemy.transform.position,
			moveSpeed);
	}
}
