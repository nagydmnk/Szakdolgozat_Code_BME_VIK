using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMemory : MonoBehaviour
{

    private InteractableObject intObj;
    private int neededCubes = 0;
    private int neededSpheres = 0;
    
    public void setInteractableObject(InteractableObject newObj)
    {
        this.intObj = newObj;
    } 

    public void setNeededCubes(int Cube){this.neededCubes = Cube;}

    public void setNeededSpheares(int Sphere){this.neededSpheres = Sphere;}

    public CharacterMemory(InteractableObject intObj)
    {
        this.intObj=intObj;
    }

    public string getMemoryString()
    {
        return this.intObj.name + ":" + neededCubes + "Cube," + neededSpheres + "Sphere;";
    }

    public InteractableObject getInteractableObject() { return this.intObj; }
}
