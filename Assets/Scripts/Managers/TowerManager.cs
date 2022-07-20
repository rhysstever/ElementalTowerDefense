using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]    // Object parents
    private GameObject towersParent, bulletsParent;
    [SerializeField]    // Tower Prefabs
    private GameObject towerPrefab;

    private GameObject currentSelection;

    public GameObject CurrentSelection { get { return currentSelection;} }

    // Start is called before the first frame update
    void Start()
    {
        currentSelection = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
            Build();
        else if(Input.GetKeyDown(KeyCode.S))
            Sell();
    }

    /// <summary>
    /// Selects a new gameObject
    /// </summary>
    /// <param name="selection">The gameObject that is selected</param>
	public void Select(GameObject selection)
	{
		currentSelection = selection;
	}

    /// <summary>
    /// Builds a tower
    /// </summary>
    public void Build()
	{
        // Check that the current selection is a tile
        if(currentSelection == null
            || currentSelection.tag != "Tile")
		{
            Debug.Log("You can only build on a tile!");
            return;
		}

        // Create the tower 
        GameObject newTower = Instantiate(towerPrefab, currentSelection.transform.position, Quaternion.identity, towersParent.transform);
        newTower.name = "tower" + (towersParent.transform.childCount - 1);

        // Link the tile and tower to each other
        currentSelection.GetComponent<Tile>().Tower = newTower;
        newTower.GetComponent<Tower>().Tile = currentSelection;

        // Deactivate the tile and select the new tower
        currentSelection.SetActive(false);
        Select(newTower);
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

        // Select the tile under the tower
        Select(currentSelection.GetComponent<Tower>().Tile);
        currentSelection.SetActive(true);

        // Destroy the tower and unlink it from the tile
        Destroy(currentSelection.GetComponent<Tile>().Tower);
        currentSelection.GetComponent<Tile>().Tower = null;
    }
}
