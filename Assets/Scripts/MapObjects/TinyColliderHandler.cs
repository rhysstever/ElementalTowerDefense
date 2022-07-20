using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyColliderHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        // If the enemy has gotten to its target checkpoint
        if(collision.gameObject
            == transform.parent.GetComponent<Enemy>().currentCheckpoint)
            transform.parent.GetComponent<Enemy>().ReachedDestination();
    }
}
