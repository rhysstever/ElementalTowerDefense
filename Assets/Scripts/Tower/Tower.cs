using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
	First, 
	Last,
	Strong,
	Weak
}

public class Tower : MonoBehaviour
{
	[SerializeField] // *Required* for tower panel selection
	private TowerType type;
	private GameObject tile;
	private TargetType targetType;

	private List<GameObject> inRangeEnemies;
	private GameObject currentTarget;
	private float attackTimer;

	// Properties
	public TowerType Type { get { return type; } }
	public GameObject Tile
	{
		get { return tile; }
		set { tile = value; }
	}

	// Start is called before the first frame update
	void Start()
	{
		targetType = TargetType.First;
		inRangeEnemies = new List<GameObject>();
		currentTarget = null;
		attackTimer = 0.0f;
	}

	void Update()
	{
		DetectEnemies();
		currentTarget = TargetEnemy();
	}

	void FixedUpdate()
	{
		if(tile != null
			&& GameManager.instance.CurrentMenuState == MenuState.Game)
		{
			// Increments timer
			attackTimer += Time.deltaTime;

			// The timer is up, try to shoot
			if(attackTimer >= TowerManager.instance.TowerInfo[type].AttackSpeed)
			{
				if(TowerManager.instance.TowerInfo[type].AOE)
					foreach(GameObject enemy in inRangeEnemies)
						Shoot(enemy);
				else
					Shoot(currentTarget);
			}
		}
	}

	/// <summary>
	/// Setup tower based on its type
	/// </summary>
	/// <param name="type">The tower's elemental type</param>
	public void SetupTower(TowerType type)
	{
		// Set type and sprite
		this.type = type;
		GetComponent<SpriteRenderer>().sprite = TowerManager.instance.TowerInfo[type].TowerSprite;
	}

	/// <summary>
	/// Finds and stores all enemies within the tower's range
	/// </summary>
	private void DetectEnemies()
	{
		inRangeEnemies.Clear();

		// Loop through each enemy and check if they are within range
		foreach(Transform enemyTrans in EnemyManager.instance.EnemyParent.transform)
			if(Vector2.Distance(transform.position, enemyTrans.position)
				<= TowerManager.instance.TowerInfo[type].Range)
				inRangeEnemies.Add(enemyTrans.gameObject);
	}

	/// <summary>
	/// Targets an enemy
	/// </summary>
	/// <returns>The gameObject the tower will target</returns>
	private GameObject TargetEnemy()
	{
		GameObject targettedEnemy = null;

		switch(targetType)
		{
			case TargetType.First:
				targettedEnemy = TargetFirstEnemy();
				break;
			case TargetType.Last:
				targettedEnemy = TargetLastEnemy();
				break;
			case TargetType.Strong:
				targettedEnemy = TargetStrongestEnemy();
				break;
			case TargetType.Weak:
				targettedEnemy = TargetWeakestEnemy();
				break;
		}

		return targettedEnemy;
	}

	/// <summary>
	/// Targets the enemy that is closest to the exit
	/// </summary>
	/// <returns>The enemy gameobject that is first</returns>
	private GameObject TargetFirstEnemy()
	{
		// If there at least one enemy within range, set the initial enemy to be the first one in the list
		GameObject firstEnemy = inRangeEnemies.Count > 0 ? inRangeEnemies[0] : null;

		foreach(GameObject enemy in inRangeEnemies)
		{
			// Skip self-checks
			if(firstEnemy == enemy)
				continue;

			// Get info about both enemies being compared
			Enemy firstEnemyData = firstEnemy.GetComponent<Enemy>();
			Enemy enemyData = enemy.GetComponent<Enemy>();

			int firstEnemyCheckpointNum = int.Parse(firstEnemyData.currentCheckpoint.name.Substring("checkpoint".Length));
			int enemyCheckpointNum = int.Parse(enemyData.currentCheckpoint.name.Substring("checkpoint".Length));

			// An enemy is ahead if it is moving towards a larger numbered checkpoint
			if(enemyCheckpointNum > firstEnemyCheckpointNum)
				firstEnemy = enemy;
			// If the checkpoints are the same, compare the distances to the checkpoint
			else if(enemyCheckpointNum == firstEnemyCheckpointNum)
			{
				// An enemy is ahead if its distance to the checkpoint is shorter
				if(enemyData.DistToCP < firstEnemyData.DistToCP)
					firstEnemy = enemy;
			}
		}

		return firstEnemy;
	}

