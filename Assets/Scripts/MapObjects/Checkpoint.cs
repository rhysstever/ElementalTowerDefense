using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameObject nextCheckpoint;
    
    public GameObject Next 
    { 
        get { return nextCheckpoint; } 
        set { nextCheckpoint = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
