using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField]
	private TowerType type;
	private GameObject tile;

	// Properties
	public TowerType Type { get { return type; } }
	public GameObject Tile
	{
		get { return tile; }
		set { tile = value; }
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetupTower(TowerType type)
	{
		// Set type and sprite
		this.type = type;
		GetComponent<SpriteRenderer>().sprite = TowerManager.instance.TowerInfo[type].Sprite;
	}
}