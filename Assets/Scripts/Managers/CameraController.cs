using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private PlayerInput playerInput;
	private InputAction moveAction;

    private float moveSpeed;
    private float xBounds;
    private float yBounds;

	// Start is called before the first frame update
	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.actions["Movement"];

		moveSpeed = 2.0f;
        xBounds = 2.5f;
        yBounds = 2.5f;
    }

	private void FixedUpdate()
	{
		if(GameManager.instance.CurrentMenuState == MenuState.Game)
			MoveCamera(moveAction.ReadValue<Vector2>());
	}

	/// <summary>
	/// Moves the camera around the screen
	/// </summary>
	/// <param name="direction"></param>
	private void MoveCamera(Vector2 direction)
	{
		direction *= moveSpeed * Time.deltaTime;
		transform.position += new Vector3(direction.x, direction.y, 0.0f);
		CheckBounds();
	}

	/// <summary>
	/// Ensures the camera stays within bounds
	/// </summary>
	private void CheckBounds()
	{
		Vector3 clampedPos = transform.position;

		// Check Horizontal Bounds
		clampedPos.x = Mathf.Clamp(clampedPos.x, -xBounds, xBounds);

		// Check Vertical Bounds
		clampedPos.y = Mathf.Clamp(clampedPos.y, -yBounds, yBounds);

		transform.position = clampedPos;
	}
}
