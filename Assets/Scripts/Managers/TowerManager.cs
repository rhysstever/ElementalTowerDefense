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
		LoadSprites();
		CreateTowerInfoDictionary();
	}

	/// <summary>
	/// Link each tower type with the corresponding sprite, via dictionary
	/// </summary>
	private void LoadSprites()
	{
		// Load Tower Sprites
		towerSprites = new Dictionary<TowerType, Sprite>();
		Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/ElementIcons");

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
		// Tier 1
		towerInfo.Add(TowerType.Air,
			new TowerInfo(towerSprites[TowerType.Air], bulletSprites[TowerType.Air], 30, 1, 0.66f, 8.0f, false));
		towerInfo.Add(TowerType.Earth,
			new TowerInfo(towerSprites[TowerType.Earth], bulletSprites[TowerType.Earth], 20, 6, 2.0f, 1.5f, true));
		towerInfo.Add(TowerType.Fire,
			new TowerInfo(towerSprites[TowerType.Fire], bulletSprites[TowerType.Fire], 20, 3, 1.5f, 3.0f, false));
		towerInfo.Add(TowerType.Water,
			new TowerInfo(towerSprites[TowerType.Water], bulletSprites[TowerType.Water], 20, 2, 1.25f, 5.0f, false));

		// Tier 2 - one element (x2)
		towerInfo.Add(TowerType.Earthquake,
			new TowerInfo(towerSprites[TowerType.Earthquake], bulletSprites[TowerType.Earth], 150, 10, 1.5f, 3.5f, true));
		towerInfo.Add(TowerType.Flamethrower,
			new TowerInfo(towerSprites[TowerType.Flamethrower], bulletSprites[TowerType.Fire], 120, 5, 1.0f, 5.0f, false));
		towerInfo.Add(TowerType.Tornado,
			new TowerInfo(towerSprites[TowerType.Tornado], bulletSprites[TowerType.Air], 140, 2, 0.33f, 8.0f, false));
		towerInfo.Add(TowerType.Tsunami,
			new TowerInfo(towerSprites[TowerType.Tsunami], bulletSprites[TowerType.Water], 100, 4, 0.75f, 6.0f, false));

		// TODO: Set stats for combo towers
		// Tier 2 - two elements (combo)
		towerInfo.Add(TowerType.Blizzard,
			new TowerInfo(towerSprites[TowerType.Blizzard], bulletSprites[TowerType.Air], 999, 0, 0.0f, 0.0f, false));
		towerInfo.Add(TowerType.Flood,
			new TowerInfo(towerSprites[TowerType.Flood], bulletSprites[TowerType.Water], 999, 0, 0.0f, 0.0f, true));
		towerInfo.Add(TowerType.Volcano,
			new TowerInfo(towerSprites[TowerType.Volcano], bulletSprites[TowerType.Earth], 999, 0, 0.0f, 0.0f, true));
		towerInfo.Add(TowerType.Wildfire,
			new TowerInfo(towerSprites[TowerType.Wildfire], bulletSprites[TowerType.Fire], 999, 0, 0.0f, 0.0f, false));
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
