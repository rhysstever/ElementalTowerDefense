using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] // *Required* for tower panel selection
	private TowerType type;
	private GameObject tile;

	[SerializeField]
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
				Shoot();
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

	private void DetectEnemies()
	{
		inRangeEnemies.Clear();

		// Expensive - TODO: Simplify
		foreach(Transform enemyTrans in EnemyManager.instance.EnemyParent.transform)
			if(Vector2.Distance(transform.position, enemyTrans.position)
				<= TowerManager.instance.TowerInfo[type].Range)
				inRangeEnemies.Add(enemyTrans.gameObject);
	}

	private void TargetEnemy()
	{
		// Targetting first enemy in list - TODO: Change to be farthest along enemy
		if(inRangeEnemies.Count > 0)
			currentTarget = inRangeEnemies[0];
		else 
			currentTarget = null;
	}

	/// <summary>
	/// Checks whether the tower can shoot
	/// </summary>
	/// <returns>If the tower can shoot a bullet</returns>
	private bool CanShoot()
	{
		// Check there is a target to shoot at
		if(currentTarget == null)
		{
			// Debug.Log("Nothing to shoot at!");
			return false;
		}

		return true;
	}

	/// <summary>
	/// Shoots a bullet at the current target enemy
	/// </summary>
	private void Shoot()
	{
		if(!CanShoot())
			return;

		// Reset the attack timer
		attackTimer = 0.0f;

		// Shoot the bullet
		BulletManager.instance.SpawnBullet(gameObject, currentTarget);
	}
}
