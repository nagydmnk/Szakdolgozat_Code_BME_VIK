using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public WorldManager Manager;
    public string ObjectName;
    public Vector3 Position;
    
    public WorldObject()
    {
        
    }
    public void Awake()
    {
        this.Manager = Camera.main.GetComponent<WorldManager>();
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