	/// <summary>
	/// Targets the enemy that is closest to the start
	/// </summary>
	/// <returns>The enemy gameobject</returns>
	private GameObject TargetLastEnemy()
	{
		// If there at least one enemy within range, set the initial enemy to be the last one in the list
		GameObject lastEnemy = inRangeEnemies.Count > 0 ? inRangeEnemies[inRangeEnemies.Count - 1] : null;

		foreach(GameObject enemy in inRangeEnemies)
		{
			// Skip self-checks
			if(lastEnemy == enemy)
				continue;

			// Get info about both enemies being compared
			Enemy lastEnemyData = lastEnemy.GetComponent<Enemy>();
			Enemy enemyData = enemy.GetComponent<Enemy>();

			int lastEnemyCheckpointNum = int.Parse(lastEnemyData.currentCheckpoint.name.Substring("checkpoint".Length));
			int enemyCheckpointNum = int.Parse(enemyData.currentCheckpoint.name.Substring("checkpoint".Length));

			// An enemy is behind if it is moving towards a smaller numbered checkpoint
			if(enemyCheckpointNum < lastEnemyCheckpointNum)
				lastEnemy = enemy;
			// If the checkpoints are the same, compare the distances to the checkpoint
			else if(enemyCheckpointNum == lastEnemyCheckpointNum)
			{
				// An enemy is behind if its distance to the checkpoint is larger
				if(enemyData.DistToCP > lastEnemyData.DistToCP)
					lastEnemy = enemy;
			}
		}

		return lastEnemy;
	}

	/// <summary>
	/// Targets the enemy that has the most health
	/// </summary>
	/// <returns>The enemy gameobject</returns>
	private GameObject TargetStrongestEnemy()
	{
		// If there at least one enemy within range, set the initial enemy to be the first one in the list
		GameObject strongestEnemy = inRangeEnemies.Count > 0 ? inRangeEnemies[0] : null;

		foreach(GameObject enemy in inRangeEnemies)
		{
			// Skip self-checks
			if(strongestEnemy == enemy)
				continue;

			// Get info about both enemies being compared
			Enemy strongestEnemyData = strongestEnemy.GetComponent<Enemy>();
			Enemy enemyData = enemy.GetComponent<Enemy>();

			// An enemy is stronger if it has more health
			if(enemyData.CurrentHealth > strongestEnemyData.CurrentHealth)
				strongestEnemy = enemy;
			// If enemies have the same health, target the most ahead (first) one
			else if(enemyData.CurrentHealth == strongestEnemyData.CurrentHealth)
			{
				int firstEnemyCheckpointNum = int.Parse(strongestEnemyData.currentCheckpoint.name.Substring("checkpoint".Length));
				int enemyCheckpointNum = int.Parse(enemyData.currentCheckpoint.name.Substring("checkpoint".Length));

				// An enemy is ahead if it is moving towards a larger numbered checkpoint
				if(enemyCheckpointNum > firstEnemyCheckpointNum)
					strongestEnemy = enemy;
				// If the checkpoints are the same, compare the distances to the checkpoint
				else if(enemyCheckpointNum == firstEnemyCheckpointNum)
				{
					// An enemy is ahead if its distance to the checkpoint is shorter
					if(enemyData.DistToCP < strongestEnemyData.DistToCP)
						strongestEnemy = enemy;
				}
			}
		}

		return strongestEnemy;
	}

	/// <summary>
	/// Targets the enemy that has the least health
	/// </summary>
	/// <returns>The enemy gameobject</returns>
	private GameObject TargetWeakestEnemy()
	{
		// If there at least one enemy within range, set the initial enemy to be the first one in the list
		GameObject weakestEnemy = inRangeEnemies.Count > 0 ? inRangeEnemies[0] : null;

		foreach(GameObject enemy in inRangeEnemies)
		{
			// Skip self-checks
			if(weakestEnemy == enemy)
				continue;

			// Get info about both enemies being compared
			Enemy weakestEnemyData = weakestEnemy.GetComponent<Enemy>();
			Enemy enemyData = enemy.GetComponent<Enemy>();

			// An enemy is weaker if it has less health
			if(enemyData.CurrentHealth < weakestEnemyData.CurrentHealth)
				weakestEnemy = enemy;
			// If enemies have the same health, target the most ahead (first) one
			else if(enemyData.CurrentHealth == weakestEnemyData.CurrentHealth)
			{
				int firstEnemyCheckpointNum = int.Parse(weakestEnemyData.currentCheckpoint.name.Substring("checkpoint".Length));
				int enemyCheckpointNum = int.Parse(enemyData.currentCheckpoint.name.Substring("checkpoint".Length));

				// An enemy is ahead if it is moving towards a larger numbered checkpoint
				if(enemyCheckpointNum > firstEnemyCheckpointNum)
					weakestEnemy = enemy;
				// If the checkpoints are the same, compare the distances to the checkpoint
				else if(enemyCheckpointNum == firstEnemyCheckpointNum)
				{
					// An enemy is ahead if its distance to the checkpoint is shorter
					if(enemyData.DistToCP < weakestEnemyData.DistToCP)
						weakestEnemy = enemy;
				}
			}
		}

		return weakestEnemy;
	}

	/// <summary>
	/// Checks whether the tower can shoot
	/// </summary>
	/// <param name="target">The object being shot at</param>
	/// <returns>If the tower can shoot a bullet</returns>
	private bool CanShoot(GameObject target)
	{
		// Check there is a target to shoot at
		if(target == null)
			return false;

		return true;
	}

	/// <summary>
	/// Shoots a bullet at the target
	/// </summary>
	/// <param name="target">The object being shot at</param>
	private void Shoot(GameObject target)
	{
		if(!CanShoot(target))
			return;

		// Reset the attack timer
		attackTimer = 0.0f;

		// Shoot the bullet
		BulletManager.instance.SpawnBullet(gameObject, target);
	}
}
