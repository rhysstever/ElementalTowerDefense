using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton Code
    // A public reference to this script
    public static EnemyManager instance = null;

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
    private GameObject enemyParent, enemyPrefab;

    public GameObject EnemyParent { get { return enemyParent; } }

    // Start is called before the first frame update
    void Start()
    {
        CreateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Spawn an enemy into the scene
    /// </summary>
    private void CreateEnemy()
	{
        // Make the enemy spawn at the first checkpoint
        Vector2 startingPos = MapManager.instance.Checkpoints[0].transform.position;

        GameObject newEnemy = Instantiate(enemyPrefab, startingPos, Quaternion.identity, enemyParent.transform);
        newEnemy.name = "enemy" + (enemyParent.transform.childCount - 1);
	}
}
