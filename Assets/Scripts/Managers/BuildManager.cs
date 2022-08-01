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
		//TODO: Remove when buy/sell buttons are created
		if(Input.GetKeyDown(KeyCode.E))
			Build(TowerManager.instance.SelectedTypeInfo);
		else if(Input.GetKeyDown(KeyCode.Q))
			Sell();
	}

	/// <summary>
	/// Selects a new gameObject
	/// </summary>
	/// <param name="selection">The gameObject that is selected</param>
	public void Select(GameObject selection)
	{
		if(currentSelection != null
			&& currentSelection.GetComponent<Selectable>() != null)
			currentSelection.GetComponent<Selectable>().Deselect();

		currentSelection = selection;

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
		// Check that the current selection is a tile
		if(currentSelection == null
			|| currentSelection.tag != "Tile")
		{
			Debug.Log("You can only build on a tile!");
			return false;
		}

		// Check that the tile does not already have a tower built on it
		if(currentSelection.GetComponent<Tile>().Tower
			!= null)
		{
			Debug.Log("This tile already has a tower built on it!");
			return false;
		}

		// Check that the player has enough money to afford the tower
		int cost = TowerManager.instance.TowerInfo[type].Cost;
		if(GameManager.instance.money < cost)
		{
			Debug.Log("You do not have enough money!");
			return false;
		}

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

		// Remove the money from the player
		GameManager.instance.money -= TowerManager.instance.TowerInfo[type].Cost;

		// Create the tower 
		GameObject newTower = Instantiate(
			TowerManager.instance.TowerPrefab,
			currentSelection.transform.position,
			Quaternion.identity,
			towersParent.transform);
		newTower.name = type + " Tower";

		// Setup Tower
		newTower.GetComponent<Tower>().SetupTower(type);

		// Link the tile and tower to each other
		currentSelection.GetComponent<Tile>().Tower = newTower;
		newTower.GetComponent<Tower>().Tile = currentSelection;

		// Deactivate the tile and select the new tower
		currentSelection.SetActive(false);
		Select(newTower);

		// Update player stats UI
		UIManager.instance.UpdatePlayerStatsText();
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
		GameManager.instance.money += cost / 2;

		// Select the tile under the tower
		Select(currentSelection.GetComponent<Tower>().Tile);
		currentSelection.SetActive(true);

		// Destroy the tower and unlink it from the tile
		Destroy(currentSelection.GetComponent<Tile>().Tower);
		currentSelection.GetComponent<Tile>().Tower = null;

		// Update player stats UI
		UIManager.instance.UpdatePlayerStatsText();
	}
}
