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

	// Properties
	public GameObject TowerPrefab { get { return towerPrefab; } }
	public Dictionary<TowerType, TowerInfo> TowerInfo { get { return towerInfo; } }

	// Start is called before the first frame update
	void Start()
	{
		SetupTowerSpriteDictionary();
		CreateTowerInfoDictionary();
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
			new TowerInfo(towerSprites[TowerType.Air], TowerType.Air, 20, 1, 50));
		towerInfo.Add(TowerType.Earth,
			new TowerInfo(towerSprites[TowerType.Earth], TowerType.Earth, 0, 0, 0));
		towerInfo.Add(TowerType.Fire,
			new TowerInfo(towerSprites[TowerType.Fire], TowerType.Fire, 0, 0, 0));
		towerInfo.Add(TowerType.Water,
			new TowerInfo(towerSprites[TowerType.Water], TowerType.Water, 0, 0, 0));

		// TODO: Tier 2 - single element

		// TODO: Tier 2 - double element

		// TODO: Tower Upgrades
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
}
