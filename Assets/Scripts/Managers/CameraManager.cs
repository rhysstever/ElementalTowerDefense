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

    private void MoveCamera()
	{
        if(mainCam == null)
            return;

        Vector2 direction = Vector2.zero;

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

    private void CheckBounds()
	{
        Vector3 clampedPos = mainCam.transform.position;

        // Check Vertical Bounds
        if(clampedPos.y > yBounds)
            clampedPos.y = yBounds;
        else if(clampedPos.y < -yBounds)
            clampedPos.y = -yBounds;

        // Check Horizontal Bounds
        if(clampedPos.x > xBounds)
            clampedPos.x = xBounds;
        else if(clampedPos.x < -xBounds)
            clampedPos.x = -xBounds;

        mainCam.transform.position = clampedPos;
    }
}
