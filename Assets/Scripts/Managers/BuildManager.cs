using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static BuildManager instance = null;

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

	[SerializeField]    // Object parents
	private GameObject towersParent, bulletsParent;

	private GameObject currentSelection;

	// Properties
	public GameObject CurrentSelection { get { return currentSelection; } }

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Selects a new gameObject
	/// </summary>
	/// <param name="selection">The gameObject that is selected</param>
	public void Select(GameObject selection)
	{
		if(currentSelection != null
			&& currentSelection.GetComponent<Selectable>() != null)
			currentSelection.GetComponent<Selectable>().Select(false);

		currentSelection = selection;

		if(currentSelection != null
			&& currentSelection.GetComponent<Selectable>() != null)
		{
			currentSelection.SetActive(true);
			currentSelection.GetComponent<Selectable>().Select(true);
		}

		// Update UI
		UIManager.instance.UpdateSelectedObjectUI(currentSelection);
	}

	/// <summary>
	/// Checks if the player can buy a certain tower
	/// </summary>
	/// <param name="type">The type of tower</param>
	/// <returns>Whether the player can buy the tower</returns>
	private bool CanBuild(TowerType type)
	{
		// Check that there is a current selection
		if(currentSelection == null)
			return false;

		// If the current selection is a tower,
		// try to upgrade
		if(currentSelection.tag == "Tower")
		{
			Upgrade(type);
			return false;
		}
		// Otherwise, check that the current selection is a tile
		else if(currentSelection.tag != "Tile")
			return false;

		// Check that the tile does not already have a tower built on it
		if(currentSelection.GetComponent<Tile>().Tower != null)
			return false;

		// Check that the player has enough money to afford the tower
		int cost = TowerManager.instance.TowerInfo[type].Cost;
		if(GameManager.instance.Money < cost)
			return false;

		return true;
	}

	/// <summary>
	/// Builds a tower
	/// </summary>
	/// <param name="type">The type of tower</param>
	public void Build(TowerType type)
	{
		if(!CanBuild(type))
			return;

		// Remove the cost from the player
		GameManager.instance.UpdateMoney(-TowerManager.instance.TowerInfo[type].Cost);

		// Create the tower
		GameObject newTower = CreateTower(type);

		// Link the tile and tower to each other
		currentSelection.GetComponent<Tile>().Tower = newTower;
		newTower.GetComponent<Tower>().Tile = currentSelection;

		// Hide the tile and select the new tower
		currentSelection.SetActive(false);
		Select(newTower);
	}

	/// <summary>
	/// Checks if the player can upgrade a tower
	/// </summary>
	/// <param name="type">The secondary element</param>
	/// <returns>Whether the upgrade can be bought</returns>
	private bool CanUpgrade(TowerType type)
	{
		// Check if the upgrade exists
		TowerType baseType = currentSelection.GetComponent<Tower>().Type;
		if(!TowerManager.instance.TowerInfo[baseType].Upgrades.ContainsKey(type))
		{
			Debug.Log("Upgrade does not exist!");
			return false;
		}

		// Check if the player has enough money
		TowerType upgradeType = TowerManager.instance.TowerInfo[baseType].Upgrades[type];
		int cost = TowerManager.instance.TowerInfo[upgradeType].Cost;
		if(GameManager.instance.Money < cost)
		{
			Debug.Log("Not enough money! " + cost + " needed.");
			return false;
		}

		return true;
	}

	/// <summary>
	/// Upgrades a tower
	/// </summary>
	/// <param name="type">The secondary tower type (not already built)</param>
	private void Upgrade(TowerType type)
	{
		// Check that the upgrade can happen
		if(!CanUpgrade(type))
			return;

		TowerType baseType = currentSelection.GetComponent<Tower>().Type;
		TowerType upgradeType = TowerManager.instance.TowerInfo[baseType].Upgrades[type];

		// Remove the remaining cost from the player based on what is already built
		int remainingCost = TowerManager.instance.TowerInfo[upgradeType].Cost - TowerManager.instance.TowerInfo[baseType].Cost;
		GameManager.instance.UpdateMoney(-remainingCost);

		// Create the upgraded tower
		GameObject newTower = CreateTower(upgradeType);

		// Select the tile under the tower (temporarily)
		Select(currentSelection.GetComponent<Tower>().Tile);

		// Destroy the base tower object
		Destroy(currentSelection.GetComponent<Tile>().Tower);

		// Link the new tower and the tile it is on to each other
		currentSelection.GetComponent<Tile>().Tower = newTower;
		newTower.GetComponent<Tower>().Tile = currentSelection;

		// Hide the tile and select the newly built upgrade tower
		currentSelection.SetActive(false);
		Select(newTower);
	}

	/// <summary>
	/// Creates a tower in the scene
	/// </summary>
	/// <param name="type">The tower type being created</param>
	/// <returns>The created tower game object</returns>
	private GameObject CreateTower(TowerType type)
	{
		// Create the tower 
		GameObject newTower = Instantiate(
			TowerManager.instance.TowerPrefab,
			currentSelection.transform.position,
			Quaternion.identity,
			towersParent.transform);
		newTower.name = type + " Tower";

		// Setup Tower
		newTower.GetComponent<Tower>().SetupTower(type);

		// return the newly built tower
		return newTower;
	}

	/// <summary>
	/// Sells the current tower
	/// </summary>
	public void Sell()
	{
		// Check that the current selection is a tower
		if(currentSelection == null
			|| currentSelection.tag != "Tower")
		{
			Debug.Log("You can only sell towers!");
			return;
		}

		// Give the player 1/2 the cost of the tower
		int cost = TowerManager.instance.TowerInfo[currentSelection.GetComponent<Tower>().Type].Cost;
		GameManager.instance.UpdateMoney(cost / 2);

		// Select the tile under the tower
		Select(currentSelection.GetComponent<Tower>().Tile);

		// Destroy the tower and unlink it from the tile
		Destroy(currentSelection.GetComponent<Tile>().Tower);
		currentSelection.GetComponent<Tile>().Tower = null;
	}
}
