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
    Tornado,
    Earthquake,
    Flamethrower,
    Tsunami,

    // Tier 2 - double element
    Wildfire,
    Volcano,
    Hurricane,
    Blizzard
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

    private GameObject[] towerPrefabs;
    private Dictionary<TowerType, TowerInfo> towerInfo;

    public Dictionary<TowerType, TowerInfo> TowerInfo { get { return towerInfo; } }

    // Start is called before the first frame update
    void Start()
    {
        towerPrefabs = Resources.LoadAll<GameObject>("Prefabs/GameObjects/Towers");
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
            new TowerInfo(GetTowerPrefab(TowerType.Air), TowerType.Air, 20, 1, 50));
        towerInfo.Add(TowerType.Earth, 
            new TowerInfo(GetTowerPrefab(TowerType.Earth), TowerType.Earth, 0, 0, 0));
        towerInfo.Add(TowerType.Fire,
            new TowerInfo(GetTowerPrefab(TowerType.Fire), TowerType.Fire, 0, 0, 0));
        towerInfo.Add(TowerType.Water,
            new TowerInfo(GetTowerPrefab(TowerType.Water), TowerType.Water, 0, 0, 0));

        // Tier 2 - single element
        towerInfo.Add(TowerType.Tornado,
            new TowerInfo(GetTowerPrefab(TowerType.Tornado), TowerType.Tornado, 0, 0, 0));
        towerInfo.Add(TowerType.Earthquake,
            new TowerInfo(GetTowerPrefab(TowerType.Earthquake), TowerType.Earthquake, 0, 0, 0));
        towerInfo.Add(TowerType.Flamethrower,
            new TowerInfo(GetTowerPrefab(TowerType.Flamethrower), TowerType.Flamethrower, 0, 0, 0));
        towerInfo.Add(TowerType.Tsunami,
            new TowerInfo(GetTowerPrefab(TowerType.Tsunami), TowerType.Tsunami, 0, 0, 0));

        // Tier 2 - double element
        towerInfo.Add(TowerType.Wildfire,
            new TowerInfo(GetTowerPrefab(TowerType.Wildfire), TowerType.Wildfire, 0, 0, 0));
        towerInfo.Add(TowerType.Volcano,
            new TowerInfo(GetTowerPrefab(TowerType.Volcano), TowerType.Volcano, 0, 0, 0));
        towerInfo.Add(TowerType.Hurricane,
            new TowerInfo(GetTowerPrefab(TowerType.Hurricane), TowerType.Hurricane, 0, 0, 0));
        towerInfo.Add(TowerType.Blizzard,
            new TowerInfo(GetTowerPrefab(TowerType.Blizzard), TowerType.Blizzard, 0, 0, 0));

        // === Add Upgrades ===
        // Tier 2 - single element 
        towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Tornado], TowerType.Air);
        towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Earthquake], TowerType.Earth);
        towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Flamethrower], TowerType.Fire);
        towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Tsunami], TowerType.Water);

        // Tier 2 - double element
        towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Wildfire], TowerType.Fire);
        towerInfo[TowerType.Air].AddUpgrade(towerInfo[TowerType.Hurricane], TowerType.Water);
        towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Volcano], TowerType.Fire);
        towerInfo[TowerType.Earth].AddUpgrade(towerInfo[TowerType.Blizzard], TowerType.Water);
        towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Wildfire], TowerType.Air);
        towerInfo[TowerType.Fire].AddUpgrade(towerInfo[TowerType.Volcano], TowerType.Earth);
        towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Hurricane], TowerType.Air);
        towerInfo[TowerType.Water].AddUpgrade(towerInfo[TowerType.Blizzard], TowerType.Earth);
    }

    /// <summary>
    /// Gets a tower's prefab
    /// </summary>
    /// <param name="type">The type of tower</param>
    /// <returns>A tower's prefab game object, null if not found</returns>
    private GameObject GetTowerPrefab(TowerType type)
	{
        for(int i = 0; i < towerPrefabs.Length; i++)
            if(towerPrefabs[i].GetComponent<Tower>().Type == type)
                return towerPrefabs[i];

        return null;
	}
}
