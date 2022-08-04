using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour
{
	public bool isSelected;

	// Start is called before the first frame update
	void Start()
	{
		isSelected = false;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void OnMouseDown()
	{
		// Only select the game object if:
		// 1) It is the Game menu state
		// 2) The tower info panel is closed
		// 3) The mouse is not over UI elements
		if(GameManager.instance.CurrentMenuState == MenuState.Game
			&& !UIManager.instance.IsInfoPanelOpen
			&& !EventSystem.current.IsPointerOverGameObject())
			BuildManager.instance.Select(gameObject);
	}

	/// <summary>
	/// Select or deselect this GameObject
	/// </summary>
	/// <param name="select">Whether the gameobject is being selected or deselected</param>
	public void Select(bool select)
	{
		isSelected = select;

		// If this is a tile, update its color
		if(gameObject.tag == "Tile")
			gameObject.GetComponent<Tile>().UpdateColor(select);
	}
}
