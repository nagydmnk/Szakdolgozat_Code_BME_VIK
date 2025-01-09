using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<InteractableObject> interactableObjects;

    public List<MoveableItem> moveableItems;

    void Start()
    {
        // Initialize world objects if necessary
    }

    void Update()
    {
        // Update world object states if necessary
    }

    public void addIntearctableObject(InteractableObject newObj)
    {
        this.interactableObjects.Add(newObj);
    }

    public void removeInteractableObject(InteractableObject rmObj)
    {
        this.interactableObjects.Remove(rmObj);
    }

    public void addMoveAbleObject(MoveableItem newItem)
    {
        this.moveableItems.Add(newItem);
    }

    public void removeMoveableObject(MoveableItem rmItem)
    {
        this.moveableItems.Remove(rmItem); 
    }

    public InteractableObject getInteractByIndex(int index)
    {
        return this.interactableObjects[index];
    }

 

    public string getMoveableNames()
    {
        string names = null;
        foreach (var item in this.moveableItems)
        {
            //Ne szerepeljen olyan obj a prompt-ban, amelyikkel éppen dolgozik valaki.
            if(!item.isInUse())
                names += item.name + ",";
        }
        return names;
    }

    public string getInteractableNames()
    {
        string names = null;
        foreach (var item in interactableObjects)
        {
            names += item.name + ",";
        }
        return names;
    }

}
