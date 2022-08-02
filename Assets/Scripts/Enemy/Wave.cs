using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
	private string name;
	private int numOfEnemies;
	private EnemyInfo enemyInfo;
	private float spawnDelay;
	private bool hasSpawned;
	private bool hasCleared;
	private int numSpawned;
	private int numLeft;
	private Wave nextWave;

	public string Name { get { return name; } }
	public EnemyInfo EnemyInfo { get { return enemyInfo; } }
	public int EnemyCount { get { return numOfEnemies; } }
	public float SpawnDelay { get { return spawnDelay; } }
	public bool HasSpawned { get { return hasSpawned; } }
	public bool HasCleared { get { return hasCleared; } }
	public int EnemiesSpawned { get { return numSpawned; } }
	public int EnemiesLeft { get { return numLeft; } }
	public Wave NextWave { get { return nextWave; } }

	/// <summary>
	/// Creates a wave of enemies
	/// </summary>
	/// <param name="name">The name of the wave</param>
	/// <param name="enemyInfo">The enemy info</param>
	/// <param name="numOfEnemies">The number of enemies</param>
	/// <param name="spawnDelay">How long in between each enemy will be spawned</param>
	/// <param name="nextWave">The next wave that follows after this one is complete</param>
	public Wave(string name, EnemyInfo enemyInfo, int numOfEnemies, float spawnDelay, Wave nextWave) : this(name, enemyInfo, numOfEnemies, spawnDelay)
	{
		this.nextWave = nextWave;
	}

	/// <summary>
	/// Creates a wave of enemies
	/// </summary>
	/// <param name="name">The name of the wave</param>
	/// <param name="enemyInfo">The enemy info</param>
	/// <param name="numOfEnemies">The number of enemies</param>
	/// <param name="spawnDelay">How long in between each enemy will be spawned</param>
	public Wave(string name, EnemyInfo enemyInfo, int numOfEnemies, float spawnDelay)
	{
		this.name = name;
		this.enemyInfo = enemyInfo;
		this.numOfEnemies = numOfEnemies;
		this.spawnDelay = spawnDelay;
		hasSpawned = false;
		hasCleared = false;
		numSpawned = 0;
		numLeft = numOfEnemies;
	}

	/// <summary>
	/// Starts a wave
	/// </summary>
	public void StartSpawn()
	{
		if(hasCleared)
		{
			Debug.LogError("Wave already cleared");
			return;
		}
		else if(hasSpawned)
		{
			Debug.LogError("Wave already spawned");
			return;
		}

		hasSpawned = true;
	}

	/// <summary>
	/// Adds 1 to the count of enemies spawned
	/// </summary>
	public void EnemySpawned()
	{
		if(!hasSpawned)
		{
			Debug.LogError("Wave not spawned yet");
			return;
		}
		else if(hasCleared)
		{
			Debug.LogError("Wave already cleared");
			return;
		}

		numSpawned++;
	}

	/// <summary>
	/// Removes an enemy from the wave enemy count
	/// </summary>
	public void EnemyRemoved()
	{
		numLeft--;

		if(numLeft == 0)
		{
			hasCleared = true;
			UIManager.instance.UpdateWaveText();
		}
	}

	/// <summary>
	/// Combines wave info into a full description
	/// </summary>
	/// <returns>A full description of the wave</returns>
	public string Description()
	{
		return name + ": \n" + numOfEnemies + " " + enemyInfo.EnemyName + " enemies";
	}
}
