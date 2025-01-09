using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MoveableItem : WorldObject
{
    public float arrivalTreshold = 0.5f;
    private Character pickUpCharacter = null;
    private bool waitingForPickup = false;
    public string type = "Sphere";

    public bool isInUse()
    {
        if(waitingForPickup || pickUpCharacter != null)
        {
            return true;
        }
        return false;
    }
    public void pickUp()
    {
        if (waitingForPickup)
        {
            Debug.Log("Picking up item: " + ObjectName);
            transform.SetParent(pickUpCharacter.transform); 
            transform.localPosition = new Vector3(0, 1, 0); 
            gameObject.SetActive(true); 
            this.waitingForPickup = false;
        }
        
    }

    public void putDown()
    {
        Debug.Log("Putting down item: " + ObjectName);
        transform.SetParent(null); 
        transform.position = pickUpCharacter.transform.position; 
        gameObject.SetActive(true); 
        this.pickUpCharacter = null;
    }

    public void Hide()
    {
        Debug.Log("Hiding item: " + ObjectName);
        transform.SetParent(null); 
        transform.position = pickUpCharacter.transform.position;
        gameObject.SetActive(false); 
        this.pickUpCharacter = null;
    }

    public void StartPickupProcess(Character pickupCharacter)
    {
        this.pickUpCharacter = pickupCharacter;
        this.waitingForPickup = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForPickup && pickUpCharacter != null)
        {
            float dist = Vector3.Distance(pickUpCharacter.transform.position, this.transform.position);
            if(dist <= arrivalTreshold)
            {
                pickUp();
            }
        }
    }
}
