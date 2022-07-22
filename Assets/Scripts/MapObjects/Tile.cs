using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameObject tower;

    public GameObject Tower 
    { 
        get { return tower; } 
        set { tower = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
