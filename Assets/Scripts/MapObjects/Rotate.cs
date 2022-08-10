using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	private bool rotateClockwise;
	private float speedModifier;
	private float rotationSpeed;

	private void Start()
	{
		speedModifier = 20.0f;
	}

	private void FixedUpdate()
	{
		rotationSpeed = Time.deltaTime * speedModifier;
		rotationSpeed *= rotateClockwise ? 1.0f : -1.0f;
		transform.RotateAround(transform.position, transform.forward, rotationSpeed);
	}
}
