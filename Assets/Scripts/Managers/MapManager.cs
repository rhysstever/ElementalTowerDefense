using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static MapManager instance = null;

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
    private GameObject checkpointsParent, tilesParent;
    [SerializeField]    // Map Prefabs
    private GameObject tilePrefab, checkpointPrefab;

    private float xOffset, yOffset;
    private Vector2 startingPos;

    private GameObject[] checkpoints;

    public GameObject[] Checkpoints { get { return checkpoints; } }

    // Start is called before the first frame update
    void Start()
    {
        xOffset = 1.0f;
        yOffset = 1.0f;

        startingPos = new Vector2(-8.0f, 4.0f);

        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates and instantiates a map
    /// </summary>
    private void CreateMap()
	{
        // Define the map and spawn it
        string[,] mapLayout = new string[,] {
            { " ", " ", " ",  " ", " ", " ", " ",  " ", " ", " ", " ",  " ", " ", " ", " ",  " ", " " },
            { " ", " ", "C0", " ", " ", " ", "C3", "=", "=", "=", "C4", " ", " ", " ", "C7", " ", " " },
            { " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " " },
            { " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " " },
            { " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " " },
            { " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " " },
            { " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " ", " ", "=",  " ", " " },
            { " ", " ", "C1", "=", "=", "=", "C2", " ", " ", " ", "C5", "=", "=", "=", "C6", " ", " " },
            { " ", " ", " ",  " ", " ", " ", " ",  " ", " ", " ", " ",  " ", " ", " ", " ",  " ", " " }
        };

        SpawnMap(mapLayout, 8);
    }

    /// <summary>
    /// Instantiates a map into the scene
    /// </summary>
    /// <param name="mapLayout">The map info</param>
    /// <param name="checkpointCount">The number of checkpoints</param>
    private void SpawnMap(string[,] mapLayout, int checkpointCount)
	{
        checkpoints = new GameObject[checkpointCount];

        for(int r = 0; r < mapLayout.GetLength(0); r++)
        {
            for(int c = 0; c < mapLayout.GetLength(1); c++)
            {
                switch(mapLayout[r, c].Substring(0, 1))
                {
                    // Checkpoint
                    case "C":
                        CreateCheckpoint(
                            IndeciesIntoPosition(r, c) + startingPos, 
                            int.Parse(mapLayout[r, c].Substring(1, 1)));
                        break;
                    // Path
                    case "=":
                        break;
                    // Tile
                    default:
                        CreateTile(IndeciesIntoPosition(r, c) + startingPos);
                        break;
                }
            }
        }

        LinkCheckpoints();
    }

    /// <summary>
    /// Creates a checkpoint
    /// </summary>
    /// <param name="position">The position of the checkpoint</param>
    /// <param name="checkpointNumber">Number of CURRENT checkpoints, including this one (starting at 0)</param>
    /// <returns>A checkpoint, newly created in the scene</returns>
    private GameObject CreateCheckpoint(Vector2 position, int checkpointNumber)
	{
        // Create the checkpoint
        GameObject newCheckpoint = Instantiate(checkpointPrefab, position, Quaternion.identity, checkpointsParent.transform);
        newCheckpoint.name = "checkpoint" + checkpointNumber;
        checkpoints[checkpointNumber] = newCheckpoint;

        return newCheckpoint;
	}

    /// <summary>
    /// Creates a tile
    /// </summary>
    /// <param name="position">The position of the tile</param>
    /// <returns>A tile, newly created in the scene</returns>
    private GameObject CreateTile(Vector2 position)
	{
        GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity, tilesParent.transform);
        newTile.name = "tile" + (tilesParent.transform.childCount - 1);

        return newTile;
    }

    /// <summary>
    /// Links checkpoints to the previous and next checkpoint
    /// </summary>
    private void LinkCheckpoints()
	{
        for(int i = 0; i < checkpoints.Length; i++)
		{
            // If the checkpoint is not the first checkpoint, 
            // attach this checkpoint as the preivous checkpoint's Next
            if(i > 0)
                checkpoints[i - 1].GetComponent<Checkpoint>().Next = checkpoints[i].gameObject;

            // If the checkpoint is not the last checkpoint,
            // attach the next checkpoint as this checkpoint's Next
            if(i < checkpointsParent.transform.childCount - 1)
                checkpoints[i].GetComponent<Checkpoint>().Next = checkpoints[i + 1].gameObject;
        }
	}

    // === Helper Methods ===

    /// <summary>
    /// A helper method for converting indecies into position
    /// </summary>
    /// <param name="row">The row index</param>
    /// <param name="column">The column index</param>
    /// <returns>The corresponding position in world space</returns>
    private Vector2 IndeciesIntoPosition(int row, int column)
	{
        return new Vector2(
            column * xOffset,
            row * -yOffset);
    }
}
