using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
	#region Singleton Code
	// A public reference to this script
	public static BulletManager instance = null;

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
    private GameObject bulletParent, bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/// <summary>
	/// Spawns a bullet into the scene
	/// </summary>
	/// <param name="tower">The game object tower the bullet is shot from</param>
	/// <param name="targetEnemy">The game object enemy the bullet is shot at</param>
    public void SpawnBullet(GameObject tower, GameObject targetEnemy)
	{
		Vector3 startingPos = tower.transform.position;

		GameObject newBullet = Instantiate(bulletPrefab, startingPos, Quaternion.identity, bulletParent.transform);
		newBullet.GetComponent<Bullet>().SetEndpoints(tower, targetEnemy);
	}
}
