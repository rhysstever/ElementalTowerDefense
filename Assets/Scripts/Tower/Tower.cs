using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] // *Required* for tower panel selection
	private TowerType type;
	private GameObject tile;

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
		inRangeEnemies = new List<GameObject>();
		currentTarget = null;
		attackTimer = 0.0f;
	}

	void Update()
	{
		DetectEnemies();
		TargetEnemy();
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

		// TODO: Simplify, its currently expensive
		foreach(Transform enemyTrans in EnemyManager.instance.EnemyParent.transform)
			if(Vector2.Distance(transform.position, enemyTrans.position)
				<= TowerManager.instance.TowerInfo[type].Range)
				inRangeEnemies.Add(enemyTrans.gameObject);
	}

	/// <summary>
	/// Sets the current target
	/// </summary>
	private void TargetEnemy()
	{
		// TODO: Change to be farthest along enemy
		// Targetting first enemy in list
		if(inRangeEnemies.Count > 0)
			currentTarget = inRangeEnemies[0];
		else 
			currentTarget = null;
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
