using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHelpers
{
	/// <summary>
	/// Destroys all child objects of a parent
	/// </summary>
	/// <param name="parent">The parent game object</param>
	public static void ClearParent(GameObject parent)
	{
		for(int i = parent.transform.childCount - 1; i >= 0; i--)
		{
			// Create a reference to the child
			Transform child = parent.transform.GetChild(i);
			// Remove the parent object from the child
			child.parent = null;
			// Destroy the child object
			GameObject.Destroy(child.gameObject);
		}
	}
}
