using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
	// Tier 1
	Air,
	Earth,
	Fire,
	Water,

	// Tier 2 - single elemet
	Earthquake,
	Flamethrower,
	Tornado,
	Tsunami,

	// Tier 2 - double element
	Blizzard,
	Flood,
	Volcano,
	Wildfire
}

public class TowerManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static TowerManager instance = null;

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
	private GameObject towerPrefab;

	private Dictionary<TowerType, Sprite> towerSprites;
	private Dictionary<TowerType, Sprite> bulletSprites;
	private Dictionary<TowerType, TowerInfo> towerInfo;
	private TowerType selectedTypeInfo;

	// Properties
	public GameObject TowerPrefab { get { return towerPrefab; } }
	public Dictionary<TowerType, TowerInfo> TowerInfo { get { return towerInfo; } }
	public TowerType SelectedTypeInfo 
	{ 
		get { return selectedTypeInfo; }
		set { selectedTypeInfo = value; }
	}

	// Start is called before the first frame update
	void Start()
	{
		LoadTowerSprites();
		CreateTowerInfoDictionary();
	}

	/// <summary>
	/// Loads sprites of each tower and bullet type from the Resources folder
	/// </summary>
	private void LoadTowerSprites()
	{
		// Load Tower Sprites
		towerSprites = new Dictionary<TowerType, Sprite>();
		Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/Elements");

		for(int i = 0; i < loadedSprites.Length; i++)
		{
			string towerName = loadedSprites[i].name.Substring("icon".Length);
			TowerType type = (TowerType)System.Enum.Parse(typeof(TowerType), towerName);
			towerSprites.Add(type, loadedSprites[i]);
		}

		// Load Bullet Sprites
		bulletSprites = new Dictionary<TowerType, Sprite>();
		Sprite[] loadedBulletSprites = Resources.LoadAll<Sprite>("Sprites/Bullets");

		for(int i = 0; i < loadedBulletSprites.Length; i++)
		{
			string bulletElement = loadedBulletSprites[i].name.Substring("bullet".Length);
			TowerType type = (TowerType)System.Enum.Parse(typeof(TowerType), bulletElement);
			bulletSprites.Add(type, loadedBulletSprites[i]);
		}
	}

	/// <summary>
	/// Sets info for each type of tower
	/// </summary>
	private void CreateTowerInfoDictionary()
	{
		towerInfo = new Dictionary<TowerType, TowerInfo>();

		#region TowerInfo for each element
		// Calculations:
		// DPS* = Damage / sec
		// CPDPS = Cost / DPS*
		// 
		// *Fire tower's fire dps affliction is denoted (+#)

		// Tier 1
		towerInfo.Add(TowerType.Air,
			new TowerInfo(towerSprites[TowerType.Air], bulletSprites[TowerType.Air], 30, 1, 0.5f, 8.0f, false));	// DPS: 2 | CPDPS: 15
		towerInfo.Add(TowerType.Earth,
			new TowerInfo(towerSprites[TowerType.Earth], bulletSprites[TowerType.Earth], 40, 5, 2.0f, 2.5f, true)); // DPS: 2.5 | CPDPS: 16
		towerInfo.Add(TowerType.Fire,
			new TowerInfo(towerSprites[TowerType.Fire], bulletSprites[TowerType.Fire], 40, 2, 1.5f, 4.0f, false)); // DPS: 2 (+0.5) | CPDPS: 16
		towerInfo.Add(TowerType.Water,
			new TowerInfo(towerSprites[TowerType.Water], bulletSprites[TowerType.Water], 30, 2, 1.25f, 5.0f, false)); // DPS: 1.6 | CPDPS: 18.75, with slow

		// Tier 2 - one element (x2)
		towerInfo.Add(TowerType.Tornado,
			new TowerInfo(towerSprites[TowerType.Tornado], bulletSprites[TowerType.Air], 140, 2, 0.33f, 8.0f, false)); // DPS: 6.06 | CPDPS: 23.1
		towerInfo.Add(TowerType.Earthquake,
			new TowerInfo(towerSprites[TowerType.Earthquake], bulletSprites[TowerType.Earth], 150, 10, 1.5f, 3.5f, true)); // DPS: 6.66 | CPDPS: 22.5
		towerInfo.Add(TowerType.Flamethrower,
			new TowerInfo(towerSprites[TowerType.Flamethrower], bulletSprites[TowerType.Fire], 120, 5, 1.0f, 5.0f, false)); // DPS: 5 (+1) | CPDPS: 24
		towerInfo.Add(TowerType.Tsunami,
			new TowerInfo(towerSprites[TowerType.Tsunami], bulletSprites[TowerType.Water], 100, 4, 1.0f, 6.0f, false)); // DPS: 4 | CPDPS: 25, with slow

		// Tier 2 - two elements (combo)
		towerInfo.Add(TowerType.Blizzard,
			new TowerInfo(towerSprites[TowerType.Blizzard], bulletSprites[TowerType.Air], 130, 3, 0.6f, 6.0f, false)); // DPS: 5 | CPDPS: 26, with slow
		towerInfo.Add(TowerType.Flood,
			new TowerInfo(towerSprites[TowerType.Flood], bulletSprites[TowerType.Water], 110, 5, 1.25f, 4.0f, true)); // DPS: 4 | CPDPS: 27.5, with slow
		towerInfo.Add(TowerType.Volcano,
			new TowerInfo(towerSprites[TowerType.Volcano], bulletSprites[TowerType.Earth], 120, 6, 1.5f, 3.5f, true)); // DPS: 4 (+0.5) | CPDPS: 26.66
		towerInfo.Add(TowerType.Wildfire,
			new TowerInfo(towerSprites[TowerType.Wildfire], bulletSprites[TowerType.Fire], 150, 3, 0.5f, 6.0f, false)); // DPS: 6 (+0.5) | CPDPS: 23.07
		#endregion

		#region Upgrades
		// Tier 2 - single element 
		towerInfo[TowerType.Air].AddUpgrade(TowerType.Air, TowerType.Tornado);
		towerInfo[TowerType.Earth].AddUpgrade(TowerType.Earth, TowerType.Earthquake);
		towerInfo[TowerType.Fire].AddUpgrade(TowerType.Fire, TowerType.Flamethrower);
		towerInfo[TowerType.Water].AddUpgrade(TowerType.Water, TowerType.Tsunami);

		// Tier 2 - double element
		towerInfo[TowerType.Air].AddUpgrade(TowerType.Fire, TowerType.Wildfire);
		towerInfo[TowerType.Air].AddUpgrade(TowerType.Water, TowerType.Blizzard);
		towerInfo[TowerType.Earth].AddUpgrade(TowerType.Fire, TowerType.Volcano);
		towerInfo[TowerType.Earth].AddUpgrade(TowerType.Water, TowerType.Flood);
		towerInfo[TowerType.Fire].AddUpgrade(TowerType.Air, TowerType.Wildfire);
		towerInfo[TowerType.Fire].AddUpgrade(TowerType.Earth, TowerType.Volcano);
		towerInfo[TowerType.Water].AddUpgrade(TowerType.Air, TowerType.Blizzard);
		towerInfo[TowerType.Water].AddUpgrade(TowerType.Earth, TowerType.Flood);
		#endregion
	}
}
