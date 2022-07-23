using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField]
	private Camera mainCam;

	private float moveSpeed;
	private float xBounds;
	private float yBounds;

	// Start is called before the first frame update
	void Start()
	{
		moveSpeed = 2.0f;
		xBounds = 2.5f;
		yBounds = 2.5f;
	}

	void FixedUpdate()
	{
		MoveCamera();
	}

	/// <summary>
	/// Moves the camera laterally around the scene
	/// </summary>
	private void MoveCamera()
	{
		if(mainCam == null)
			return;

		Vector2 direction = Vector2.zero;

		// TODO: Use Unity Input System
		// Up
		if(Input.GetKey(KeyCode.W))
			direction.y += 1;

		// Down
		if(Input.GetKey(KeyCode.S))
			direction.y -= 1;

		// Left
		if(Input.GetKey(KeyCode.A))
			direction.x -= 1;

		// Right
		if(Input.GetKey(KeyCode.D))
			direction.x += 1;

		direction.Normalize();
		direction *= moveSpeed * Time.deltaTime;
		mainCam.transform.position += new Vector3(direction.x, direction.y, 0.0f);
		CheckBounds();
	}

	/// <summary>
	/// Ensures the camera stays within bounds
	/// </summary>
	private void CheckBounds()
	{
		Vector3 clampedPos = mainCam.transform.position;

		// Check Horizontal Bounds
		clampedPos.x = Mathf.Clamp(clampedPos.x, -xBounds, xBounds);

		// Check Vertical Bounds
		clampedPos.y = Mathf.Clamp(clampedPos.y, -yBounds, yBounds);

		mainCam.transform.position = clampedPos;
	}
}
