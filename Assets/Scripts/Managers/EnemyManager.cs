using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static EnemyManager instance = null;

	// Awake is called even before start 
	// (I think its at the very beginning of runtime)
	private void Awake()
	{
		// If the reference for this script is null, assign it this script
		if(instance == null)
			instance = this;
		// If the reference is to something else (it already exists)
		// than this is not needed, thus destroy it
		else if(instance != this)
			Destroy(gameObject);
	}
	#endregion

	[SerializeField]
	private GameObject enemyParent, enemyPrefab;

	private Wave currentWave;
	private float spawnTimer;

	// Properties
	public GameObject EnemyParent { get { return enemyParent; } }
	public Wave CurrentWave { get { return currentWave; } }

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			SpawnWave(currentWave);

		if(currentWave != null
			&& currentWave.HasCleared)
		{
			// Set the current wave to the next wave
			currentWave = currentWave.NextWave;

			// If there is a next wave, reset the spawn timer
			if(currentWave != null)
				spawnTimer = currentWave.SpawnDelay;
			// If there is no next wave, the game is won
			else
				GameManager.instance.EndGame(true);
		}
	}

	void FixedUpdate()
	{
		// If the wave has started spawning
		// AND has not been completed yet
		if(GameManager.instance.CurrentMenuState == MenuState.Game
			&& currentWave.HasSpawned
			&& !currentWave.HasCleared)
		{
			// Increments timer
			spawnTimer += Time.deltaTime;

			// If the timer has waited long enough
			// AND there are still enemies to be spawned
			if(spawnTimer >= currentWave.SpawnDelay
				&& currentWave.EnemiesSpawned < currentWave.EnemyCount)
			{
				// Resets timer and spawns an enemy
				spawnTimer = 0.0f;
				SpawnEnemy(currentWave.EnemyPrefab);
			}
		}
	}

	/// <summary>
	/// Creates each enemy wave and sets initial values 
	/// </summary>
	public void SetupEnemyWaves()
	{
		currentWave = CreateWaves();
		spawnTimer = currentWave.SpawnDelay;     // Allows the first enemy to spawn immediately
	}

	/// <summary>
	/// Creates each wave
	/// </summary>
	/// <returns>The first wave</returns>
	private Wave CreateWaves()
	{
		Wave wave2 = new Wave("Wave 2", enemyPrefab, 8, 1.0f);
		Wave wave1 = new Wave("Wave 1", enemyPrefab, 5, 1.0f, wave2);

		return wave1;
	}

	/// <summary>
	/// Spawns a wave
	/// </summary>
	/// <param name="wave">A wave of enemies</param>
	private void SpawnWave(Wave wave)
	{
		// Spawns the wave if the wave
		// 1) exists
		// 2) has not been beaten yet
		// 3) has not already been spawned
		if(wave != null
			&& !wave.HasCleared
			&& !wave.HasSpawned)
			wave.StartSpawn();
	}

	/// <summary>
	/// Creates an enemy in the scene
	/// </summary>
	/// <param name="enemyObject">The prefab of the enemy being spawned</param>
	private void SpawnEnemy(GameObject enemyObject)
	{
		// Make the enemy spawn at the first checkpoint
		Vector2 startingPos = MapManager.instance.Checkpoints[0].transform.position;

		GameObject newEnemy = Instantiate(enemyObject, startingPos, Quaternion.identity, enemyParent.transform);
		newEnemy.name = "enemy" + (enemyParent.transform.childCount - 1);

		// Tell the wave an enemy was spawned
		currentWave.EnemySpawned();
	}
}
