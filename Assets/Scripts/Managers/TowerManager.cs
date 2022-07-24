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
		SetupTowerSpriteDictionary();
		CreateTowerInfoDictionary();
	}

	/// <summary>
	/// Link each tower type with the corresponding sprite, via dictionary
	/// </summary>
	private void SetupTowerSpriteDictionary()
	{
		towerSprites = new Dictionary<TowerType, Sprite>();
		Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Sprites/ElementIcons");

		for(int i = 0; i < loadedSprites.Length; i++)
		{
			string towerName = loadedSprites[i].name.Substring(4);
			TowerType type = (TowerType)System.Enum.Parse(typeof(TowerType), towerName);
			towerSprites.Add(type, loadedSprites[i]);
		}
	}

	/// <summary>
	/// Sets info for each type of tower
	/// </summary>
	private void CreateTowerInfoDictionary()
	{
		towerInfo = new Dictionary<TowerType, TowerInfo>();

		// === Create TowerInfo objects for each element ===
		// Tier 1
		towerInfo.Add(TowerType.Air,
			new TowerInfo(towerSprites[TowerType.Air], 20, 1, 0.33f, 8, false));
		towerInfo.Add(TowerType.Earth,
			new TowerInfo(towerSprites[TowerType.Earth], 20, 1, 0.33f, 8, true));
		towerInfo.Add(TowerType.Fire,
			new TowerInfo(towerSprites[TowerType.Fire], 20, 1, 0.33f, 8, false));
		towerInfo.Add(TowerType.Water,
			new TowerInfo(towerSprites[TowerType.Water], 20, 1, 0.33f, 8, false));

		// Tier 2 - single element
		towerInfo.Add(TowerType.Earthquake,
			new TowerInfo(towerSprites[TowerType.Earthquake], 20, 1, 0.33f, 8, true));
		towerInfo.Add(TowerType.Flamethrower,
			new TowerInfo(towerSprites[TowerType.Flamethrower], 20, 1, 0.33f, 8, false));
		towerInfo.Add(TowerType.Tornado,
			new TowerInfo(towerSprites[TowerType.Tornado], 20, 1, 0.33f, 8, false));
		towerInfo.Add(TowerType.Tsunami,
			new TowerInfo(towerSprites[TowerType.Tsunami], 20, 1, 0.33f, 8, false));

		// Tier 2 - double element
		towerInfo.Add(TowerType.Blizzard,
			new TowerInfo(towerSprites[TowerType.Blizzard], 20, 1, 0.33f, 8, false));
		towerInfo.Add(TowerType.Flood,
			new TowerInfo(towerSprites[TowerType.Flood], 20, 1, 0.33f, 8, true));
		towerInfo.Add(TowerType.Volcano,
			new TowerInfo(towerSprites[TowerType.Volcano], 20, 1, 0.33f, 8, true));
		towerInfo.Add(TowerType.Wildfire,
			new TowerInfo(towerSprites[TowerType.Wildfire], 20, 1, 0.33f, 8, false));

		// TODO: Add Upgrades
		//// === Add Upgrades ===
		//// Tier 2 - single element 
		//towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Tornado], TowerType.Air);
		//towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Earthquake], TowerType.Earth);
		//towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Flamethrower], TowerType.Fire);
		//towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Tsunami], TowerType.Water);

		//// Tier 2 - double element
		//towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Wildfire], TowerType.Fire);
		//towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Blizzard], TowerType.Water);
		//towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Volcano], TowerType.Fire);
		//towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Flood], TowerType.Water);
		//towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Wildfire], TowerType.Air);
		//towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Volcano], TowerType.Earth);
		//towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Blizzard], TowerType.Air);
		//towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Flood], TowerType.Earth);
	}
}
