using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Select();
	}

    /// <summary>
    /// Select this GameObject
    /// </summary>
	public void Select()
	{
        isSelected = true;

        // If this is a tile, change its color to yellow
        if(gameObject.tag == "Tile")
            GetComponent<SpriteRenderer>().color = Color.yellow;

        // Tell the Build Manager to select this gameobject
        BuildManager.instance.Select(gameObject);
    }

    // Deselect this GameObject
    public void Deselect()
	{
        isSelected = false;

        // If this is a tile, change its color back to gray
        if(gameObject.tag == "Tile")
            GetComponent<SpriteRenderer>().color = Color.gray;
	}
}
