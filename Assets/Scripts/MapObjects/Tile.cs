using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private GameObject tower;
	private Color selectedColor;
	private Color unselectedColor;

	public GameObject Tower
	{
		get { return tower; }
		set { tower = value; }
	}

	// Start is called before the first frame update
	void Start()
	{
		selectedColor = Color.yellow;
		unselectedColor = new Color(0.75f, 0.75f, 0.75f);

		UpdateColor(false);
	}

	// Update is called once per frame
	void Update()
	{

	}

	/// <summary>
	/// Updates the color of the tile
	/// </summary>
	/// <param name="isSelected">Whether the tile is being selected</param>
	public void UpdateColor(bool isSelected)
	{
		GetComponent<SpriteRenderer>().color = isSelected ? selectedColor : unselectedColor;
	}
}
