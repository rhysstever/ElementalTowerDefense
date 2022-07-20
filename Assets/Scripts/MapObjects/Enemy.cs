using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject currentCheckpoint;
    public bool canMove;

    private float health;
    private float moveSpeed;
    private float damage;

    // Start is called before the first frame update
    void Start()
    {
        currentCheckpoint = MapManager.instance.Checkpoints[1];
        canMove = true;

        health = 10.0f;
        moveSpeed = 2.0f;
        damage = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void FixedUpdate()
	{
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Lose Health if it has collided with a bullet
        if(collision.gameObject.tag == "Bullet")
        {

        }

        // If it has reached the base (last checkpoint)
        if(collision.gameObject == currentCheckpoint
            && currentCheckpoint.GetComponent<Checkpoint>().Next == null)
            ReachedDestination();
    }

    /// <summary>
    /// Moves the enemy in the direction of its target checkpoint
    /// </summary>
	private void Move()
	{
		if(canMove)
		{
            Vector2 direction = currentCheckpoint.transform.position - transform.position;
            direction.Normalize();
            direction *= moveSpeed * Time.deltaTime;

            transform.position += new Vector3(direction.x, direction.y, 0.0f);
		}
	}

    /// <summary>
    /// Handles the enemy reaching its target checkpoint
    /// </summary>
    public void ReachedDestination()
	{
        if(currentCheckpoint.GetComponent<Checkpoint>().Next != null)
            currentCheckpoint = currentCheckpoint.GetComponent<Checkpoint>().Next;
        else 
            Destroy(gameObject);
	}
}
