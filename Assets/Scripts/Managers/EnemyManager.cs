using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
	Normal,
	Tanky,
	Speedy
}

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
	private Dictionary<EnemyType, Sprite> enemySprites;
	private Dictionary<EnemyType, EnemyInfo> enemyInfo;

	private Wave currentWave;
	private float spawnTimer;

	// Properties
	public GameObject EnemyParent { get { return enemyParent; } }
	public Dictionary<EnemyType, EnemyInfo> EnemyInfo { get { return enemyInfo; } }
	public Wave CurrentWave { get { return currentWave; } }

	// Start is called before the first frame update
	void Start()
	{
		LoadEnemySprites();
		CreateEnemyInfoTypes();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			SpawnWave(currentWave);

		if(GameManager.instance.CurrentMenuState == MenuState.Game
			&& currentWave != null
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
				SpawnEnemy();
			}
		}
	}

	/// <summary>
	/// Loads sprites of each enemy type from the Resources folder
	/// </summary>
	private void LoadEnemySprites()
	{
		enemySprites = new Dictionary<EnemyType, Sprite>();
		Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Enemies");

		for(int i = 0; i < loadedSprites.Length; i++)
		{
			string enemyName = loadedSprites[i].name.Substring("enemy".Length);
			EnemyType type = (EnemyType)System.Enum.Parse(typeof(EnemyType), enemyName);
			enemySprites.Add(type, loadedSprites[i]);
		}
	}

	/// <summary>
	/// Create each type of enemy
	/// </summary>
	private void CreateEnemyInfoTypes()
	{
		enemyInfo = new Dictionary<EnemyType, EnemyInfo>();

		// Create Enemies
		enemyInfo.Add(EnemyType.Normal,
			new EnemyInfo(enemySprites[EnemyType.Normal], 5.0f, 2.0f, 5, 5));
		enemyInfo.Add(EnemyType.Tanky,
			new EnemyInfo(enemySprites[EnemyType.Tanky], 15.0f, 1.0f, 10, 10));
		enemyInfo.Add(EnemyType.Speedy,
			new EnemyInfo(enemySprites[EnemyType.Speedy], 3.0f, 4.0f, 2, 2));
	}

	/// <summary>
	/// Creates each enemy wave and sets initial values 
	/// </summary>
	public void SetupEnemyWaves()
	{
		// Create Waves
		Wave wave10 = new Wave("Wave 10", EnemyType.Tanky, 10, 2.5f);
		Wave wave9 = new Wave("Wave 9", EnemyType.Speedy, 25, 0.5f, wave10);
		Wave wave8 = new Wave("Wave 8", EnemyType.Normal, 16, 1.0f, wave9);
		Wave wave7 = new Wave("Wave 7", EnemyType.Tanky, 5, 2.5f, wave8);
		Wave wave6 = new Wave("Wave 6", EnemyType.Speedy, 15, 0.5f, wave7);
		Wave wave5 = new Wave("Wave 5", EnemyType.Normal, 12, 1.0f, wave6);
		Wave wave4 = new Wave("Wave 4", EnemyType.Tanky, 2, 2.5f, wave5);
		Wave wave3 = new Wave("Wave 3", EnemyType.Speedy, 12, 0.5f, wave4);
		Wave wave2 = new Wave("Wave 2", EnemyType.Normal, 8, 1.0f, wave3);
		Wave wave1 = new Wave("Wave 1", EnemyType.Normal, 5, 1.0f, wave2);

		// Set first wave
		currentWave = wave1;
		spawnTimer = currentWave.SpawnDelay;     // Allows the first enemy to spawn immediately

		// Update UI
		UIManager.instance.UpdateWaveText();
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
	private void SpawnEnemy()
	{
		// Make the enemy spawn at the first checkpoint
		Vector2 startingPos = MapManager.instance.Checkpoints[0].transform.position;

		// Create the enemy in the scene
		GameObject newEnemy = Instantiate(enemyPrefab, startingPos, Quaternion.identity, enemyParent.transform);

		// Set the game object's name in the scene and its stats
		newEnemy.name = "enemy" + (currentWave.EnemiesSpawned + 1);
		newEnemy.GetComponent<Enemy>().SetType(currentWave.EnemyType);

		// Tell the wave an enemy was spawned
		currentWave.EnemySpawned();
	}

	/// <summary>
	/// Displays the wave text information
	/// </summary>
	/// <returns>A string description of the wave</returns>
	public string GetWaveText()
	{
		// Return nothing if there is no current wave
		if(currentWave == null)
			return "";

		// If the wave is cleared, display the next wave's info
		if(currentWave.HasCleared
			&& currentWave.NextWave != null)
			return currentWave.NextWave.Description();
		else 
			return currentWave.Description();
	}

	// === Helper Methods ===

	/// <summary>
	/// Calculates the next move from one position to another
	/// </summary>
	/// <param name="startingPos">The starting position</param>
	/// <param name="targetPos">The goal position</param>
	/// <param name="moveSpeed">The amount of movement that can be made</param>
	/// <returns>The next movement (in 2D)</returns>
	public Vector3 CalculateMovement(Vector3 startingPos, Vector3 targetPos, float moveSpeed)
	{
		Vector2 direction = targetPos - startingPos;
		direction.Normalize();
		direction *= moveSpeed * Time.deltaTime;

		return direction;
	}
}
